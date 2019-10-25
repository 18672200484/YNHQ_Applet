using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities;
using CMCS.Common.Enums;
using CMCS.DumblyConcealer.Enums;
using CMCS.DumblyConcealer.Tasks.AutoCupboard_NCGM;
using CMCS.DumblyConcealer.Tasks.AutoCupboard_NCGM.Enums;
using CMCS.DumblyConcealer.Tasks.AutoMaker_NCGM.Entities;
using CMCS.DumblyConcealer.Tasks.AutoMaker_NCGM.Enums;
using CMCS.DumblyConcealer.Tasks.BeltSampler_NCGM.Entities;
using CMCS.DumblyConcealer.Tasks.PneumaticTransfer_XMJS.Enums;

namespace CMCS.DumblyConcealer.Tasks.AutoMaker_NCGM_DAO
{
    /// <summary>
    /// 南昌光明全自动制样机接口业务
    /// </summary>
    public class AutoMaker_NCGM_DAO
    {
        private static AutoMaker_NCGM_DAO instance;

        public static AutoMaker_NCGM_DAO GetInstance()
        {
            if (instance == null)
            {
                instance = new AutoMaker_NCGM_DAO();
            }
            return instance;
        }

        private AutoMaker_NCGM_DAO()
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
            if (machineCode == EnumClass.eMachineCode.ZYJ01.ToString())
                return GlobalVars.MachineCode_NCGM_QZDZYJ_1;

            return string.Empty;
        }

        /// <summary>
        /// 集中管控设备编码 转换成 第三方接口设备编码
        /// </summary>
        /// <param name="machineCode">集中管控设备编码</param>
        /// <returns></returns>
        public string ConvertToInfMachineCode(string machineCode)
        {
            if (machineCode == GlobalVars.MachineCode_NCGM_QZDZYJ_1)
                return EnumClass.eMachineCode.ZYJ01.ToString();

            return string.Empty;
        }

        /// <summary>
        /// 转换成第三方接口-煤种
        /// </summary>
        /// <param name="fuelKindName">煤种</param>
        /// <returns></returns>
        public int ConvertToInfFuelKindName(string fuelKindName)
        {
            EnumClass.eFuelKindName enumResulr;
            if (Enum.TryParse(fuelKindName, out enumResulr))
                return (int)enumResulr;
            else
                return (int)EnumClass.eFuelKindName.其它;
        }

        /// <summary>
        /// 转换成第三方接口-水分
        /// </summary>
        /// <param name="mt">水分</param>
        /// <returns></returns>
        public int ConvertToInfMt(double mt)
        {
            // TODO

            return (int)EnumClass.eMt.干煤;
        }

        /// <summary>
        /// 第三方样品类型 转换成 集中管控样品类型
        /// </summary>
        /// <param name="ypType">第三方样品类型</param>
        /// <returns></returns>
        public string ConvertToCmcsYpType(string ypType)
        {
            if (ypType == "1")
                return "6mm全水样";
            if (ypType == "2")
                return "3mm备查样";
            if (ypType == "3")
                return "0.2mm分析样";
            if (ypType == "4")
                return "0.2mm备查样";
            if (ypType == "5")
                return "6mm总经理备查样1";
            if (ypType == "6")
                return "6mm总经理备查样2";
            if (ypType == "7")
                return "6mm总经理备查样3";
            else
                return "未知";
        }

        #endregion

        /// <summary>
        /// 获取上位机运行状态表 - 心跳值
        /// 每隔30s读取该值，如果数值不变化则表示设备上位机出现故障
        /// </summary>
        /// <returns></returns>
        public string GetDataFlag()
        {
            InfMakeDataFlag makeDataFlag = DcDbers.GetInstance().AutoMaker_NCGM_Dber.Entity<InfMakeDataFlag>();
            if (makeDataFlag != null) return makeDataFlag.DataFlag.ToString();

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
                CommonDAO.GetInstance().SetSignalDataValue(ConvertToCmcsMachineCode(EnumClass.eMachineCode.ZYJ01.ToString()), GlobalVars.EquSystemStatueName, eEquInfSamplerSystemStatus.发生故障.ToString());
            }
        }

        /// <summary>
        /// 同步制样计划
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncMakePlan(Action<string, eOutputType> output)
        {
            int res = 0;

            // 集中管控 > 第三方 
            foreach (CmcsMakerPlan entity in MakerDAO.GetInstance().GetWaitForSyncMakePlan(GlobalVars.InterfaceType_NCGM_QZDZYJ))
            {
                bool isSuccess = false;

                InfMakePlan makeplan = DcDbers.GetInstance().AutoMaker_NCGM_Dber.Get<InfMakePlan>(entity.Id);
                if (makeplan == null)
                {
                    isSuccess = DcDbers.GetInstance().AutoMaker_NCGM_Dber.Insert(new InfMakePlan
                    {
                        // 保持相同的Id
                        Id = entity.Id,
                        InFactoryBatchId = entity.InFactoryBatchId,
                        MakeCode = entity.MakeCode,
                        FuelKindName = ConvertToInfFuelKindName(entity.FuelKindName),
                        Mt = ConvertToInfMt(0),
                        CoalSize = 2,
                        MakeType = 1,
                        DataFlag = 0
                    }) > 0;
                }
                else
                {
                    makeplan.Id = entity.Id;
                    makeplan.MakeCode = entity.MakeCode;
                    makeplan.FuelKindName = ConvertToInfFuelKindName(entity.FuelKindName);
                    makeplan.Mt = ConvertToInfMt(0);
                    makeplan.CoalSize = 2;
                    makeplan.MakeType = 1;
                    makeplan.DataFlag = 0;
                    isSuccess = DcDbers.GetInstance().AutoMaker_NCGM_Dber.Update(makeplan) > 0;
                }

                if (isSuccess)
                {
                    entity.DataFlag = 1;
                    Dbers.GetInstance().SelfDber.Update(entity);

                    res++;
                }
            }
            output(string.Format("同步制样计划 {0} 条（集中管控 > 第三方）", res), eOutputType.Normal);
        }

        /// <summary>
        /// 控制命令表
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncMakeControlCMD(Action<string, eOutputType> output)
        {
            int res = 0;

            // 集中管控 > 第三方 
            foreach (CmcsMakerControlCmd entity in MakerDAO.GetInstance().GetWaitForSyncMakerControlCmd(GlobalVars.InterfaceType_NCGM_QZDZYJ))
            {
                bool isSuccess = false;

                eMakeCMDCode enumMakeCMDCode;
                Enum.TryParse(entity.CmdCode, out enumMakeCMDCode);

                InfMakeControlCMD makecontrolcmd = DcDbers.GetInstance().AutoMaker_NCGM_Dber.Get<InfMakeControlCMD>(entity.Id);
                if (makecontrolcmd == null)
                {

                    isSuccess = DcDbers.GetInstance().AutoMaker_NCGM_Dber.Insert(new InfMakeControlCMD
                    {
                        // 保持相同的Id
                        Id = entity.Id,
                        CMDCode = (int)enumMakeCMDCode,
                        MakeCode = entity.MakeCode,
                        DataFlag = 0
                    }) > 0;
                }
                else
                {
                    makecontrolcmd.Id = entity.Id;
                    makecontrolcmd.CMDCode = (int)enumMakeCMDCode;
                    makecontrolcmd.MakeCode = entity.MakeCode;
                    makecontrolcmd.DataFlag = 0;
                    isSuccess = DcDbers.GetInstance().AutoMaker_NCGM_Dber.Update(makecontrolcmd) > 0;
                }

                if (isSuccess)
                {
                    entity.DataFlag = 1;
                    Dbers.GetInstance().SelfDber.Update(entity);

                    res++;
                }
            }
            output(string.Format("同步控制命令 {0} 条（集中管控 > 第三方）", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步制样 出样明细信息到集中管控
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncMakeDetail(Action<string, eOutputType> output)
        {
            int res = 0;

            foreach (InfMakeDetail entity in DcDbers.GetInstance().AutoMaker_NCGM_Dber.Entities<InfMakeDetail>("where DataFlag=0 order by CreateDate"))
            {
                if (SyncToRCMakeDetail(entity))
                {
                    if (MakerDAO.GetInstance().SaveMakerRecord(new CmcsMakerRecord
                    {
                        InterfaceType = CommonDAO.GetInstance().GetMachineInterfaceTypeByCode(GlobalVars.MachineCode_NCGM_QZDZYJ_1),
                        MachineCode = GlobalVars.MachineCode_NCGM_QZDZYJ_1,
                        MakerPlanId = "",
                        MakeCode = entity.MakeCode,
                        BarrelCode = entity.BarrelCode,
                        YPType = ConvertToCmcsYpType(entity.YPType),
                        YPWeight = entity.YPWeight,
                        StartTime = entity.StartTime,
                        EndTime = entity.EndTime,
                        MakeUser = entity.MakeUser,
                        DataFlag = 0
                    }))
                    {
                        entity.DataFlag = 1;
                        DcDbers.GetInstance().AutoMaker_NCGM_Dber.Update(entity);
                        res++;

                        //eYPLX eyplx;
                        //Enum.TryParse(entity.YPType, out eyplx);
                        // 插入气动传输调度计划
                        AutoCupboard_NCGM_DAO.GetInstance().AddNewSendSampleId(entity.BarrelCode, entity.YPType, eOp.制样机1, eOp.自动存查样管理系统);
                    }
                }
            }

            output(string.Format("同步出样明细记录 {0} 条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步制样 故障信息到集中管控
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncMakeError(Action<string, eOutputType> output)
        {
            int res = 0;

            foreach (InfMakeError entity in DcDbers.GetInstance().AutoMaker_NCGM_Dber.Entities<InfMakeError>("where DataFlag=0"))
            {
                if (CommonDAO.GetInstance().SaveEquInfHitch(GlobalVars.MachineCode_NCGM_QZDZYJ_1, entity.ErrorTime, entity.ErrorDescribe))
                {
                    entity.DataFlag = 1;
                    DcDbers.GetInstance().AutoMaker_NCGM_Dber.Update(entity);

                    res++;
                }
            }

            output(string.Format("同步故障信息记录 {0} 条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步实时信号到集中管控
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public int SyncSignalDatal(Action<string, eOutputType> output)
        {
            int res = 0;

            foreach (InfMakeSignal entity in DcDbers.GetInstance().AutoMaker_NCGM_Dber.Entities<InfMakeSignal>(""))
            {
                // 当心跳检测为故障时，则不更新系统状态，保持 eSampleSystemStatus.发生故障
                if (entity.TagName == GlobalVars.EquSystemStatueName && IsHitch) continue;

                string tagValue = entity.TagValue;
                if (entity.TagName == GlobalVars.EquSystemStatueName)
                    tagValue = ((eEquInfAutoMakerSystemStatus)Convert.ToInt32(tagValue)).ToString();

                res += CommonDAO.GetInstance().SetSignalDataValue(ConvertToCmcsMachineCode(entity.MachineCode), entity.TagName, tagValue) ? 1 : 0;
            }
            output(string.Format("同步实时信号 {0} 条", res), eOutputType.Normal);

            return res;
        }

        /// <summary>
        /// 同步样品信息到集中管控入厂煤制样明细表
        /// </summary>
        /// <param name="makeDetail"></param>
        private bool SyncToRCMakeDetail(InfMakeDetail makeDetail)
        {
            CmcsRCMake rCMake = Dbers.GetInstance().SelfDber.Entity<CmcsRCMake>("where MakeCode=:MakeCode", new { MakeCode = makeDetail.MakeCode });
            if (rCMake != null)
            {
                CmcsRCMakeDetail rCMakeDetail = Dbers.GetInstance().SelfDber.Entity<CmcsRCMakeDetail>("where MakeId=:MakeId and SampleType=:SampleType", new { MakeId = rCMake.Id, SampleType = ConvertToCmcsYpType(makeDetail.YPType) });
                if (rCMakeDetail != null)
                {
                    rCMakeDetail.Weight = makeDetail.YPWeight;
                    rCMakeDetail.BarrelCode = makeDetail.BarrelCode;
                    return Dbers.GetInstance().SelfDber.Update(rCMakeDetail) > 0;
                }
            }

            return false;
        }
    }
}
