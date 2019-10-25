using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
//
using CMCS.Common.Entities;
using CMCS.Common.Entities.AutoMaker;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.Fuel;
using CMCS.Common.Entities.iEAA;
using CMCS.Common.Entities.Inf;
using CMCS.Common.Entities.Sys;
using CMCS.Common.Enums;
using CMCS.Common.Utilities;
using CMCS.DapperDber.Dbs.OracleDb;
using CMCS.DapperDber.Util;
using CMCS.DapperDber.Dbs.SqlServerDb;

namespace CMCS.Common.DAO
{
    /// <summary>
    /// 公共业务
    /// </summary>
    public class CommonDAO
    {
        private static CommonDAO instance;

        public static CommonDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new CommonDAO();
            }

            return instance;
        }

        private CommonDAO()
        { }

        public OracleDapperDber SelfDber
        {
            get { return Dbers.GetInstance().SelfDber; }
        }



        #region 编码管理

        /// <summary>
        /// 根据名称获取编码
        /// </summary>
        /// <param name="kindName">编码类别</param>
        /// <returns></returns>
        public List<CodeContent> GetCodeContentByKind(string kindName)
        {
            List<CodeContent> res = new List<CodeContent>();

            CodeKind codeKind = SelfDber.Entity<CodeKind>("where Kind=:Kind", new { Kind = kindName });
            if (codeKind != null) res = SelfDber.Entities<CodeContent>("where KindId=:KindId order by CodeOrder asc", new { KindId = codeKind.Id });

            return res;
        }

        /// <summary>
        /// 判断用户是否有权限
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="resourceCode">模块功能编码</param>
        /// <returns></returns>
        public bool HasResourcePowerByResCode(string userAccount, string resourceCode)
        {
            if (string.IsNullOrEmpty(userAccount) || string.IsNullOrEmpty(resourceCode)) return false;
            User user = SelfDber.Query<User>("select t.* from sysamtbuser t inner join sysamtbparty_role a on t.partyid=a.partyid inner join sysamtbpartyrole b on b.id=a.roleid where (b.RoleCode=:RoleCode or b.RoleCode='0000') and t.UserAccount=:UserAccount", new { RoleCode = resourceCode, UserAccount = userAccount }).FirstOrDefault();
            if (user != null) return true;
            return false;
        }

        #endregion

        #region 用户登录

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <param name="userAccount"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Login(string roleCode, string userAccount, string password)
        {
            return SelfDber.Query<User>("select t.* from sysamtbuser t inner join sysamtbparty_role a on t.partyid=a.partyid inner join sysamtbpartyrole b on b.id=a.roleid where (b.RoleCode=:RoleCode or b.RoleCode='0000') and t.UserAccount=:UserAccount and t.MDPassword=:MDPassword ", new { RoleCode = roleCode, UserAccount = userAccount, MDPassword = password }).FirstOrDefault();
        }

        /// <summary>
        /// 获取某角色下所有的用户
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <returns></returns>
        public List<User> GetAllSystemUser(string roleCode)
        {
            return SelfDber.Query<User>("select t.* from sysamtbuser t inner join sysamtbparty_role a on t.partyid=a.partyid inner join sysamtbpartyrole b on b.id=a.roleid where b.RoleCode=:RoleCode or b.RoleCode='0000'", new { RoleCode = roleCode }).ToList();
        }

        /// <summary>
        /// 获取管理员
        /// </summary>
        /// <returns></returns>
        public User GetAdminUser()
        {
            return SelfDber.Entity<User>("where UserAccount=:UserAccount", new { UserAccount = GlobalVars.AdminAccount });
        }

        #endregion

        #region 程序配置

        /// <summary>
        /// 获取程序配置
        /// </summary>
        /// <param name="appIdentifier">程序唯一标识</param>
        /// <param name="configName">配置名称</param>
        /// <returns></returns>
        public string GetAppletConfigString(string appIdentifier, string configName)
        {
            CmcsAppletConfig appletConfig = SelfDber.Entity<CmcsAppletConfig>("where AppIdentifier=:AppIdentifier and ConfigName=:ConfigName", new { AppIdentifier = appIdentifier, ConfigName = configName });
            if (appletConfig != null) return appletConfig.ConfigValue;

            return string.Empty;
        }

        /// <summary>
        /// 设置程序配置
        /// </summary>
        /// <param name="appIdentifier">程序唯一标识</param>
        /// <param name="configName">配置名称</param>
        /// <param name="configValue">值</param>
        /// <returns></returns>
        public bool SetAppletConfig(string appIdentifier, string configName, string configValue)
        {
            CmcsAppletConfig appletConfig = SelfDber.Entity<CmcsAppletConfig>("where AppIdentifier=:AppIdentifier and ConfigName=:ConfigName", new { AppIdentifier = appIdentifier, ConfigName = configName });
            if (appletConfig != null)
            {
                appletConfig.ConfigValue = configValue;
                return SelfDber.Update(appletConfig) > 0;
            }
            else
            {
                return SelfDber.Insert(new CmcsAppletConfig()
                {
                    AppIdentifier = appIdentifier,
                    ConfigName = configName,
                    ConfigValue = configValue
                }) > 0;
            }
        }

        /// <summary>
        /// 获取程序配置
        /// </summary> 
        /// <param name="configName">配置名称</param>
        /// <returns></returns>
        public string GetAppletConfigString(string configName)
        {
            return GetAppletConfigString(CommonAppConfig.GetInstance().AppIdentifier, configName);
        }

        /// <summary>
        /// 获取程序配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns></returns>
        public bool GetAppletConfigBoolen(string configName)
        {
            try
            {
                return Convert.ToBoolean(GetAppletConfigString(configName));
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取程序配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns></returns>
        public int GetAppletConfigInt32(string configName)
        {
            try
            {
                return Convert.ToInt32(GetAppletConfigString(configName));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取程序配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns></returns>
        public DateTime GetAppletConfigDateTime(string appIdentifier, string configName)
        {
            return Convert.ToDateTime(GetAppletConfigString(configName));
        }

        /// <summary>
        /// 获取程序配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns></returns>
        public double GetAppletConfigDouble(string configName)
        {
            try
            {
                return Convert.ToDouble(GetAppletConfigString(configName));
            }
            catch (Exception)
            {
                return 0.00;
            }
        }

        /// <summary>
        /// 设置程序配置
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="configValue"></param>
        /// <returns></returns>
        public bool SetAppletConfig(string configName, string configValue)
        {
            return SetAppletConfig(CommonAppConfig.GetInstance().AppIdentifier, configName, configValue);
        }

        /// <summary>
        /// 获取公共程序配置
        /// </summary> 
        /// <param name="configName">配置名称</param>
        /// <returns></returns>
        public string GetCommonAppletConfigString(string configName)
        {
            return GetAppletConfigString(GlobalVars.CommonAppletConfigName, configName);
        }

        /// <summary>
        /// 获取公共程序配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns></returns>
        public bool GetCommonAppletConfigBoolen(string configName)
        {
            return Convert.ToBoolean(GetCommonAppletConfigString(configName));
        }

        /// <summary>
        /// 获取公共程序配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns></returns>
        public int GetCommonAppletConfigInt32(string configName)
        {
            return Convert.ToInt32(GetCommonAppletConfigString(configName));
        }

        /// <summary>
        /// 获取公共程序配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns></returns>
        public DateTime GetCommonAppletConfigDateTime(string appIdentifier, string configName)
        {
            return Convert.ToDateTime(GetCommonAppletConfigString(configName));
        }

        /// <summary>
        /// 获取公共程序配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns></returns>
        public double GetCommonAppletConfigDouble(string configName)
        {
            return Convert.ToDouble(GetCommonAppletConfigString(configName));
        }

        /// <summary>
        /// 设置公共程序配置
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="configValue"></param>
        /// <returns></returns>
        public bool SetCommonAppletConfig(string configName, string configValue)
        {
            return SetAppletConfig(GlobalVars.CommonAppletConfigName, configName, configValue);
        }

        #endregion

        #region 运行日志

        /// <summary>
        /// 保存程序运行日志
        /// </summary>
        /// <param name="appletLogLevel">日志等级</param>
        /// <param name="title">日志标题</param>
        /// <param name="content">日志内容</param>
        /// <returns></returns>
        public bool SaveAppletLog(eAppletLogLevel appletLogLevel, string title, string content, string userName)
        {
            return SelfDber.Insert(new CmcsAppletLog()
            {
                AppIdentifier = CommonAppConfig.GetInstance().AppIdentifier,
                Title = title,
                LogLevel = appletLogLevel.ToString(),
                Content = content,
                CreateUser = userName,
                OperUser = userName
            }) > 0;
        }

        #endregion

        #region 实时信号

        /// <summary>
        /// 获取实时信号
        /// </summary> 
        /// <param name="signalPrefix">信号前缀</param>
        /// <param name="signalName">信号名</param>
        /// <returns></returns>
        public string GetSignalDataValue(string signalPrefix, string signalName)
        {
            CmcsSignalData cmcsSignalData = SelfDber.Entity<CmcsSignalData>("where SignalPrefix=:SignalPrefix and SignalName=:SignalName order by UpdateTime desc", new { SignalPrefix = signalPrefix, SignalName = signalName });
            if (cmcsSignalData != null) return cmcsSignalData.SignalValue;

            return string.Empty;
        }

        /// <summary>
        /// 获取实时信号
        /// </summary> 
        /// <param name="signalPrefix">信号前缀</param>
        /// <param name="signalName">信号名</param>
        /// <returns></returns>
        public double GetSignalDataValueDouble(string signalPrefix, string signalName)
        {
            double res = 0;
            Double.TryParse(GetSignalDataValue(signalPrefix, signalName), out res);

            return res;
        }

        /// <summary>
        /// 获取实时信号
        /// </summary> 
        /// <param name="signalPrefix">信号前缀</param>
        /// <param name="signalName">信号名</param>
        /// <returns></returns>
        public int GetSignalDataValueInt32(string signalPrefix, string signalName)
        {
            int res = 0;
            Int32.TryParse(GetSignalDataValue(signalPrefix, signalName), out res);

            return res;
        }

        /// <summary>
        /// 获取实时信号
        /// </summary> 
        /// <param name="signalPrefix">信号前缀</param>
        /// <param name="signalName">信号名</param>
        /// <returns></returns>
        public bool GetSignalDataValueBoolean(string signalPrefix, string signalName)
        {
            Boolean res = false;
            Boolean.TryParse(GetSignalDataValue(signalPrefix, signalName), out res);

            return res;
        }

        /// <summary>
        /// 获取实时信号
        /// </summary> 
        /// <param name="signalPrefix">信号前缀</param>
        /// <param name="signalName">信号名</param>
        /// <returns></returns>
        public DateTime GetSignalDataValueDateTime(string signalPrefix, string signalName)
        {
            DateTime res = DateTime.MinValue;
            DateTime.TryParse(GetSignalDataValue(signalPrefix, signalName), out res);

            return res;
        }

        /// <summary>
        /// 设置实时信号
        /// </summary> 
        /// <param name="signalPrefix">信号前缀</param>
        /// <param name="signalName">信号名</param>
        /// <param name="signalValue">值</param>
        /// <returns></returns>
        public bool SetSignalDataValue(string signalPrefix, string signalName, string signalValue)
        {
            CmcsSignalData cmcsSignalData = SelfDber.Entity<CmcsSignalData>("where SignalPrefix=:SignalPrefix and SignalName=:SignalName order by UpdateTime desc", new { SignalPrefix = signalPrefix, SignalName = signalName });
            if (cmcsSignalData == null)
            {
                SelfDber.Insert(new CmcsSignalData
                {
                    SignalPrefix = signalPrefix,
                    SignalName = signalName,
                    SignalValue = signalValue,
                    UpdateTime = DateTime.Now
                });
            }

            return SelfDber.Execute("update " + EntityReflectionUtil.GetTableName<CmcsSignalData>() + " set SignalValue=:SignalValue,UpdateTime=sysdate where SignalPrefix=:SignalPrefix and  SignalName=:SignalName", new { SignalPrefix = signalPrefix, SignalName = signalName, SignalValue = signalValue }) > 0;
        }

        #endregion

        #region 设备管理

        /// <summary>
        /// 根据设备编码获取设备
        /// </summary>
        /// <param name="machineCode">设备编码</param>
        /// <returns></returns>
        public CmcsCMEquipment GetCMEquipmentByMachineCode(string machineCode)
        {
            return SelfDber.Entity<CmcsCMEquipment>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = machineCode });
        }

        /// <summary>
        /// 根据设备编码获取设备名称
        /// </summary>
        /// <param name="machineCode">设备编码</param>
        /// <returns></returns>
        public string GetMachineNameByCode(string machineCode)
        {
            CmcsCMEquipment entity = GetCMEquipmentByMachineCode(machineCode);
            return entity != null ? entity.EquipmentName : string.Empty;
        }

        /// <summary>
        /// 根据设备编码获取接口类型
        /// </summary>
        /// <param name="machineCode">设备编码</param>
        /// <returns></returns>
        public string GetMachineInterfaceTypeByCode(string machineCode)
        {
            CmcsCMEquipment entity = GetCMEquipmentByMachineCode(machineCode);
            return entity != null ? entity.InterfaceType : string.Empty;
        }

        /// <summary>
        /// 根据父节点设备编码获取所有子设备集合
        /// </summary>
        /// <param name="machineCode">设备编码</param>
        /// <returns></returns>
        public List<CmcsCMEquipment> GetChildrenMachinesByCode(string machineCode)
        {
            List<CmcsCMEquipment> list = new List<CmcsCMEquipment>();

            CmcsCMEquipment entity = GetCMEquipmentByMachineCode(machineCode);
            if (entity != null) list = SelfDber.Entities<CmcsCMEquipment>(" where ParentId=:ParentId  order by Sequence asc", new { ParentId = entity.Id });

            return list;
        }

        #endregion

        #region 第三方设备接口

        /// <summary>
        /// 获取未读第三方设备故障信息
        /// </summary>
        /// <param name="machineCode">设备编码</param>
        /// <returns></returns>
        public List<InfEquInfHitch> GetUnReadEquInfHitchs(string machineCode)
        {
            List<InfEquInfHitch> res = SelfDber.Entities<InfEquInfHitch>("where MachineCode=:machineCode and DataFlag=0", new { MachineCode = machineCode });
            SelfDber.Execute("update " + EntityReflectionUtil.GetTableName<InfEquInfHitch>() + " set DataFlag=1 where MachineCode=:machineCode and DataFlag=0", new { MachineCode = machineCode });
            return res;
        }

        /// <summary>
        /// 获取当日第三方设备故障信息
        /// </summary>
        /// <param name="machineCode">设备编码</param>
        /// <param name="dtTime">时间</param>
        /// <returns></returns>
        public List<InfEquInfHitch> GetEquInfHitchsByTime(string machineCode, DateTime dtTime)
        {
            List<InfEquInfHitch> res = SelfDber.Entities<InfEquInfHitch>("where MachineCode=:machineCode and HitchTime like '%" + dtTime.ToString("yyyy-MM-dd") + "%' order by HitchTime desc", new { MachineCode = machineCode });
            return res;
        }

        /// <summary>
        /// 保存第三方设备接口 - 实时集样罐表
        /// </summary>
        /// <param name="equInfSampleBarrel"></param>
        /// <returns></returns>
        public bool SaveEquInfSampleBarrel(InfEquInfSampleBarrel equInfSampleBarrel)
        {
            InfEquInfSampleBarrel oldEquInfSampleBarrel = SelfDber.Entity<InfEquInfSampleBarrel>("where InterfaceType=:InterfaceType and MachineCode=:MachineCode and BarrelNumber=:BarrelNumber and BarrelType=:BarrelType"
                , new { InterfaceType = equInfSampleBarrel.InterfaceType, MachineCode = equInfSampleBarrel.MachineCode, BarrelNumber = equInfSampleBarrel.BarrelNumber, BarrelType = equInfSampleBarrel.BarrelType });

            if (oldEquInfSampleBarrel == null)
                return SelfDber.Insert(equInfSampleBarrel) > 0;
            else
            {
                oldEquInfSampleBarrel.BarrelNumber = equInfSampleBarrel.BarrelNumber;
                oldEquInfSampleBarrel.BarrelStatus = equInfSampleBarrel.BarrelStatus;
                oldEquInfSampleBarrel.InFactoryBatchId = equInfSampleBarrel.InFactoryBatchId;
                oldEquInfSampleBarrel.IsCurrent = equInfSampleBarrel.IsCurrent;
                oldEquInfSampleBarrel.SampleCode = equInfSampleBarrel.SampleCode;
                oldEquInfSampleBarrel.SampleCount = equInfSampleBarrel.SampleCount;
                oldEquInfSampleBarrel.UpdateTime = equInfSampleBarrel.UpdateTime;
                oldEquInfSampleBarrel.DataFlag = equInfSampleBarrel.DataFlag;

                return SelfDber.Update(oldEquInfSampleBarrel) > 0;
            }
        }

        /// <summary>
        /// 保持第三方设备接口 - 故障信息表
        /// </summary> 
        /// <param name="machineCode">设备编码</param>
        /// <param name="hitchTime">故障时间</param>
        /// <param name="hitchDescribe">故障描述</param>
        /// <returns></returns>
        public bool SaveEquInfHitch(string machineCode, DateTime hitchTime, string hitchDescribe)
        {
            return SelfDber.Insert(new InfEquInfHitch
            {
                DataFlag = 0,
                HitchDescribe = hitchDescribe,
                HitchTime = hitchTime,
                InterfaceType = GetMachineInterfaceTypeByCode(machineCode),
                MachineCode = machineCode
            }) > 0;
        }

        #endregion

        #region 系统消息弹框

        /// <summary>
        /// 获取当日未读取的异常信息，按先进先出、同一设备异常分组的原则,
        /// </summary>
        /// <returns></returns>
        public List<InfEquInfHitch> GetWarnEquInfHitch()
        {
            List<InfEquInfHitch> cmcsequinfhitch = SelfDber.Entities<InfEquInfHitch>(" where IsRead=0 and HitchTime like '%" + DateTime.Now.ToString("yyyy-MM-dd") + "%' order by HitchTime ");
            if (cmcsequinfhitch.Count > 0)
                return cmcsequinfhitch.GroupBy(a => a.MachineCode).First().ToList();
            else
                return new List<InfEquInfHitch>();
        }

        /// <summary>
        /// 根据异常时间查询异常信息
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public List<InfEquInfHitch> GetCmcsEquInfHitchs(DateTime dtStart, DateTime dtEnd)
        {
            List<InfEquInfHitch> equinfhitchs = SelfDber.Entities<InfEquInfHitch>(" where HitchTime>='" + dtStart + "' and HitchTime<='" + dtEnd + "' order by HitchTime ");
            return equinfhitchs;
        }

        /// <summary>
        /// 将异常信息值为已读
        /// </summary>
        /// <param name="EquInfHitchId"></param>
        public void UpdateReadEquInfHitch(string EquInfHitchId)
        {
            SelfDber.Execute(" update " + EntityReflectionUtil.GetTableName<InfEquInfHitch>() + " t set t.isread=1 where t.id='" + EquInfHitchId + "' ");
        }

        /// <summary>
        /// 获取当日未读取的管控系统消息
        /// </summary>
        /// <returns></returns>
        public CmcsSysMessage GetTodayTopSysMessage()
        {
            return SelfDber.Entity<CmcsSysMessage>("where MsgStatus=:MsgStatus and MsgTime like '%' || :MsgTime || '%' order by MsgTime", new { MsgStatus = eSysMessageStatus.默认.ToString(), MsgTime = DateTime.Now.ToString("yyyy-MM-dd") });
        }

        /// <summary>
        /// 更改系统消息的状态
        /// </summary>
        /// <param name="sysMessageId"></param>
        /// <param name="sysMessageStatus"></param>
        public void ChangeSysMessageStatus(string sysMessageId, eSysMessageStatus sysMessageStatus)
        {
            SelfDber.Execute("update " + EntityReflectionUtil.GetTableName<CmcsSysMessage>() + " t set t.MsgStatus=:MsgStatus where t.Id=:Id", new { Id = sysMessageId, MsgStatus = sysMessageStatus.ToString() });
        }

        /// <summary>
        /// 将所有上次提醒数据置换为已处理
        /// </summary>
        public void ResetAllSysMessageStatus()
        {
            SelfDber.Execute("update " + EntityReflectionUtil.GetTableName<CmcsSysMessage>() + " t set t.MsgStatus=:MsgStatus1 where t.MsgStatus=:MsgStatus1", new { MsgStatus1 = eSysMessageStatus.处理中.ToString(), MsgStatus2 = eSysMessageStatus.处理中.ToString() });
        }

        /// <summary>
        /// 添加提示信息
        /// </summary>
        /// <param name="msgCode">编码</param>
        /// <param name="msgContent">内容</param>
        /// <param name="windowTitle">标题</param>
        /// <param name="msgButton">按钮名称</param>
        /// <param name="msgWarnType">是否右下角弹出</param>
        /// <param name="isAutoClose">是否自动关闭</param>
        /// <param name="msgParam">JSON</param>
        /// <returns></returns>
        public bool SaveSysMessage(String msgCode, String msgContent, String windowTitle = "提示", String msgButton = "查看|取消", bool msgWarnType = true, bool isAutoClose = false, String msgParam = "")
        {
            if (!HasSysMessage(msgCode, msgContent))
            {
                return SelfDber.Insert(new CmcsSysMessage
                {
                    MsgTime = DateTime.Now,
                    MsgParam = msgParam,
                    MsgContent = msgContent,
                    WindowsTitle = windowTitle,
                    MsgWarnType = msgWarnType ? 1 : 0,
                    IsAutoClose = isAutoClose ? 1 : 0,
                    MsgCode = msgCode,
                    MsgButton = msgButton,
                }) > 0;
            }

            return false;
        }

        /// <summary>
        /// 是否存在未处理的数据
        /// </summary>
        /// <param name="msgCode">编码</param>
        /// <param name="msgContent">内容</param>
        /// <returns></returns>
        public bool HasSysMessage(String msgCode, String msgContent)
        {
            return SelfDber.Count<CmcsSysMessage>("where (MsgStatus=:MsgStatus1 or MsgStatus=:MsgStatus2) and MsgContent=:MsgContent and MsgCode=:MsgCode", new { MsgStatus1 = eSysMessageStatus.处理中.ToString(), MsgStatus2 = eSysMessageStatus.处理中.ToString(), MsgContent = msgContent, MsgCode = msgCode }) > 0;
        }

        #endregion

        #region 基础信息

        /// <summary>
        /// 根据供煤单位名称或首字母模糊查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sqlWhere">条件语句</param>
        /// <returns></returns>
        public List<CmcsSupplier> GetSupplierByNameOrChs(string name, string sqlWhere)
        {
            List<CmcsSupplier> res = SelfDber.Entities<CmcsSupplier>(sqlWhere);

            if (!string.IsNullOrEmpty(name))
            {
                return res.Where(a =>
                {
                    if (a.Name.ToUpper().Contains(name.ToUpper())) return true;
                    if (ChsSpeller.GetFirst(a.Name).ToUpper().Contains(name.ToUpper())) return true;

                    return false;
                }).ToList();
            }
            else
                return res;
        }

        /// <summary>
        /// 根据矿点名称或首字母模糊查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sqlWhere">条件语句</param>
        /// <returns></returns>
        public List<CmcsMine> GetMineByNameOrChs(string name, string sqlWhere)
        {
            List<CmcsMine> res = SelfDber.Entities<CmcsMine>(sqlWhere);

            if (!string.IsNullOrEmpty(name))
            {
                return res.Where(a =>
                {
                    if (a.Name.ToUpper().Contains(name.ToUpper())) return true;
                    if (ChsSpeller.GetFirst(a.Name).ToUpper().Contains(name.ToUpper())) return true;

                    return false;
                }).ToList();
            }
            else
                return res;
        }

        /// <summary>
        /// 根据运输单位名称或首字母模糊查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sqlWhere">条件语句</param>
        /// <returns></returns>
        public List<CmcsTransportCompany> GetTransportCompanyByNameOrChs(string name, string sqlWhere)
        {
            List<CmcsTransportCompany> res = SelfDber.Entities<CmcsTransportCompany>(sqlWhere);

            if (!string.IsNullOrEmpty(name))
            {
                return res.Where(a =>
                {
                    if (a.Name.ToUpper().Contains(name.ToUpper())) return true;
                    if (ChsSpeller.GetFirst(a.Name).ToUpper().Contains(name.ToUpper())) return true;

                    return false;
                }).ToList();
            }
            else
                return res;
        }

        /// <summary>
        /// 根据供货收货单位名称或首字母模糊查询数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sqlWhere">条件语句</param>
        /// <returns></returns>
        public List<CmcsSupplyReceive> GetSupplyReceiveByNameOrChs(string name, string sqlWhere)
        {
            List<CmcsSupplyReceive> res = SelfDber.Entities<CmcsSupplyReceive>(sqlWhere);

            if (!string.IsNullOrEmpty(name))
            {
                return res.Where(a =>
                {
                    if (a.UnitName.ToUpper().Contains(name.ToUpper())) return true;
                    if (ChsSpeller.GetFirst(a.UnitName).ToUpper().Contains(name.ToUpper())) return true;

                    return false;
                }).ToList();
            }
            else
                return res;
        }

        #endregion

        #region 车辆管理

        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="carNumber"></param>
        /// <returns></returns>
        public CmcsAutotruck GetAutotruckByCarNumber(string carNumber)
        {
            return SelfDber.Entity<CmcsAutotruck>("where CarNumber=:CarNumber", new { CarNumber = carNumber });
        }

        #endregion

        #region 程序远程控制

        /// <summary>
        /// 发送程序远程控制命令
        /// </summary>
        /// <param name="appIdentifier">程序唯一标识符</param>
        /// <param name="cmdCode">命令代码</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public bool SendAppRemoteControlCmd(string appIdentifier, string cmdCode, string param = "")
        {

            return SelfDber.Insert(new CmcsAppRemoteControlCmd
               {
                   AppIdentifier = appIdentifier,
                   CmdCode = cmdCode,
                   Param = param,
                   ResultCode = eEquInfCmdResultCode.默认.ToString(),
                   DataFlag = 0
               }) > 0;
        }

        /// <summary>
        /// 重置程序远程控制命令
        /// </summary>
        /// <param name="appIdentifier"></param>
        public void ResetAppRemoteControlCmd(string appIdentifier)
        {
            SelfDber.Execute("update " + EntityReflectionUtil.GetTableName<CmcsAppRemoteControlCmd>() + " set DataFlag=1 where AppIdentifier=:AppIdentifier", new { AppIdentifier = appIdentifier });
        }

        /// <summary>
        /// 获取最新的程序远程控制命令
        /// </summary>
        /// <param name="appIdentifier">程序唯一标识</param>
        /// <returns></returns>
        public CmcsAppRemoteControlCmd GetNewestAppRemoteControlCmd(string appIdentifier)
        {
            return SelfDber.Entity<CmcsAppRemoteControlCmd>("where AppIdentifier=:AppIdentifier and DataFlag=0 order by CreateDate asc", new { AppIdentifier = appIdentifier });
        }

        /// <summary>
        /// 获取最新的程序远程控制命令
        /// </summary>
        /// <param name="appIdentifier">程序唯一标识</param>
        /// <returns></returns>
        public CmcsAppRemoteControlCmd GetNewestAppRemoteControlCmd(string appIdentifier, string cmdCode)
        {
            return SelfDber.Entity<CmcsAppRemoteControlCmd>("where AppIdentifier=:AppIdentifier and CmdCode=:CmdCode order by CreateDate asc", new { AppIdentifier = appIdentifier, CmdCode = cmdCode });
        }

        /// <summary>
        /// 更改程序远程控制命令的执行结果
        /// </summary>
        /// <param name="appRemoteControlCmd"></param>
        /// <param name="cmdResultCode"></param>
        public void SetAppRemoteControlCmdResultCode(CmcsAppRemoteControlCmd appRemoteControlCmd, eEquInfCmdResultCode cmdResultCode)
        {
            appRemoteControlCmd.DataFlag = 1;
            appRemoteControlCmd.ResultCode = cmdResultCode.ToString();
            SelfDber.Update(appRemoteControlCmd);
        }

        #endregion

        #region 入厂煤批次

        /// <summary>
        /// 生成制定日期的批次编码
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="dt">实际到厂时间</param>
        /// <returns></returns>
        public string CreateNewBatchNumber(string prefix, DateTime dt)
        {
            CmcsInFactoryBatch entity = SelfDber.Entity<CmcsInFactoryBatch>("where Batch like '%-'||to_char(:CreateDate,'YYYYMMDD')||'-%' and Batch like :Prefix || '%' order by Batch desc", new { CreateDate = dt, Prefix = prefix + "-" + dt.ToString("yyyyMMdd") });

            if (entity == null)
                return string.Format("{0}-{1}-01", prefix, dt.ToString("yyyyMMdd"));
            else
                return string.Format("{0}-{1}-{2}", prefix, dt.ToString("yyyyMMdd"), (Convert.ToInt16(entity.Batch.Replace(string.Format("{0}-{1}-", prefix, dt.ToString("yyyyMMdd")), "")) + 1).ToString().PadLeft(2, '0'));
        }

        #endregion

        #region 采制化三级编码

        /// <summary>
        /// 根据批次ID获取采样记录，如果没有则创建
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public CmcsRCSampling GCRcSampling(CmcsInFactoryBatch batch)
        {
            CmcsRCSampling sampling = SelfDber.Entity<CmcsRCSampling>("where InFactoryBatchId=:InFactoryBatchId", new { InFactoryBatchId = batch.Id });
            if (sampling == null)
            {
                sampling = new CmcsRCSampling()
                {
                    InFactoryBatchId = batch.Id,
                    Weath = "晴",
                    SamplingDate = DateTime.Now,
                    DataFrom = GlobalVars.DataFrom
                };

                SelfDber.Insert(sampling);

                CmcsRCSamplingDetail samplingDetail = new CmcsRCSamplingDetail()
                {
                    SamplingId = sampling.Id,
                    SamplingType = "机器采样",
                    CarCount = 0,
                    BagNum = 1,
                    BillNumber = CreateNewSampleNumber(batch.FactArriveDate),
                    DataFrom = GlobalVars.DataFrom
                };
                SelfDber.Insert(samplingDetail);
            }
            return sampling;
        }

        /// <summary>
        /// 生成采样码
        /// </summary> 
        /// <param name="dt"></param>
        /// <returns></returns>
        public string CreateNewSampleNumber(DateTime dt)
        {
            string res = string.Empty;
            do
            {
                res = string.Format("CY{0}{1}", dt.ToString("yyMMdd"), new Random().Next(0, 1000).ToString().PadLeft(3, '0'));
            } while (SelfDber.Count<CmcsRCSamplingDetail>("where BillNumber=:BillNumber", new { BillNumber = res }) > 0);

            return res;
        }

        /// <summary>
        /// 根据批次Id获取采样明细记录
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public CmcsRCSamplingDetail GetRcSamplingDetailByBatchIdForCar(string batchId)
        {
            CmcsRCSamplingDetail res = null;

            CmcsRCSampling sampling = SelfDber.Entity<CmcsRCSampling>("where InFactoryBatchId=:InFactoryBatchId", new { InFactoryBatchId = batchId });
            if (sampling != null) res = SelfDber.Entity<CmcsRCSamplingDetail>("where SamplingId=:SamplingId", new { SamplingId = sampling.Id });

            return res;
        }

        /// <summary>
        /// 采样
        /// </summary>
        /// <param name="inFactoryBatchId"></param>
        /// <param name="carNumber"></param>
        /// <returns></returns>
        public bool CreateSampleNums(string inFactoryBatchId, string carNumber)
        {
            CmcsRCSampling sampling = SelfDber.Entity<CmcsRCSampling>("where InFactoryBatchId=:InFactoryBatchId order by CreateDate desc", new { InFactoryBatchId = inFactoryBatchId });
            if (sampling != null)
            {
                sampling.TotalNum += 1;
                sampling.CarNums += (string.IsNullOrEmpty(sampling.CarNums) ? carNumber : "," + carNumber);
                return SelfDber.Update(sampling) > 0;
            }
            //CmcsRCSamplingDetail samplingDetail = GetRcSamplingDetailByBatchIdForCar(inFactoryBatchId);
            //samplingDetail.BagNum = samplingDetail.BagNum + 1;
            //return SelfDber.Update(samplingDetail) > 0;
            return false;
        }
        #endregion

        #region 数据待处理事件

        /// <summary>
        /// 插入数据待处理事件
        /// </summary>
        /// <param name="eventCode">事件代码</param>
        /// <param name="objectId">业务记录Id</param>
        /// <param name="paramValue1">附加参数1</param>
        /// <param name="paramValue2">附加参数2</param>
        /// <returns></returns>
        public bool InsertWaitForHandleEvent(string eventCode, string objectId, string paramValue1 = "", string paramValue2 = "")
        {
            return SelfDber.Insert(new CmcsWaitForHandleEvent
            {
                EventCode = eventCode,
                ObjectId = objectId,
                ParamValue1 = paramValue1,
                ParamValue2 = paramValue2
            }) > 0;
        }

        #endregion
    }
}
