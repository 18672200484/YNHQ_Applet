using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.DumblyConcealer.Tasks.AutoCupboard_NCGM.Entities;
using CMCS.DumblyConcealer.Tasks.AutoCupboard_NCGM.Enums;
using CMCS.DumblyConcealer.Tasks.PneumaticTransfer_XMJS;
using CMCS.DumblyConcealer.Tasks.PneumaticTransfer_XMJS.Enums;
using CMCS.Common;
using CMCS.Common.Entities;
using CMCS.DumblyConcealer.Enums;
using System.Data;
using CMCS.DumblyConcealer.Tasks.PneumaticTransfer_XMJS.Entities;
using CMCS.Common.DAO;
using CMCS.Common.Enums;

namespace CMCS.DumblyConcealer.Tasks.AutoCupboard_NCGM
{
    public class AutoCupboard_NCGM_DAO
    {
        private static AutoCupboard_NCGM_DAO instance;

        public static AutoCupboard_NCGM_DAO GetInstance()
        {
            if (instance == null)
            {
                instance = new AutoCupboard_NCGM_DAO();
            }
            return instance;
        }

        private AutoCupboard_NCGM_DAO()
        { }

        private static DateTime LastConnectTime = DateTime.Now.AddMinutes(-1);
        private static int connectValue = -9999;
        private static String MachineCode = GlobalVars.MachineCode_NCGM_CYG;

        /// <summary>
        /// 检测是否丢失心跳
        /// </summary>
        public bool CheckHeat()
        {
            bool returnCheckHeat = true;
            DateTime dt = DateTime.Now;
            //检测是否丢失心跳
            if ((dt - LastConnectTime).TotalSeconds > 30)
            {
                List<InfCYGDataFlag> items = DcDbers.GetInstance().AutoCupboard_NCGM_Dber.Entities<InfCYGDataFlag>("");
                if (items.Count > 0 && connectValue != items[0].DataFlag)
                {
                    if (connectValue != -9999)
                    {
                        connectValue = items[0].DataFlag;
                        LastConnectTime = dt;
                    }
                    else
                    {
                        connectValue = items[0].DataFlag;
                    }
                }
                else
                {
                    returnCheckHeat = false;
                }
            }
            return returnCheckHeat;
        }

        /// <summary>
        /// 托盘到位
        /// </summary>
        /// <returns></returns>
        public bool SamIsReady()
        {
            bool returnSamIsReady = true;

            List<InfCYGSignal> infcygsignals = DcDbers.GetInstance().AutoCupboard_NCGM_Dber.Entities<InfCYGSignal>(" where TagName='托盘到位'");
            if (infcygsignals.Count > 0 && infcygsignals[0].TagValue == 1)
            {
                returnSamIsReady = true;
            }
            else
            {
                returnSamIsReady = false;
            }
            return returnSamIsReady;
        }

        /// <summary>
        /// 是否处于空闲状态
        /// </summary>
        public bool CheckFree()
        {
            bool returnCheckFree = true;

            List<InfCYGSignal> infcygsignals = DcDbers.GetInstance().AutoCupboard_NCGM_Dber.Entities<InfCYGSignal>(" where TagName='系统'");
            if (infcygsignals.Count > 0 && infcygsignals[0].TagValue == 3)
            {
                returnCheckFree = true;
            }
            else
            {
                returnCheckFree = false;
            }
            List<InfCYGBill> InfCYGBill = DcDbers.GetInstance().AutoCupboard_NCGM_Dber.Entities<InfCYGBill>(" WHERE CZPFSSJ>GETDATE() - 0.010416667 ORDER BY CZPFSSJ DESC");
            if (returnCheckFree && (InfCYGBill.Count == 0 || InfCYGBill[0].InfCYGBillRecords.Count > 0))
            {
                returnCheckFree = true;
            }
            else
            {
                returnCheckFree = false;
            }
            return returnCheckFree;
        }

        /// <summary>
        /// 存取弃样命令
        /// </summary>
        /// <param name="czplx"></param>
        /// <param name="yplx"></param>
        /// <returns></returns>
        private bool SentInfCYGBill(CmcsCYGControlCMDDetail cmcscygcontrolcmd, eCZPLX czplx)
        {
            InfCYGBill infcygbill = new InfCYGBill();
            infcygbill.Id = cmcscygcontrolcmd.Id;
            infcygbill.CZPLX = (int)czplx;
            infcygbill.CZPFSSJ = DateTime.Now;
            infcygbill.ZYBM = cmcscygcontrolcmd.Code;
            infcygbill.YPLX = (int)ConvertToInfeYPLX(cmcscygcontrolcmd.SamType);
            infcygbill.YPRFIDBM = cmcscygcontrolcmd.Code;
            infcygbill.CZMS = 1;
            infcygbill.DATAFLAG = 0;

            if (DcDbers.GetInstance().AutoCupboard_NCGM_Dber.Insert(infcygbill) > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取需要处理的样品
        /// </summary>
        /// <returns></returns>
        public CmcsCYGControlCMDDetail GetFirstUseCode(Action<string, eOutputType> output)
        {
            output(string.Format("获取需要处理的样品"), eOutputType.Normal);
            List<CmcsCYGControlCMD> cmcscygcontrolcmds = Dbers.GetInstance().SelfDber.Entities<CmcsCYGControlCMD>(" WHERE STATUS!='处理完成' AND CANWORKING=1 ORDER BY CREATEDATE");
            if (cmcscygcontrolcmds.Count == 0)
            {
                cmcscygcontrolcmds = Dbers.GetInstance().SelfDber.Entities<CmcsCYGControlCMD>(" WHERE STATUS IS NULL AND CANWORKING=1 ORDER BY CREATEDATE");
                if (cmcscygcontrolcmds.Count == 0)
                {
                    output(string.Format("无待处理样品"), eOutputType.Normal);
                    return new CmcsCYGControlCMDDetail() { };
                }
                else
                {
                    cmcscygcontrolcmds[0].Status = "正在处理";
                    Dbers.GetInstance().SelfDber.Update(cmcscygcontrolcmds[0]);
                }
            }

            List<CmcsCYGControlCMDDetail> cmcscygcontrolcmddetails = Dbers.GetInstance().SelfDber.Entities<CmcsCYGControlCMDDetail>(" WHERE CYGControlCMDId='" + cmcscygcontrolcmds[0].Id + "' AND  STATUS!='处理完成' ORDER BY CREATEDATE");
            if (cmcscygcontrolcmddetails.Count == 0)
            {
                cmcscygcontrolcmddetails = Dbers.GetInstance().SelfDber.Entities<CmcsCYGControlCMDDetail>(" WHERE CYGControlCMDId='" + cmcscygcontrolcmds[0].Id + "' AND STATUS IS NULL ORDER BY CREATEDATE");
                if (cmcscygcontrolcmddetails.Count == 0)
                {
                    cmcscygcontrolcmds[0].Status = "处理完成";
                    cmcscygcontrolcmds[0].UpdateTime = DateTime.Now;
                    cmcscygcontrolcmds[0].DataFlag = 1;
                    Dbers.GetInstance().SelfDber.Update(cmcscygcontrolcmds[0]);
                    return GetFirstUseCode(output);
                }
                else
                {
                    cmcscygcontrolcmddetails[0].Status = "正在处理";
                    Dbers.GetInstance().SelfDber.Update(cmcscygcontrolcmddetails[0]);
                }
            }
            output(string.Format("处理的样品(Id:{0},Code:{1},StartPlace:{2},Place:{3})", cmcscygcontrolcmddetails[0].Id, cmcscygcontrolcmddetails[0].Code, cmcscygcontrolcmddetails[0].TheCmcsCYGControlCMD.StartPlace, cmcscygcontrolcmddetails[0].TheCmcsCYGControlCMD.Place), eOutputType.Normal);
            return cmcscygcontrolcmddetails[0];
        }
        /// <summary>
        /// 样品类型转换
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        int ConvertToInfeYPLX(string Code)
        {

            DataTable datatable = Dbers.GetInstance().SelfDber.ExecuteDataTable(String.Format("select t.content from syssmtbcodecontent t where t.kindid in ( select t.id from syssmtbcodekind t where t.kind='样品类型') and code='{0}'", Code));
            if (datatable.Rows.Count != 0)
            {
                try
                {
                    return Convert.ToInt32(datatable.Rows[0][0].ToString());
                }
                catch (Exception)
                {
                }
            }
            return 0;
        }


        /// <summary>
        /// 连锁确定存样柜
        /// </summary>
        /// <param name="czplx"></param>
        /// <param name="yplx"></param>
        /// <returns></returns>
        public bool AutoCheckSentInfCYGBill(CmcsCYGControlCMDDetail cmcscygcontrolcmddetail, Action<string, eOutputType> output)
        {
            bool returnvalue = false;
            if (cmcscygcontrolcmddetail.Status == "正在处理")
            {
                output(string.Format("连锁存样柜"), eOutputType.Normal);
                if (cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.StartPlace != eOp.自动存查样管理系统.ToString() && cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place != eOp.自动存查样管理系统.ToString())
                {
                    //专业气动传输
                    returnvalue = true;
                    cmcscygcontrolcmddetail.Status = "存样柜处理中";
                }
                else if (cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.StartPlace == eOp.自动存查样管理系统.ToString() && cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place != eOp.自动存查样管理系统.ToString())
                {
                    //检测样柜是否有样
                    if (Dbers.GetInstance().SelfDber.Entities<CmcsCYGSam>(String.Format(" where Code='{0}' ", cmcscygcontrolcmddetail.Code)).Count == 0)
                    {
                        returnvalue = true;
                        cmcscygcontrolcmddetail.ResultCode = 1;
                        cmcscygcontrolcmddetail.Status = "处理完成";
                        cmcscygcontrolcmddetail.UpdateTime = DateTime.Now;
                        output(string.Format("不存在{0}的样品", cmcscygcontrolcmddetail.Code), eOutputType.Normal);
                    }
                    if (CheckHeat() && CheckFree() && PneumaticTransfer_XMJS_DAO.GetInstance().CheckFree() && cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place != eOp.弃样.ToString())
                    {
                        //取样
                        returnvalue = SentInfCYGBill(cmcscygcontrolcmddetail, eCZPLX.取样_气动口);
                        cmcscygcontrolcmddetail.Status = "存样柜处理中";
                    }
                    else if (CheckHeat() && CheckFree() && cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place == eOp.弃样.ToString())
                    {
                        //弃样
                        returnvalue = SentInfCYGBill(cmcscygcontrolcmddetail, eCZPLX.弃样);
                        cmcscygcontrolcmddetail.Status = "等待结果";
                    }
                }
                else if (cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.StartPlace != eOp.自动存查样管理系统.ToString() && cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place == eOp.自动存查样管理系统.ToString())
                {
                    //存样
                    if (CheckHeat() && CheckFree() && PneumaticTransfer_XMJS_DAO.GetInstance().CheckFree())
                    {
                        returnvalue = SentInfCYGBill(cmcscygcontrolcmddetail, eCZPLX.存样);
                        cmcscygcontrolcmddetail.Status = "存样柜处理中";
                    }
                }
                else
                {
                    returnvalue = true;
                    cmcscygcontrolcmddetail.ResultCode = 1;
                    cmcscygcontrolcmddetail.Status = "处理完成";
                    cmcscygcontrolcmddetail.UpdateTime = DateTime.Now;
                    output(string.Format("不存在{0}的样品路径：{1}到{2}", cmcscygcontrolcmddetail.Code, cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.StartPlace, cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place), eOutputType.Normal);
                }
                if (returnvalue)
                {
                    Dbers.GetInstance().SelfDber.Update(cmcscygcontrolcmddetail);
                }
                else
                {
                    if (!CheckHeat())
                    {
                        output(string.Format("存样柜心跳断开"), eOutputType.Normal);
                    }
                    if (!CheckFree())
                    {
                        output(string.Format("存样柜非空闲"), eOutputType.Normal);
                    }
                    if (!PneumaticTransfer_XMJS_DAO.GetInstance().CheckFree())
                    {
                        output(string.Format("气动传输非空闲"), eOutputType.Normal);
                    }
                }
            }
            return returnvalue;
        }


        /// <summary>
        /// 连锁确定气动传输
        /// </summary>
        /// <param name="cmcscygcontrolcmddetail"></param>
        /// <returns></returns>
        public bool AutoCheckFreeSendSample(CmcsCYGControlCMDDetail cmcscygcontrolcmddetail, Action<string, eOutputType> output)
        {
            bool returnvalue = false;
            if (cmcscygcontrolcmddetail.Status == "存样柜处理中")
            {
                output(string.Format("气动传输处理中"), eOutputType.Normal);
                if (cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.StartPlace != eOp.自动存查样管理系统.ToString() && cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place != eOp.自动存查样管理系统.ToString())
                {
                    //专业气动传输
                    returnvalue = PneumaticTransfer_XMJS_DAO.GetInstance().CheckFreeSendSample(PneumaticTransfer_XMJS_DAO.GetInstance().ConvertToInfeOp(cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.StartPlace), PneumaticTransfer_XMJS_DAO.GetInstance().ConvertToInfeOp(cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place));
                    cmcscygcontrolcmddetail.Status = "气动传输处理中";
                }
                else if (cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.StartPlace == eOp.自动存查样管理系统.ToString() && cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place != eOp.自动存查样管理系统.ToString())
                {
                    {
                        InfCYGBill infcygbill = DcDbers.GetInstance().AutoCupboard_NCGM_Dber.Get<InfCYGBill>(cmcscygcontrolcmddetail.Id);
                        if (infcygbill != null)
                        {
                            List<InfCYGBillRecord> infcygbillrecords = infcygbill.InfCYGBillRecords;
                            if (infcygbillrecords.Count > 0)
                            {
                                if (infcygbillrecords[0].CZPJG == 2)
                                {
                                    infcygbillrecords[0].DATAFLAG = 1;
                                    cmcscygcontrolcmddetail.Errors = "存样柜：失败";
                                    cmcscygcontrolcmddetail.Status = "处理完成";
                                    cmcscygcontrolcmddetail.UpdateTime = DateTime.Now;
                                    returnvalue = true;
                                    output(string.Format("存样柜：传输失败:(Id:{0},Code:{1},StartPlace:{2},Place:{3})", cmcscygcontrolcmddetail.Id, cmcscygcontrolcmddetail.Code, cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.StartPlace, cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place), eOutputType.Normal);
                                }
                            }
                        }

                        //取样
                        if (CheckHeat() && PneumaticTransfer_XMJS_DAO.GetInstance().CheckFree() && SamIsReady())
                        {
                            returnvalue = PneumaticTransfer_XMJS_DAO.GetInstance().CheckFreeSendSample(PneumaticTransfer_XMJS_DAO.GetInstance().ConvertToInfeOp(cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.StartPlace), PneumaticTransfer_XMJS_DAO.GetInstance().ConvertToInfeOp(cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place));
                            cmcscygcontrolcmddetail.Status = "气动传输处理中";
                        }
                    }
                }
                else if (cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.StartPlace != eOp.自动存查样管理系统.ToString() && cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place == eOp.自动存查样管理系统.ToString())
                {
                    //存样
                    if (CheckHeat() && PneumaticTransfer_XMJS_DAO.GetInstance().CheckFree() && SamIsReady())
                    {
                        returnvalue = PneumaticTransfer_XMJS_DAO.GetInstance().CheckFreeSendSample(PneumaticTransfer_XMJS_DAO.GetInstance().ConvertToInfeOp(cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.StartPlace), PneumaticTransfer_XMJS_DAO.GetInstance().ConvertToInfeOp(cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place));
                        cmcscygcontrolcmddetail.Status = "气动传输处理中";
                    }
                }
                if (returnvalue)
                {
                    Dbers.GetInstance().SelfDber.Update(cmcscygcontrolcmddetail);
                }
                else
                {
                    if (!CheckHeat())
                    {
                        output(string.Format("存样柜心跳断开"), eOutputType.Normal);
                    }
                    if (!SamIsReady())
                    {
                        output(string.Format("存样柜托盘未到位"), eOutputType.Normal);
                    }
                    if (!PneumaticTransfer_XMJS_DAO.GetInstance().CheckFree())
                    {
                        output(string.Format("气动传输非空闲"), eOutputType.Normal);
                    }
                }
            }
            return returnvalue;
        }

        /// <summary>
        /// 等待结果
        /// </summary>
        /// <param name="cmcscygcontrolcmddetail"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public bool CheckResult(CmcsCYGControlCMDDetail cmcscygcontrolcmddetail, Action<string, eOutputType> output)
        {
            bool returnvalue = false;
            if (cmcscygcontrolcmddetail.Status == "气动传输处理中")
            {
                output(string.Format("气动传输结果检测中"), eOutputType.Normal);
                List<InfQDBill> infqdbills = DcDbers.GetInstance().PneumaticTransfer_XMJS_Dber.Entities<InfQDBill>(" order by Send_Time desc");
                if (infqdbills.Count != 0)
                {
                    if (infqdbills[0].DataStatus == 2 || infqdbills[0].DataStatus == 1)
                    {
                        if (infqdbills[0].DataStatus == 2)
                        {
                            cmcscygcontrolcmddetail.Errors = "气动传输：传输失败";
                            cmcscygcontrolcmddetail.Status = "处理完成";
                            cmcscygcontrolcmddetail.UpdateTime = DateTime.Now;
                            returnvalue = true;
                            output(string.Format("气动传输：传输失败:(Id:{0},Code:{1},StartPlace:{2},Place:{3})", cmcscygcontrolcmddetail.Id, cmcscygcontrolcmddetail.Code, cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.StartPlace, cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place), eOutputType.Normal);
                        }
                        else
                        {
                            cmcscygcontrolcmddetail.Status = "等待结果";
                            returnvalue = true;

                        }
                    }
                }
            }
            if (cmcscygcontrolcmddetail.Status == "等待结果")
            {
                output(string.Format("存样柜结果检测中"), eOutputType.Normal);

                if (cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.StartPlace != eOp.自动存查样管理系统.ToString() && cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place != eOp.自动存查样管理系统.ToString())
                {

                    cmcscygcontrolcmddetail.ResultCode = 1;
                    cmcscygcontrolcmddetail.Status = "处理完成";
                    cmcscygcontrolcmddetail.UpdateTime = DateTime.Now;
                    returnvalue = true;
                    output(string.Format("存样柜传输成功:(Id:{0},Code:{1},StartPlace:{2},Place:{3})", cmcscygcontrolcmddetail.Id, cmcscygcontrolcmddetail.Code, cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.StartPlace, cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place), eOutputType.Normal);
                }
                else
                {
                    InfCYGBill infcygbill = DcDbers.GetInstance().AutoCupboard_NCGM_Dber.Get<InfCYGBill>(cmcscygcontrolcmddetail.Id);
                    if (infcygbill != null)
                    {
                        List<InfCYGBillRecord> infcygbillrecords = infcygbill.InfCYGBillRecords;
                        if (infcygbillrecords.Count > 0)
                        {
                            if (infcygbillrecords[0].CZPJG == 1)
                            {
                                infcygbillrecords[0].DATAFLAG = 1;
                                cmcscygcontrolcmddetail.Status = "处理完成";
                                cmcscygcontrolcmddetail.UpdateTime = DateTime.Now;
                                cmcscygcontrolcmddetail.ResultCode = 1;
                                returnvalue = true;
                                output(string.Format("存样柜传输成功:(Id:{0},Code:{1},StartPlace:{2},Place:{3})", cmcscygcontrolcmddetail.Id, cmcscygcontrolcmddetail.Code, cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.StartPlace, cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place), eOutputType.Normal);
                            }
                            if (infcygbillrecords[0].CZPJG == 2)
                            {
                                infcygbillrecords[0].DATAFLAG = 1;
                                cmcscygcontrolcmddetail.Errors = "存样柜：失败";
                                cmcscygcontrolcmddetail.Status = "处理完成";
                                cmcscygcontrolcmddetail.UpdateTime = DateTime.Now;
                                returnvalue = true;
                                output(string.Format("存样柜：传输失败:(Id:{0},Code:{1},StartPlace:{2},Place:{3})", cmcscygcontrolcmddetail.Id, cmcscygcontrolcmddetail.Code, cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.StartPlace, cmcscygcontrolcmddetail.TheCmcsCYGControlCMD.Place), eOutputType.Normal);
                            }
                        }
                    }
                }
            }
            if (returnvalue)
            {
                Dbers.GetInstance().SelfDber.Update(cmcscygcontrolcmddetail);
            }
            return returnvalue;
        }
        /// <summary>
        /// 存样返回Id用Id查询存样是否成功
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="eyplx">样品类型</param>
        /// <param name="eopstart">开始位置</param>
        /// <param name="eopeopEnd"></param>
        /// <returns></returns>
        public String AddNewSendSampleId(String Code, String eyplx, eOp eopstart, eOp eopeopEnd)
        {
            CmcsCYGControlCMD cmcscygcontrolcmd = new CmcsCYGControlCMD();
            cmcscygcontrolcmd.PlanDate = DateTime.Now;
            int newbill = 1;
            try
            {
                newbill = Convert.ToInt32(Dbers.GetInstance().SelfDber.Entities<CmcsCYGControlCMD>(" where Bill like '" + "CYG" + DateTime.Now.ToString("yyMMdd") + "%'").Max(a => a.Bill).Substring(9, 3)) + 1;
            }
            catch (Exception)
            {
            }
            cmcscygcontrolcmd.Bill = "CYG" + DateTime.Now.ToString("yyMMdd") + newbill.ToString().PadLeft(3, '0');
            cmcscygcontrolcmd.CodeNumber = 1;
            cmcscygcontrolcmd.DataFlag = 0;
            cmcscygcontrolcmd.OperType = eyplx;
            cmcscygcontrolcmd.StartPlace = eopstart.ToString();
            cmcscygcontrolcmd.Place = eopeopEnd.ToString();
            cmcscygcontrolcmd.OperPerson = "admin";
            cmcscygcontrolcmd.CanWorking = "1";
            Dbers.GetInstance().SelfDber.Insert(cmcscygcontrolcmd);
            CmcsCYGControlCMDDetail cmcscygcontrolcmddetail = new CmcsCYGControlCMDDetail();
            cmcscygcontrolcmddetail.CYGControlCMDId = cmcscygcontrolcmd.Id;
            cmcscygcontrolcmddetail.Code = Code;
            Dbers.GetInstance().SelfDber.Insert(cmcscygcontrolcmddetail);

            return cmcscygcontrolcmddetail.Id;
        }
        /// <summary>
        /// Id查询存样是否成功
        /// </summary>
        /// <param name="SendSampleId"></param>
        /// <returns></returns>
        public String GetResult(String SendSampleId)
        {
            CmcsCYGControlCMDDetail cmcscygcontrolcmddetail = Dbers.GetInstance().SelfDber.Get<CmcsCYGControlCMDDetail>(SendSampleId);
            if (!String.IsNullOrEmpty(cmcscygcontrolcmddetail.Errors))
            {
                return cmcscygcontrolcmddetail.Errors;
            }
            if (cmcscygcontrolcmddetail.Status == "处理完成")
            {
                return "成功";
            }
            return "调度中";
        }


        /// <summary>
        /// 转换成第三方接口-操作票类型
        /// </summary>
        /// <param name="sampleType">操作票类型</param>
        /// <returns></returns>
        public eCZPLX ConvertToInfeCZPLX(string mType)
        {
            eCZPLX enumResulr;
            if (Enum.TryParse(mType, out enumResulr))
                return enumResulr;
            else
                return eCZPLX.取样_气动口;
        }
        ///// <summary>
        ///// 转换成第三方接口-操作票类型
        ///// </summary>
        ///// <param name="sampleType">操作票类型</param>
        ///// <returns></returns>
        //public eYPLX ConvertToInfeYPLX(string mType)
        //{
        //    eYPLX enumResulr;
        //    if (Enum.TryParse(mType, out enumResulr))
        //        return enumResulr;
        //    else
        //        return eYPLX.分析样02mm;
        //}

        /// <summary>
        /// 同步存样柜数据
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public bool SyncCYGInfo(Action<string, eOutputType> output)
        {
            int resd = 0;//删除
            int resu = 0;//修改
            int resi = 0;//新增
            bool returnresult = false;
            List<CmcsCYGSam> cmcscygsam = Dbers.GetInstance().SelfDber.Entities<CmcsCYGSam>("");
            List<InfCYGSam> infcygsam = DcDbers.GetInstance().AutoCupboard_NCGM_Dber.Entities<InfCYGSam>("");
            foreach (CmcsCYGSam item in cmcscygsam)
            {
                InfCYGSam infcygsamitem = infcygsam.Where(a => a.样瓶编码 == item.Code).FirstOrDefault();
                if (infcygsamitem == null)
                {
                    resd += Dbers.GetInstance().SelfDber.Delete<CmcsCYGSam>(item.Id);
                    returnresult = true;
                }
                else
                {
                    item.UpdateTime = infcygsamitem.操作时间;
                    item.MachineCode = infcygsamitem.柜号;
                    item.SamType = infcygsamitem.瓶子类型;
                    item.IsNew = infcygsamitem.柜子状态;
                    item.Code = infcygsamitem.样瓶编码;
                    if (infcygsamitem.操作人员代码 != null)
                        item.OperUser = infcygsamitem.操作人员代码;

                    resu += Dbers.GetInstance().SelfDber.Update(item);
                }
            }

            foreach (var item in infcygsam)
            {
                CmcsCYGSam infcygsamitem = cmcscygsam.Where(a => a.Code == item.样瓶编码).FirstOrDefault();
                if (infcygsamitem == null)
                {
                    CmcsCYGSam iteme = new CmcsCYGSam();
                    iteme.UpdateTime = item.操作时间;
                    iteme.MachineCode = item.柜号;
                    iteme.SamType = item.瓶子类型;
                    iteme.IsNew = item.柜子状态;
                    iteme.Code = item.样瓶编码;
                    resi += Dbers.GetInstance().SelfDber.Insert(iteme);
                    returnresult = true;
                }
            }
            if (Dbers.GetInstance().SelfDber.Entities<CmcsCYGSam>(" where IsNew!=1").Count <= 50)
            {

                if (!CommonDAO.GetInstance().hasSysMessage(eMessageType.气动传输.ToString(), "存样柜空闲个数小于50个!"))
                {
                    CommonDAO.GetInstance().SaveSysMessage(eMessageType.气动传输.ToString(), "存样柜空闲个数小于50个!", eMessageType.气动传输.ToString());
                }
            }

            if (Dbers.GetInstance().SelfDber.Entities<CmcsCYGSam>(" where IsNew=1 and UpdateTime+90>sysdate").Count <= 50)
            {

                if (!CommonDAO.GetInstance().hasSysMessage(eMessageType.气动传输.ToString(), "存样柜样品已超期!"))
                {
                    CommonDAO.GetInstance().SaveSysMessage(eMessageType.气动传输.ToString(), "存样柜样品已超期!", eMessageType.气动传输.ToString());
                }
            }

            output(string.Format("同步数据:(新增数:{0},修改数:{1},删除数:{2})", resi, resu, resd), eOutputType.Normal);
            return returnresult;
        }



        /// <summary>
        /// 同步实时信号到集中管控
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public int SyncSignalDatal(Action<string, eOutputType> output)
        {
            int res = 0;
            foreach (InfCYGSignal entity in DcDbers.GetInstance().AutoCupboard_NCGM_Dber.Entities<InfCYGSignal>(""))
            {
                // 当心跳检测为故障时，则不更新系统状态，保持 eSampleSystemStatus.发生故障
                if (entity.TagName == GlobalVars.EquSystemStatueName) continue;
                res += CommonDAO.GetInstance().SetSignalDataValue(MachineCode, entity.TagName, entity.TagValue.ToString()) ? 1 : 0;
            }
            output(string.Format("存样柜-同步实时信号 {0} 条", res), eOutputType.Normal);

            return res;
        }
        /// <summary>
        /// 同步制样 故障信息到集中管控
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public void SyncCYGError(Action<string, eOutputType> output)
        {
            int res = 0;

            foreach (InfCYGError entity in DcDbers.GetInstance().AutoCupboard_NCGM_Dber.Entities<InfCYGError>("where DataFlag=0"))
            {
                if (CommonDAO.GetInstance().SaveEquInfHitch(MachineCode, entity.ErrorTime, entity.ErrorDescribe))
                {
                    entity.DataFlag = 1;
                    DcDbers.GetInstance().AutoCupboard_NCGM_Dber.Insert(entity);

                    res++;
                }
            }

            output(string.Format("存样柜-同步故障信息记录 {0} 条", res), eOutputType.Normal);
        }
    }
}
