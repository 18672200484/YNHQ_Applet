using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CMCS.Common.Entities;
using CMCS.DapperDber.Util;
using System.Data;
using CMCS.Common.Enums;

namespace CMCS.Common.DAO
{
    /// <summary>
    /// 制样机业务
    /// </summary>
    public class MakerDAO
    {
        private static MakerDAO instance;

        public static MakerDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new MakerDAO();
            }

            return instance;
        }

        private MakerDAO()
        { }

        /// <summary>
        /// 发送制样计划
        /// </summary>
        public bool SaveMakerPlan(CmcsMakerPlan entity, out string message)
        {
            try
            {
                message = "制样计划发送成功";
                if (Dbers.GetInstance().SelfDber.Insert<CmcsMakerPlan>(entity) > 0)
                {
                    CmcsMakerControlCmd makerControlCmd = new CmcsMakerControlCmd();
                    makerControlCmd.InterfaceType = CommonDAO.GetInstance().GetMachineInterfaceTypeByCode(entity.MachineCode);
                    makerControlCmd.MachineCode = entity.MachineCode;
                    makerControlCmd.MakeCode = entity.MakeCode;
                    makerControlCmd.ResultCode = eEquInfCmdResultCode.默认.ToString();
                    makerControlCmd.CmdCode = eMakeCMDCode.开始制样.ToString();
                    Dbers.GetInstance().SelfDber.Insert<CmcsMakerControlCmd>(makerControlCmd);
                    return true;
                }
                message = "制样计划发送失败";
                return false;
            }
            catch (Exception ex)
            {
                message = "制样计划发送失败!" + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 保存制样明细记录
        /// </summary>
        public bool SaveMakerRecord(CmcsMakerRecord entity)
        {
            return Dbers.GetInstance().SelfDber.Insert<CmcsMakerRecord>(entity) > 0 ? true : false;
        }

        /// <summary>
        /// 根据采样id获取制样记录
        /// </summary>
        /// <param name="sampleId"></param>
        /// <returns></returns>
        public CmcsRCMake GetRCMakeBySampleId(string sampleId)
        {
            CmcsRCMake rcmake = Dbers.GetInstance().SelfDber.Entity<CmcsRCMake>("where SamplingId=:SamplingId", new { SamplingId = sampleId });
            return rcmake;
        }

        /// <summary>
        /// 历史制样记录分页查询
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="CurrentIndex">当前页索引</param>
        /// <param name="sqlWhere">sql条件</param>
        /// <returns></returns>
        public List<CmcsMakerRecord> ExecutePager(int PageSize, int CurrentIndex, string sqlWhere)
        {
            return Dbers.GetInstance().SelfDber.ExecutePager<CmcsMakerRecord>(PageSize, CurrentIndex, sqlWhere + " order by CreateDate desc");
        }

        /// <summary>
        /// 历史制样记录总数
        /// </summary>
        /// <param name="sqlWhere">sql条件</param>
        /// <returns></returns>
        public int GetTotalCount(string sqlWhere)
        {
            return Dbers.GetInstance().SelfDber.Count<CmcsMakerRecord>(sqlWhere);
        }

        /// <summary>
        /// 获取待同步到第三方接口的制样计划
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <returns></returns>
        public List<CmcsMakerPlan> GetWaitForSyncMakePlan(string interfaceType)
        {
            return Dbers.GetInstance().SelfDber.Entities<CmcsMakerPlan>("where InterfaceType=:InterfaceType and DataFlag=0", new { InterfaceType = interfaceType });
        }

        /// <summary>
        /// 获取待同步到第三方接口的控制命令表
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <returns></returns>
        public List<CmcsMakerControlCmd> GetWaitForSyncMakerControlCmd(string interfaceType)
        {
            return Dbers.GetInstance().SelfDber.Entities<CmcsMakerControlCmd>("where InterfaceType=:InterfaceType and DataFlag=0", new { InterfaceType = interfaceType });
        }

        #region 获取采样单
        /// <summary>
        /// 获取采样单信息
        /// </summary>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public DataTable GetSampleInfo(DateTime dtStart, DateTime dtEnd)
        {
            string sql = @" select a.batch,a.id as batchid,
                                 b.name as suppliername,
                                 c.name as minename,
                                 d.name as kindname,
                                 e.name as stationname,
                                 a.factarrivedate,
                                 t.id,
                                 t.samplecode,
                                 t.samplingdate,
                                 t.samplingtype
                            from cmcstbrcsampling t
                            left join cmcstbinfactorybatch a on t.infactorybatchid = a.id
                            left join cmcstbsupplier b on a.supplierid = b.id
                            left join cmcstbmine c on a.mineid = c.id
                            left join cmcstbfuelkind d on a.fuelkindid = d.id
                            left join cmcstbstationinfo e on a.stationid = e.id
                       where t.samplingdate >= '" + dtStart + "' and t.samplingdate < '" + dtEnd + "'";
            return Dbers.GetInstance().SelfDber.ExecuteDataTable(sql);
        }
        #endregion
    }
}