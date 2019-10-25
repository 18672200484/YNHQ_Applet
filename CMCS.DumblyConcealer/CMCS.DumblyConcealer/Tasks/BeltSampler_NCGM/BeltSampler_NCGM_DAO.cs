using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.DumblyConcealer.Tasks.BeltSampler_NCGM.Entities;
using CMCS.Common.DAO;
using CMCS.Common;
using CMCS.Common.Entities;
using CMCS.Common.Enums;
using CMCS.DumblyConcealer.Tasks.BeltSampler_NCGM.Enums;
using CMCS.DumblyConcealer.Enums;

namespace CMCS.DumblyConcealer.Tasks.BeltSampler_NCGM
{
    /// <summary>
    /// 南昌光明火车皮带采样机接口业务
    /// </summary>
    public class BeltSampler_NCGM_DAO
    {
        private static BeltSampler_NCGM_DAO instance;

        public static BeltSampler_NCGM_DAO GetInstance()
        {
            if (instance == null)
            {
                instance = new BeltSampler_NCGM_DAO();
            }
            return instance;
        }

        private BeltSampler_NCGM_DAO()
        { }

        /// <summary>
        /// 是否处于故障状态
        /// </summary>
        bool IsHitch = false;

        #region 数据转换方法（此处有点麻烦，后期调整接口方案）

        /// <summary>
        /// 第三方接口设备编码 转换成 集中管控设备编码
        /// </summary>
        /// <param name="machineCode">接口表设备编码</param>
        /// <returns></returns>
        public string ConvertToCmcsMachineCode(string machineCode)
        {
            if (machineCode == eMachineCode.HCPDCYJ01.ToString())
                return GlobalVars.MachineCode_NCGM_HCPDCYJ_1;
            else if (machineCode == eMachineCode.HCPDCYJ02.ToString())
                return GlobalVars.MachineCode_NCGM_HCPDCYJ_2;

            return string.Empty;
        }

        /// <summary>
        /// 集中管控设备编码 转换成 第三方接口设备编码
        /// </summary>
        /// <param name="machineCode">集中管控设备编码</param>
        /// <returns></returns>
        public string ConvertToInfMachineCode(string machineCode)
        {
            if (machineCode == GlobalVars.MachineCode_NCGM_HCPDCYJ_1)
                return eMachineCode.HCPDCYJ01.ToString();
            else if (machineCode == GlobalVars.MachineCode_NCGM_HCPDCYJ_2)
                return eMachineCode.HCPDCYJ02.ToString();

            return string.Empty;
        }

        /// <summary>
        /// 转换成第三方接口-煤种
        /// </summary>
        /// <param name="fuelKindName">煤种</param>
        /// <returns></returns>
        public int ConvertToInfFuelKindName(string fuelKindName)
        {
            eFuelKindName enumResulr;
            if (Enum.TryParse(fuelKindName, out enumResulr))
                return (int)enumResulr;
            else
                return (int)eFuelKindName.其它;
        }

        /// <summary>
        /// 转换成第三方接口-水分
        /// </summary>
        /// <param name="mt">水分</param>
        /// <returns></returns>
        public int ConvertToInfMt(double mt)
        {
            // TODO

            return (int)eMt.干煤;
        }

        /// <summary>
        /// 转换成第三方接口-采样方式
        /// </summary>
        /// <param name="sampleType">采样方式</param>
        /// <returns></returns>
        public int ConvertToInfSampleType(string sampleType)
        {
            eEquInfSampleType enumResulr;
            if (Enum.TryParse(sampleType, out enumResulr))
                return (int)enumResulr;
            else
                return (int)eEquInfSampleType.到集样罐;
        }

        #endregion

        /// <summary>
        /// 同步实时信号到集中管控
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public int SyncSignalDatal(Action<string, eOutputType> output)
        {
            int res = 0;

            foreach (InfPDCYSignal entity in DcDbers.GetInstance().BeltSampler_NCGM_Dber.Entities<InfPDCYSignal>(""))
            {
                // 当心跳检测为故障时，则不更新系统状态，保持 eSampleSystemStatus.发生故障
                if (entity.TagName == GlobalVars.EquSystemStatueName && IsHitch) continue;

                string tagValue = entity.TagValue;
                if (entity.TagName == GlobalVars.EquSystemStatueName)
                    tagValue = ((eEquInfSamplerSystemStatus)Convert.ToInt32(tagValue)).ToString();

                res += CommonDAO.GetInstance().SetSignalDataValue(ConvertToCmcsMachineCode(entity.MachineCode), entity.TagName, tagValue) ? 1 : 0;
            }
            output(string.Format("同步实时信号 {0} 条", res), eOutputType.Normal);

            return res;
        }

        /// <summary>
        /// 获取上位机运行状态表 - 心跳值
        /// 每隔30s读取该值，如果数值不变化则表示设备上位机出现故障
        /// </summary>
        /// <returns></returns>
        public string GetDataFlag()
        {
            InfPDCYDataFlag pDCYDataFlag = DcDbers.GetInstance().BeltSampler_NCGM_Dber.Entity<InfPDCYDataFlag>("");
            if (pDCYDataFlag != null) return pDCYDataFlag.DataFlag.ToString();

            return string.Empty;
        }

        /// <summary>
        /// 改变系统状态值
        /// </summary>
        /// <param name="isHitch">是否故障</param>
        public void ChangeSystemHitchStatus(bool isHitch)
        {
            IsHitch = isHitch;

            if (IsHitch)
            {
                CommonDAO.GetInstance().SetSignalDataValue(ConvertToCmcsMachineCode(eMachineCode.HCPDCYJ01.ToString()), GlobalVars.EquSystemStatueName, eEquInfSamplerSystemStatus.发生故障.ToString());
                CommonDAO.GetInstance().SetSignalDataValue(ConvertToCmcsMachineCode(eMachineCode.HCPDCYJ02.ToString()), GlobalVars.EquSystemStatueName, eEquInfSamplerSystemStatus.发生故障.ToString());
            }
        }

        /// <summary>
        /// 同步集样罐信息到集中管控
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncBarrel(Action<string, eOutputType> output)
        {
            int res = 0;
            List<InfPDCYBarrel> infpdcybarrels = DcDbers.GetInstance().BeltSampler_NCGM_Dber.Entities<InfPDCYBarrel>("where DataFlag=0");
            foreach (InfPDCYBarrel entity in infpdcybarrels)
            {
                if (CommonDAO.GetInstance().SaveEquInfSampleBarrel(new CmcsEquInfSampleBarrel
                 {
                     BarrelNumber = entity.BarrelNumber,
                     BarreStatus = ((eSampleBarrelStatus)entity.IsFull).ToString(),
                     MachineCode = ConvertToCmcsMachineCode(entity.MachineCode),
                     InFactoryBatchId = entity.InFactoryBatchId,
                     InterfaceType = CommonDAO.GetInstance().GetMachineInterfaceTypeByCode(ConvertToCmcsMachineCode(entity.MachineCode)),
                     IsCurrent = entity.IsCurrent,
                     SampleCode = entity.SampleCode,
                     SampleCount = entity.SampleCount,
                     UpdateTime = entity.UpdateTime
                 }))
                {

                    entity.DataFlag = 1;
                    DcDbers.GetInstance().BeltSampler_NCGM_Dber.Update(entity);

                    res++;
                }
            }
            int fullInfPDCYBarrel = DcDbers.GetInstance().BeltSampler_NCGM_Dber.Entities<InfPDCYBarrel>(" where  isFull=1").Count;
            if (fullInfPDCYBarrel == 4 || fullInfPDCYBarrel == 5)
            {
                if (!CommonDAO.GetInstance().hasSysMessage(eMessageType.火车皮带采样机.ToString(), "皮带采样机采样桶剩余空桶数不超过2个!"))
                {
                    CommonDAO.GetInstance().SaveSysMessage(eMessageType.火车皮带采样机.ToString(), "皮带采样机采样桶剩余空桶数不超过2个!", eMessageType.轨道衡.ToString());
                }
            }
            else if (fullInfPDCYBarrel == 6)
            {
                if (!CommonDAO.GetInstance().hasSysMessage(eMessageType.火车皮带采样机.ToString(), "皮带采样机采样全部桶已满!"))
                {
                    CommonDAO.GetInstance().SaveSysMessage(eMessageType.火车皮带采样机.ToString(), "皮带采样机采样全部桶已满!", eMessageType.轨道衡.ToString(), "查看|取消", false);
                }
            }
            output(string.Format("同步集样罐记录 {0} 条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步故障信息到集中管控
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncEquInfHitch(Action<string, eOutputType> output)
        {
            int res = 0;

            foreach (InfPDCYEquInfHitch entity in DcDbers.GetInstance().BeltSampler_NCGM_Dber.Entities<InfPDCYEquInfHitch>("where DataFlag=0"))
            {
                if (CommonDAO.GetInstance().SaveEquInfHitch(ConvertToCmcsMachineCode(entity.MachineCode), entity.ErrorTime, "故障代码 " + entity.ErrorCode + "，" + entity.ErrorDescribe))
                {
                    entity.DataFlag = 1;
                    DcDbers.GetInstance().BeltSampler_NCGM_Dber.Update(entity);

                    res++;
                }
            }

            output(string.Format("同步故障信息记录 {0} 条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步采样计划
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncPDCYPlan(Action<string, eOutputType> output)
        {
            int res = 0;

            // 集中管控 > 第三方 
            foreach (CmcsBeltSamplePlan entity in BeltSamplerDAO.GetInstance().GetWaitForSyncBeltSamplePlan(GlobalVars.InterfaceType_NCGM_HCPDCYJ))
            {
                bool isSuccess = false;

                InfPDCYPlan pDCYPlan = DcDbers.GetInstance().BeltSampler_NCGM_Dber.Get<InfPDCYPlan>(entity.Id);
                if (pDCYPlan == null)
                {
                    isSuccess = DcDbers.GetInstance().BeltSampler_NCGM_Dber.Insert(new InfPDCYPlan
                    {
                        // 保持相同的Id
                        Id = entity.Id,
                        CarCount = entity.CarCount,
                        DataFlag = 0,
                        CreateDate = entity.CreateDate,
                        FuelKindName = ConvertToInfFuelKindName(entity.FuelKindName),
                        InFactoryBatchId = entity.InFactoryBatchId,
                        Mt = ConvertToInfMt(entity.Mt),
                        SampleCode = entity.SampleCode,
                        SampleType = ConvertToInfSampleType(entity.SampleType),
                        TicketWeight = entity.TicketWeight
                    }) > 0;
                }
                else
                {
                    pDCYPlan.CarCount = entity.CarCount;
                    pDCYPlan.DataFlag = 0;
                    pDCYPlan.CreateDate = entity.CreateDate;
                    pDCYPlan.FuelKindName = ConvertToInfFuelKindName(entity.FuelKindName);
                    pDCYPlan.InFactoryBatchId = entity.InFactoryBatchId;
                    pDCYPlan.Mt = ConvertToInfMt(entity.Mt);
                    pDCYPlan.SampleCode = entity.SampleCode;
                    pDCYPlan.SampleType = ConvertToInfSampleType(entity.SampleType);
                    pDCYPlan.TicketWeight = entity.TicketWeight;

                    isSuccess = DcDbers.GetInstance().BeltSampler_NCGM_Dber.Update(pDCYPlan) > 0;
                }

                if (isSuccess)
                {
                    entity.DataFlag = 1;
                    Dbers.GetInstance().SelfDber.Update(entity);

                    res++;
                }
            }
            output(string.Format("同步采样计划 {0} 条（集中管控 > 第三方）", res), eOutputType.Normal);

            res = 0;
            // 第三方 > 集中管控
            foreach (InfPDCYPlan entity in DcDbers.GetInstance().BeltSampler_NCGM_Dber.Entities<InfPDCYPlan>("where DataFlag=2 and datediff(dd,CreateDate,getdate())=0"))
            {
                CmcsBeltSamplePlan beltSamplePlan = Dbers.GetInstance().SelfDber.Get<CmcsBeltSamplePlan>(entity.Id);
                if (beltSamplePlan == null) continue;

                // 更新采样开始、结束时间、采样员等
                beltSamplePlan.StartTime = entity.StartTime;
                beltSamplePlan.EndTime = entity.EndTime;
                beltSamplePlan.SampleUser = entity.SampleUser;

                if (Dbers.GetInstance().SelfDber.Update(beltSamplePlan) > 0)
                {
                    // 我方已读
                    entity.DataFlag = 3;
                    DcDbers.GetInstance().BeltSampler_NCGM_Dber.Update(entity);

                    res++;
                }
            }
            output(string.Format("同步采样计划 {0} 条（第三方 > 集中管控）", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步控制命令
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncPDCYControlCMD(Action<string, eOutputType> output)
        {
            int res = 0;

            // 集中管控 > 第三方 
            foreach (CmcsBeltSampleCmd entity in BeltSamplerDAO.GetInstance().GetWaitForSyncBeltSampleCmd(GlobalVars.InterfaceType_NCGM_HCPDCYJ))
            {
                bool isSuccess = false;

                InfPDCYControlCMD pDCYControlCMD = DcDbers.GetInstance().BeltSampler_NCGM_Dber.Get<InfPDCYControlCMD>(entity.Id);
                if (pDCYControlCMD == null)
                {
                    isSuccess = DcDbers.GetInstance().BeltSampler_NCGM_Dber.Insert(new InfPDCYControlCMD
                    {
                        // 保持相同的Id
                        Id = entity.Id,
                        DataFlag = 0,
                        CreateDate = entity.CreateDate,
                        SampleCode = entity.SampleCode,
                        MachineCode = ConvertToInfMachineCode(entity.MachineCode),
                        CmdCode = (int)Enum.Parse(typeof(eEquInfSampleCmd), entity.CmdCode),
                        ResultCode = (int)eEquInfCmdResultCode.默认
                    }) > 0;
                }
                else isSuccess = true;

                if (isSuccess)
                {
                    entity.DataFlag = 1;
                    Dbers.GetInstance().SelfDber.Update(entity);

                    res++;
                }
            }
            output(string.Format("同步控制命令 {0} 条（集中管控 > 第三方）", res), eOutputType.Normal);

            res = 0;
            // 第三方 > 集中管控
            foreach (InfPDCYControlCMD entity in DcDbers.GetInstance().BeltSampler_NCGM_Dber.Entities<InfPDCYControlCMD>("where DataFlag=2 and datediff(dd,CreateDate,getdate())=0"))
            {
                CmcsBeltSampleCmd beltSampleCmd = Dbers.GetInstance().SelfDber.Get<CmcsBeltSampleCmd>(entity.Id);
                if (beltSampleCmd == null) continue;

                // 更新执行结果等
                beltSampleCmd.ResultCode = ((eEquInfSampleCmd)entity.ResultCode).ToString();

                if (Dbers.GetInstance().SelfDber.Update(beltSampleCmd) > 0)
                {
                    // 我方已读
                    entity.DataFlag = 3;
                    DcDbers.GetInstance().BeltSampler_NCGM_Dber.Update(entity);

                    res++;
                }
            }
            output(string.Format("同步控制命令 {0} 条（第三方 > 集中管控）", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步卸样命令
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncPDCYControlUnloadCMD(Action<string, eOutputType> output)
        {
            int res = 0;

            // 集中管控 > 第三方
            foreach (CmcsBeltSampleUnloadCmd entity in BeltSamplerDAO.GetInstance().GetWaitForSyncBeltSampleUnloadCmd(GlobalVars.InterfaceType_NCGM_HCPDCYJ))
            {
                bool isSuccess = false;

                InfPDCYUnloadCMD pDCYUnloadCMD = DcDbers.GetInstance().BeltSampler_NCGM_Dber.Get<InfPDCYUnloadCMD>(entity.Id);
                if (pDCYUnloadCMD == null)
                {
                    isSuccess = DcDbers.GetInstance().BeltSampler_NCGM_Dber.Insert(new InfPDCYUnloadCMD
                    {
                        // 保持相同的Id
                        Id = entity.Id,
                        DataFlag = 0,
                        CreateDate = entity.CreateDate,
                        SampleCode = entity.SampleCode,
                        MachineCode = ConvertToInfMachineCode(entity.MachineCode),
                        ResultCode = (int)eEquInfCmdResultCode.默认,
                        UnloadType = (int)Enum.Parse(typeof(eEquInfSamplerUnloadType), entity.UnloadType)
                    }) > 0;
                }
                else isSuccess = true;

                if (isSuccess)
                {
                    entity.DataFlag = 1;
                    Dbers.GetInstance().SelfDber.Update(entity);

                    res++;
                }
            }
            output(string.Format("同步卸样命令 {0} 条（集中管控 > 第三方）", res), eOutputType.Normal);

            res = 0;
            // 第三方 > 集中管控
            foreach (InfPDCYUnloadCMD entity in DcDbers.GetInstance().BeltSampler_NCGM_Dber.Entities<InfPDCYUnloadCMD>("where DataFlag=2 and datediff(dd,CreateDate,getdate())=0"))
            {
                CmcsBeltSampleUnloadCmd beltSampleCmd = Dbers.GetInstance().SelfDber.Get<CmcsBeltSampleUnloadCmd>(entity.Id);
                if (beltSampleCmd == null) continue;

                // 更新执行结果等
                beltSampleCmd.ResultCode = ((eEquInfCmdResultCode)entity.ResultCode).ToString();

                if (Dbers.GetInstance().SelfDber.Update(beltSampleCmd) > 0)
                {
                    // 我方已读
                    entity.DataFlag = 3;
                    DcDbers.GetInstance().BeltSampler_NCGM_Dber.Update(entity);

                    res++;
                }
            }
            output(string.Format("同步卸样命令 {0} 条（第三方 > 集中管控）", res), eOutputType.Normal);
        }
    }
}
