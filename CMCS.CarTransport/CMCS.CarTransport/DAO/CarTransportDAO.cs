using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.DAO;
using CMCS.Common.Entities;
using CMCS.DapperDber.Dbs.OracleDb;
using CMCS.Common;
using CMCS.Common.Utilities;
using CMCS.Common.Entities.CarTransport;
using CMCS.DapperDber.Util;
using CMCS.Common.Enums;
using CMCS.Common.Views;
using CMCS.CarTransport.Views;
using CMCS.Common.Entities.Fuel;
using CMCS.Common.Entities.BaseInfo;
using System.Data;
using CMCS.DapperDber.Dbs.SqlServerDb;
using CMCS.Common.Entities.CarTransport.DTEntity;
using CMCS.Common.Unitl;
using CMCS.Common.Entities.iEAA;

namespace CMCS.CarTransport.DAO
{
    /// <summary>
    /// 汽车智能化业务
    /// </summary>
    public class CarTransportDAO
    {
        private static CarTransportDAO instance;

        public static CarTransportDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new CarTransportDAO();
            }

            return instance;
        }

        private CarTransportDAO()
        { }

        public OracleDapperDber SelfDber
        {
            get { return Dbers.GetInstance().SelfDber; }
        }
        public SqlServerDapperDber SqlServerDber
        {
            get { return Dbers.GetInstance().SqlServerDber; }
        }

        CommonDAO commonDAO = CommonDAO.GetInstance();

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

        /// <summary>
        /// 根据标签卡号获取车辆信息
        /// </summary>
        /// <param name="carNumber"></param>
        /// <returns></returns>
        public CmcsAutotruck GetAutotruckByTagId(string tagId)
        {
            CmcsAdvance advance = SelfDber.Entity<CmcsAdvance>("where Tag=:Tag order by createdate desc ", new { Tag = tagId });
            if (advance != null) return SelfDber.Entity<CmcsAutotruck>("where CarNumber=:CarNumber", new { CarNumber = advance.CarNumber });
            return null;
        }

        #endregion

        #region 省份简称

        /// <summary>
        /// 获取省份简称，并按照使用次数降序
        /// </summary>
        /// <returns></returns>
        public List<CmcsProvinceAbbreviation> GetProvinceAbbreviationsInOrder()
        {
            return Dbers.GetInstance().SelfDber.Entities<CmcsProvinceAbbreviation>("order by UseCount desc");
        }

        /// <summary>
        /// 增加省份简称的使用次数
        /// </summary>
        /// <param name="paName">省份简称</param>
        public void AddProvinceAbbreviationUseCount(string paName)
        {
            Dbers.GetInstance().SelfDber.Execute("update " + EntityReflectionUtil.GetTableName<CmcsProvinceAbbreviation>() + " set UseCount=UseCount+1 where PaName=:PaName", new { PaName = paName });
        }

        #endregion

        #region 批次与采制化

        /// <summary>
        /// 根据运输记录判断批次是否已生成，并返回。
        /// 根据入厂时间（实际到厂时间）、供煤单位、煤种判断
        /// </summary>
        /// <param name="buyFuelTransport"></param>
        /// <returns></returns>
        public CmcsInFactoryBatch HasInFactoryBatch(CmcsBuyFuelTransport buyFuelTransport)
        {
            DateTime dt = buyFuelTransport.TareTime.Year < 2000 ? buyFuelTransport.CreateDate.AddHours(-commonDAO.GetCommonAppletConfigInt32("汽车分批时间点")) : buyFuelTransport.TareTime.AddHours(-commonDAO.GetCommonAppletConfigInt32("汽车分批时间点"));
            //DateTime dt = buyFuelTransport.CreateDate.AddHours(-commonDAO.GetCommonAppletConfigInt32("汽车分批时间点"));
            return SelfDber.Entity<CmcsInFactoryBatch>("where Batch like '%-'|| to_char(:CreateDate,'YYYYMMDD') ||'-%' and MineId=:MineId and BatchCreateType=1", new { CreateDate = dt, MineId = buyFuelTransport.MineId });
        }

        /// <summary>
        /// 生成制定日期的批次编码
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="dtFactArriveDate">实际到厂时间</param>
        /// <returns></returns>
        public string CreateNewBatchNumber(string prefix, DateTime dtFactArriveDate)
        {
            DateTime dt = dtFactArriveDate.AddHours(-commonDAO.GetCommonAppletConfigInt32("汽车分批时间点"));
            CmcsInFactoryBatch entity = SelfDber.Entity<CmcsInFactoryBatch>("where Batch like '%-'||to_char(:CreateDate,'YYYYMMDD')||'-%' and Batch like :Prefix || '%' order by Batch desc", new { CreateDate = dt, Prefix = prefix + "-" + dt.ToString("yyyyMMdd") });

            if (entity == null)
                return string.Format("{0}-{1}-01", prefix, dt.ToString("yyyyMMdd"));
            else
                return string.Format("{0}-{1}-{2}", prefix, dt.ToString("yyyyMMdd"), (Convert.ToInt16(entity.Batch.Replace(string.Format("{0}-{1}-", prefix, dt.ToString("yyyyMMdd")), "")) + 1).ToString().PadLeft(2, '0'));
        }

        /// <summary>
        /// 根据运输记录生成批次并返回。
        /// 根据入厂时间（实际到厂时间）、供煤单位、煤种生成，已存在则不创建
        /// </summary>
        /// <param name="buyFuelTransport"></param>
        /// <returns></returns>
        public CmcsInFactoryBatch GCQCInFactoryBatchByBuyFuelTransport(CmcsBuyFuelTransport buyFuelTransport)
        {
            bool isSuccess = true;
            CmcsTransportType transportType = commonDAO.SelfDber.Entity<CmcsTransportType>("where Name=:Name", new { Name = "公路" });
            CmcsInFactoryBatch entity = HasInFactoryBatch(buyFuelTransport);
            if (entity == null)
            {
                entity = new CmcsInFactoryBatch()
                {
                    Batch = CreateNewBatchNumber("QC", buyFuelTransport.TareTime.Year < 2000 ? buyFuelTransport.CreateDate : buyFuelTransport.TareTime),
                    BatchType = "汽车",
                    FactArriveDate = buyFuelTransport.InFactoryTime,
                    FuelKindId = buyFuelTransport.FuelKindId,
                    FuelKindName = buyFuelTransport.FuelKindName,
                    SupplierId = buyFuelTransport.SupplierId,
                    MineId = buyFuelTransport.MineId,
                    RunDate = buyFuelTransport.InFactoryTime,
                    Remark = "由汽车煤智能化自动创建",
                    IsCheck = 0,
                    BatchCreateType = 1,
                    DataFrom = GlobalVars.DataFrom,
                    BACKBATCHDATE = DateTime.Now,
                    TransportTypeName = "公路"
                };
                if (transportType != null)
                    entity.TransportTypeId = transportType.Id;

                // 创建新批次
                isSuccess = SelfDber.Insert(entity) > 0;
                if (isSuccess)
                {
                    // 生成采样记录
                    commonDAO.GCRcSampling(entity);
                }
            }
            else
            {
                entity.SupplierId = buyFuelTransport.SupplierId;//运输记录没有选择共供应商，补选供应商后重新赋值。
                entity.FuelKindId = buyFuelTransport.FuelKindId;
                entity.FuelKindName = buyFuelTransport.FuelKindName;
                SelfDber.Update(entity);
            }
            return entity;
        }

        /// <summary>
        /// 根据采样单id获取采样单
        /// </summary>
        /// <param name="samplingId">采样单id</param>
        /// <returns></returns>
        public CmcsRCSampling GetRCSamplingById(string samplingId)
        {
            return SelfDber.Entity<CmcsRCSampling>("where Id=:Id", new { Id = samplingId });
        }

        #endregion

        #region 预制信息
        /// <summary>
        /// 根据标签卡号获取预制信息
        /// </summary>
        /// <param name="carNumber"></param>
        /// <returns></returns>
        public CmcsAdvance GetAdvanceByTagId(string tagId)
        {
            CmcsAdvance advance = SelfDber.Entity<CmcsAdvance>("where Tag=:Tag order by createdate desc ", new { Tag = tagId });
            return advance;
        }
        /// <summary>
        /// 根据车号获取预制信息
        /// </summary>
        /// <param name="carNumber"></param>
        /// <returns></returns>
        public CmcsAdvance GetAdvanceByCarNumber(string carNumber)
        {
            CmcsAdvance advance = SelfDber.Entity<CmcsAdvance>("where CarNumber=:CarNumber order by createdate desc ", new { CarNumber = carNumber });
            return advance;
        }

        #endregion

        #region 运输记录

        #region 入场煤

        /// <summary>
        /// 获取入厂煤运输记录
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public List<View_BuyFuelTransport> GetBuyFuelTransportByStrWhere(string strWhere)
        {
            return SelfDber.Entities<View_BuyFuelTransport>(strWhere);
        }

        /// <summary>
        /// 获取指定日期已完成的入厂煤运输记录
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public List<View_BuyFuelTransport> GetFinishedBuyFuelTransport(DateTime dtStart, DateTime dtEnd)
        {
            return SelfDber.Entities<View_BuyFuelTransport>("where IsHidden=0 and SuttleWeight!=0 and TareTime>=:dtStart and TareTime<:dtEnd order by TareTime desc", new { dtStart = dtStart, dtEnd = dtEnd });
        }

        /// <summary>
        /// 获取未完成的入厂煤运输记录
        /// </summary>
        /// <returns></returns>
        public List<View_BuyFuelTransport> GetUnFinishBuyFuelTransport()
        {
            return SelfDber.Entities<View_BuyFuelTransport>("where IsHidden=0 and SuttleWeight=0 and UnFinishTransportId is not null order by grosstime desc,InFactoryTime desc");
        }

        /// <summary>
        /// 获取未完成的入厂煤运输记录
        /// </summary>
        /// <returns></returns>
        public List<View_BuyFuelTransport> GetUnFinishBuyFuelTransport(string ordertype)
        {
            return SelfDber.Entities<View_BuyFuelTransport>("where IsHidden=0 and SuttleWeight=0 and UnFinishTransportId is not null " + ordertype);
        }

        /// <summary>
        /// 获取今日运输记录
        /// </summary>
        /// <returns></returns>
        public IList<CmcsBuyFuelTransport> GetBuyFuelTransportToDay()
        {
            return SelfDber.Entities<CmcsBuyFuelTransport>("where trunc(InFactoryTime)=trunc(sysdate)");
        }

        #endregion

        #region 其它物资
        /// <summary>
        /// 获取今日运输记录
        /// </summary>
        /// <returns></returns>
        public IList<CmcsGoodsTransport> GetGoodsTransportToDay()
        {
            return SelfDber.Entities<CmcsGoodsTransport>("where trunc(InFactoryTime)=trunc(sysdate)");
        }

        /// <summary>
        /// 获取指定日期已完成的其他物资运输记录
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<CmcsGoodsTransport> GetFinishedGoodsTransport(DateTime dtStart, DateTime dtEnd)
        {
            return SelfDber.Entities<CmcsGoodsTransport>("where SuttleWeight>0 and InFactoryTime>=:dtStart and InFactoryTime<:dtEnd order by InFactoryTime desc", new { dtStart = dtStart, dtEnd = dtEnd });
        }

        /// <summary>
        /// 获取未完成的其他物资运输记录
        /// </summary>
        /// <returns></returns>
        public List<CmcsGoodsTransport> GetUnFinishGoodsTransport()
        {
            return SelfDber.Entities<CmcsGoodsTransport>("where SuttleWeight=0 and IsUse=1 and Id in (select TransportId from " + EntityReflectionUtil.GetTableName<CmcsUnFinishTransport>() + " where CarType=:CarType) order by InFactoryTime desc", new { CarType = eCarType.其他物资.ToString() });
        }

        #endregion

        /// <summary>
        /// 根据车Id获取未完成的运输记录
        /// </summary>
        /// <param name="autotruckId">车Id</param>
        /// <returns></returns>
        public CmcsUnFinishTransport GetUnFinishTransportByAutotruckId(string autotruckId, string carType)
        {
            return SelfDber.Entity<CmcsUnFinishTransport>("where AutotruckId=:AutotruckId and CarType=:CarType", new { AutotruckId = autotruckId, CarType = carType });
        }

        /// <summary>
        /// 根据运输记录Id获取未完成的运输记录
        /// </summary>
        /// <param name="autotruckId">运输Id</param>
        /// <returns></returns>
        public CmcsUnFinishTransport GetUnFinishTransportByTransportId(string transportId)
        {
            return SelfDber.Entity<CmcsUnFinishTransport>("where TransportId=:TransportId", new { TransportId = transportId });
        }

        /// <summary>
        /// 根据车牌号获取未完成的运输记录
        /// </summary>
        /// <param name="autotruckId">车牌号</param>
        /// <returns></returns>
        public List<View_UnFinishTransport> GetUnFinishTransportByCarNumber(string carNumber, string sqlWhere)
        {
            List<View_UnFinishTransport> res = SelfDber.Entities<View_UnFinishTransport>(sqlWhere);
            if (!string.IsNullOrEmpty(carNumber))
            {
                return res.Where(a =>
                {
                    if (a.CarNumber.ToUpper().Contains(carNumber.ToUpper())) return true;

                    return false;
                }).ToList();
            }
            else
                return res;
        }

        /// <summary>
        /// 根据运输记录id查找入厂煤运输记录
        /// </summary>
        /// <param name="transportId">未完成的运输记录Id</param>
        /// <returns></returns>
        public CmcsBuyFuelTransport GetBuyFuelTransportById(string transportId)
        {
            return SelfDber.Get<CmcsBuyFuelTransport>(transportId);
        }

        /// <summary>
        /// 将指定车的未完结运输记录强制更改为无效
        /// </summary>
        /// <param name="autotruckId">车Id</param>
        public void ChangeUnFinishTransportToInvalid(string autotruckId)
        {
            if (string.IsNullOrEmpty(autotruckId)) return;

            foreach (CmcsUnFinishTransport unFinishTransport in SelfDber.Entities<CmcsUnFinishTransport>("where AutotruckId=:AutotruckId", new { AutotruckId = autotruckId }))
            {
                if (unFinishTransport.CarType == eCarType.入厂煤.ToString())
                {
                    SelfDber.Execute("Update " + EntityReflectionUtil.GetTableName<CmcsBuyFuelTransport>() + " set IsUse=0 where Id=:Id", new { Id = unFinishTransport.TransportId });
                    SelfDber.Delete<CmcsUnFinishTransport>(unFinishTransport.Id);
                }
                if (unFinishTransport.CarType == eCarType.其他物资.ToString())
                {
                    SelfDber.Execute("Update " + EntityReflectionUtil.GetTableName<CmcsGoodsTransport>() + " set IsUse=0 where Id=:Id", new { Id = unFinishTransport.TransportId });
                    SelfDber.Delete<CmcsUnFinishTransport>(unFinishTransport.Id);
                }
            }
        }

        /// <summary>
        /// 生成运输记录流水号
        /// </summary>
        /// <param name="carType">车类型</param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string CreateNewTransportSerialNumber(eCarType carType, DateTime dt)
        {
            string prefix = "Null";

            if (carType == eCarType.入厂煤)
            {
                prefix = "RCM";

                CmcsBuyFuelTransport entity = SelfDber.Entity<CmcsBuyFuelTransport>("where CreateDate>=:StartDate and CreateDate<:EndDate and SerialNumber like :Prefix || '%' order by InFactoryTime desc", new { StartDate = dt.Date, EndDate = dt.AddDays(1).Date, Prefix = prefix });
                if (entity == null)
                    return prefix + dt.ToString("yyMMdd") + "001";
                else
                    return prefix + dt.ToString("yyMMdd") + (Convert.ToInt16(entity.SerialNumber.Replace(prefix + dt.ToString("yyMMdd"), "")) + 1).ToString().PadLeft(3, '0');
            }
            else if (carType == eCarType.其他物资)
            {
                prefix = "WZ";

                CmcsGoodsTransport entity = SelfDber.Entity<CmcsGoodsTransport>("where CreateDate>=:StartDate and CreateDate<:EndDate and SerialNumber like :Prefix || '%' order by InFactoryTime desc", new { StartDate = dt.Date, EndDate = dt.AddDays(1).Date, Prefix = prefix });
                if (entity == null)
                    return prefix + dt.ToString("yyMMdd") + "001";
                else
                    return prefix + dt.ToString("yyMMdd") + (Convert.ToInt16(entity.SerialNumber.Replace(prefix + dt.ToString("yyMMdd"), "")) + 1).ToString().PadLeft(3, '0');
            }
            return prefix + dt.ToString("yyMMdd") + DateTime.Now.Second.ToString().PadLeft(3, '0');
        }

        /// <summary>
        /// 根据汽车入厂路线设置，判断当前是否准许通过，不通过则返回下一地点的位置
        /// </summary>
        /// <param name="carType">车类型</param>
        /// <param name="currentStepName">此车当前所处步骤</param>
        /// <param name="thisSetpName">当前地点代表的步骤</param>
        /// <param name="thisPlaceName">当前地点</param>
        /// <param name="nextPlace">返回下一地点的位置</param>
        /// <returns></returns>
        public bool CheckNextTruckInFactoryWay(string carType, string currentStepName, string thisSetpName, string thisPlaceName, out string nextPlace)
        {
            nextPlace = string.Empty;

            // 查找启用的路线，若没有启用的线路则通过
            CmcsTruckInFactoryWay truckInFactoryWay = SelfDber.Entity<CmcsTruckInFactoryWay>("where WayType=:WayType", new { WayType = carType });
            if (truckInFactoryWay == null) return true;

            // 查找当前所处的步骤
            CmcsTruckInFactoryWayDetail currentTruckInFactoryWayDetail = SelfDber.Entity<CmcsTruckInFactoryWayDetail>("where StepName=:StepName and TruckInFactoryWayId=:TruckInFactoryWayId", new { StepName = currentStepName, TruckInFactoryWayId = truckInFactoryWay.Id });
            if (currentTruckInFactoryWayDetail == null) return false;

            // 查找下一步骤
            CmcsTruckInFactoryWayDetail nextTruckInFactoryWayDetail = SelfDber.Entity<CmcsTruckInFactoryWayDetail>("where TruckInFactoryWayId=:TruckInFactoryWayId and StepNumber=:StepNumber", new { TruckInFactoryWayId = truckInFactoryWay.Id, StepNumber = currentTruckInFactoryWayDetail.StepNumber + 1 });
            if (nextTruckInFactoryWayDetail == null) return false;

            // 首先判断步骤是否符合条件
            if (!thisSetpName.Contains(nextTruckInFactoryWayDetail.StepName) || !("|" + nextTruckInFactoryWayDetail.WayPalce + "|").Contains("|" + thisPlaceName + "|"))
            {
                // 步骤不符合

                string[] nextPlaces = nextTruckInFactoryWayDetail.WayPalce.Split('|');
                nextPlace = (nextPlaces.Length > 0) ? nextPlaces[0] : string.Empty;
                return false;
            }
            //else
            //{
            //    // 步骤符合，再判断地点是否符合条件

            //    if (!("|" + nextTruckInFactoryWayDetail.WayPalce + "|").Contains("|" + thisPlaceName + "|")) return false;
            //}

            return true;
        }



        /// <summary>
        /// 删除预制信息
        /// </summary>
        /// <param name="transportid"></param>
        /// <returns></returns>
        public bool DelAdvance(string transportid)
        {
            return SelfDber.DeleteBySQL<CmcsAdvance>("where TransportId=:TransportId", new { transportid }) > 0;
        }

        /// <summary>
        /// 删除未完成运输记录
        /// </summary>
        /// <param name="transportid"></param>
        /// <returns></returns>
        public bool DelUnFinishTransport(string transportid)
        {
            return SelfDber.DeleteBySQL<CmcsUnFinishTransport>("where TransportId=:TransportId", new { transportid }) > 0;
        }
        #endregion

        #region 矿点
        /// <summary>
        /// 获取默认矿点
        /// </summary>
        /// <returns></returns>
        public CmcsMine GetDefaultMine()
        {
            return commonDAO.SelfDber.Entity<CmcsMine>("where Name=:Name", new { Name = "未知" });
        }

        /// <summary>
        /// 更新矿点
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        public bool InsertMine(CmcsMine video)
        {
            CmcsMine entity = commonDAO.SelfDber.Entity<CmcsMine>("where Id=:Id", new { Id = video.Id });
            if (entity == null)
                return commonDAO.SelfDber.Insert(video) > 0;
            else
                return commonDAO.SelfDber.Update(video) > 0;
        }
        /// <summary>
        /// 删除矿点
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        IList<CmcsMine> delMines = new List<CmcsMine>();
        public bool DelMine(CmcsMine video)
        {
            int res = 0;
            string id = string.Empty;
            id = video.Id;
            delMines.Insert(0, video);
            IList<CmcsMine> list = commonDAO.SelfDber.Entities<CmcsMine>("where ParentId=:ParentId", new { ParentId = id });
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    DelMine(item);
                }
            }
            foreach (CmcsMine item in delMines)
            {
                //commonDAO.SelfDber.Delete<CmcsMine>(item.Id);
                item.Valid = "无效";
                commonDAO.SelfDber.Update(item);
                //CarTransportDAO.GetInstance().DelDttb_firm_info(item);
                res++;
            }
            delMines.Clear();
            return res > 0;
        }
        /// <summary>
        /// 根据父编码获取下级节点编码（父编码+2位逐级递增的数值）
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string GetMineNewChildCode(string strCode)
        {
            string strNewCode = strCode;
            CmcsMine mine = new CmcsMine();
            if (strCode == "00")
            {
                mine = commonDAO.SelfDber.Entity<CmcsMine>("where ParentId=:ParentId order by Createdate desc ", new { ParentId = "-1" });
            }
            else
            {
                mine = commonDAO.SelfDber.Entity<CmcsMine>("where Code like :Code ||'%' and Code !=:Code order by Createdate desc ", new { Code = strCode });
            }
            if (mine != null)
            {
                strNewCode = mine.Code.Replace(strCode, "");
                strNewCode = strCode + (Convert.ToInt32(strNewCode) + 1).ToString();
            }
            else
            {
                if (strCode == "00")
                    strNewCode = "0001";
                else
                {
                    strNewCode = strCode + "01";
                }
            }

            return strNewCode;
        }

        public string GetMineCode(CmcsMine parentMine)
        {
            CmcsMine query = commonDAO.SelfDber.Entity<CmcsMine>(" where ParentId=:ParentId order by code desc", new { ParentId = parentMine.Id });
            if (query == null)
                return parentMine.Code + "01";
            int index = 0;
            int initNumber = Convert.ToInt32(query.Code.Substring(query.Code.Length - 2, 2));
            string NewChildCode = "";
            while (index < 1000)
            {
                if (initNumber >= 999)
                    initNumber = 1;
                else
                    initNumber++;
                NewChildCode = parentMine.Code + initNumber.ToString().PadLeft(3, '0');
                if (!IsExistMineCode(NewChildCode))
                    return NewChildCode;
                index++;
            }
            return "";
        }

        public bool IsExistMineCode(string code)
        {
            return commonDAO.SelfDber.Count<CmcsMine>(" where code=:code", new { code = code }) > 0;
        }
        /// <summary>
        /// 获取排序号
        /// </summary>
        /// <param name="parentMine"></param>
        /// <returns></returns>
        public int GetMineOrderNumBer(CmcsMine parentMine)
        {
            CmcsMine mine = commonDAO.SelfDber.Entity<CmcsMine>(" where ParentId=:ParentId order by Sequence desc", new { ParentId = parentMine.Id });
            if (mine != null)
            {
                mine.Sequence++;
                return mine.Sequence;
            }
            return 0;
        }
        #endregion

        #region 供应商
        /// <summary>
        /// 获取默认供应商
        /// </summary>
        /// <returns></returns>
        public CmcsSupplier GetDefaultSupplier()
        {
            return commonDAO.SelfDber.Entity<CmcsSupplier>("where Name=:Name", new { Name = "未知" });
        }
        #endregion

        #region 煤种
        /// <summary>
        /// 获取默认煤种
        /// </summary>
        /// <returns></returns>
        public CmcsFuelKind GetDefaultFuelKind()
        {
            return commonDAO.SelfDber.Entity<CmcsFuelKind>("where FuelName=:FuelName", new { FuelName = "未知" });
        }

        /// <summary>
        /// 更新煤种
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        public bool InsertFuelKind(CmcsFuelKind video)
        {
            CmcsFuelKind entity = commonDAO.SelfDber.Entity<CmcsFuelKind>("where Id=:Id", new { Id = video.Id });
            if (entity == null)
                return commonDAO.SelfDber.Insert(video) > 0;
            else
                return commonDAO.SelfDber.Update(video) > 0;
        }
        /// <summary>
        /// 删除煤种
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        public bool DelFuelKind(CmcsFuelKind video)
        {
            int res = 0;
            string id = string.Empty;
            id = video.Id;

            IList<CmcsFuelKind> list = commonDAO.SelfDber.Entities<CmcsFuelKind>("where ParentId=:ParentId", new { ParentId = id });
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    DelFuelKind(item);
                }
            }
            else
            {
                commonDAO.SelfDber.Delete<CmcsFuelKind>(id);
                res++;
            }

            return res > 0;
        }
        /// <summary>
        /// 根据父编码获取下级节点编码（父编码+2位逐级递增的数值）
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string GetFuelKindNewChildCode(string strCode)
        {
            string strNewCode = strCode;
            CmcsFuelKind mine = new CmcsFuelKind();
            if (strCode == "00")
            {
                mine = commonDAO.SelfDber.Entity<CmcsFuelKind>("where ParentId=:ParentId order by FuelCode desc ", new { ParentId = "-1" });
            }
            else
            {
                mine = commonDAO.SelfDber.Entity<CmcsFuelKind>("where FuelCode like :Code ||'%' and Code !=:Code order by FuelCode desc ", new { Code = strCode });
            }
            if (mine != null)
            {
                strNewCode = mine.FuelCode.Replace(strCode, "");
                strNewCode = strCode + (Convert.ToInt32(strNewCode) + 1).ToString().PadLeft(2, '0');
            }
            else
            {
                if (strCode == "00")
                    strNewCode = "0001";
                else
                {
                    strNewCode = strCode + "01";
                }
            }

            return strNewCode;
        }
        #endregion

        #region 自动打印
        /// <summary>
        /// 获取自动打印的运输记录
        /// </summary>
        /// <returns></returns>
        public CmcsBuyFuelTransport GetAutoPrintTransport()
        {
            return SelfDber.Query<CmcsBuyFuelTransport>("select * from (select a.* from cmcstbbuyfueltransport a inner join cmcstbautoprint b on a.id =b.transportid where b.status=0 order by a.taretime desc ) where rownum=1").FirstOrDefault();
        }
        /// <summary>
        /// 更改自动打印状态为已打印
        /// </summary>
        /// <param name="transportid"></param>
        /// <returns></returns>
        public bool UpdatePrintStatus(string transportid)
        {
            return SelfDber.Execute(string.Format("update cmcstbautoprint set status=1 where transportid='{0}'", transportid)) > 0;
        }
        /// <summary>
        /// 插入自动打印
        /// </summary>
        /// <param name="transportid"></param>
        /// <returns></returns>
        public bool InsertAutoPrint(string transportid)
        {
            CmcsAutoPrint entity = new CmcsAutoPrint();
            entity.TransportId = transportid;
            entity.Status = 0;
            entity.PrintCount = 0;
            return SelfDber.Insert<CmcsAutoPrint>(entity) > 0;
        }
        #endregion

        #region 扣吨

        /// <summary>
        /// 插入系统自动扣吨
        /// </summary>
        /// <param name="deducttype"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public bool InsertDeduct(string transportid, string deducttype, decimal weight)
        {
            CmcsBuyFuelTransportDeduct entity = SelfDber.Entity<CmcsBuyFuelTransportDeduct>("where TransportId=:TransportId and Remark='系统自动扣水（涨吨）'", new { TransportId = transportid });
            if (entity != null)
            {
                entity.DeductWeight = weight;
                return SelfDber.Update(entity) > 0;
            }
            else
            {
                entity = new CmcsBuyFuelTransportDeduct();
                entity.TransportId = transportid;
                entity.DeductType = deducttype;
                entity.DeductWeight = weight;
                entity.Remark = "系统自动扣水（涨吨）";
                return SelfDber.Insert(entity) > 0;
            }
        }


        /// <summary>
        /// 写入扣吨明细
        /// </summary>
        /// <param name="transportdeduct"></param>
        /// <returns></returns>
        public bool ToTransportDeducts(CmcsBuyFuelTransportDeduct transportdeduct)
        {
            bool res = false;
            if (transportdeduct == null) return false;
            CmcsBuyFuelTransportDeduct tran = SelfDber.Entity<CmcsBuyFuelTransportDeduct>("where Id=:Id and TransportId=:TransportId", new { Id = transportdeduct.Id, TransportId = transportdeduct.TransportId });
            if (tran != null)
            {
                tran.DeductType = transportdeduct.DeductType;
                tran.DeductWeight = transportdeduct.DeductWeight;
                tran.Remark = transportdeduct.Remark;
                res = SelfDber.Update(tran) > 0;
            }
            else
            {
                tran = new CmcsBuyFuelTransportDeduct();
                tran.DeductType = transportdeduct.DeductType;
                tran.DeductWeight = transportdeduct.DeductWeight;
                tran.Remark = transportdeduct.Remark;
                tran.TransportId = transportdeduct.TransportId;
                res = SelfDber.Insert(tran) > 0;
            }
            return res;
        }

        /// <summary>
        /// 删除扣吨明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteTransportDeducts(string id)
        {
            SelfDber.DeleteBySQL<CmcsBuyFuelTransportDeduct>("where TransportId=:TransportId", new { TransportId = id });

            return SelfDber.Delete<CmcsBuyFuelTransportDeduct>(id) > 0;
        }

        /// <summary>
        /// 更新运输记录扣重
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateTransportDeductWeight(CmcsBuyFuelTransport transport)
        {
            bool res = false;
            if (transport != null)
            {
                List<CmcsBuyFuelTransportDeduct> tran = SelfDber.Entities<CmcsBuyFuelTransportDeduct>("where TransportId=:TransportId", new { TransportId = transport.Id });
                if (tran != null && tran.Count > 0)
                {
                    transport.DeductWeight = tran.Sum(a => a.DeductWeight);
                }
                else
                {
                    transport.DeductWeight = 0;
                }

                if (transport.CheckWeight > 0)
                {
                    if (transport.DeductWeight > 0)
                        transport.CheckWeight -= transport.DeductWeight;
                    else
                        transport.CheckWeight = transport.SuttleWeight;
                }
                res = SelfDber.Update(transport) > 0;
            }
            return res;
        }

        #endregion

        #region 与大唐的数据交互
        /// <summary>
        /// 读取大唐的车辆信息同步至智能化车辆信息
        /// </summary>
        /// <returns></returns>
        public int InsertAutoTruck()
        {
            int count = 0;
            DataTable data = SqlServerDber.ExecuteDataTable("select * from tb_vehicle_tare where CONVERT(varchar(20),taretime,120)>=CONVERT(varchar(20),GETDATE()-2,120) order by taretime desc ");
            if (data != null && data.Rows.Count > 0)
            {
                foreach (DataRow item in data.Rows)
                {
                    CmcsAutotruck autotruck = SelfDber.Entity<CmcsAutotruck>("where CarNumber=:CarNumber", new { CarNumber = item["licenseplate"].ToString() });
                    if (autotruck == null)
                    {
                        autotruck = new CmcsAutotruck()
                        {
                            CarNumber = item["licenseplate"].ToString(),
                            Driver = item["driver"].ToString(),
                            CarriageLength = Convert.ToInt32(item["vlength"]),
                            CarriageWidth = Convert.ToInt32(item["vwidth"]),
                            CarriageBottomToFloor = Convert.ToInt32(item["vheight"]),
                            CarType = "入厂煤",
                            LeftObstacle1 = Convert.ToInt32(item["stretch1"]),
                            LeftObstacle2 = Convert.ToInt32(item["stretch2"]),
                            LeftObstacle3 = Convert.ToInt32(item["stretch3"]),
                            LeftObstacle4 = Convert.ToInt32(item["stretch4"]),
                            LeftObstacle5 = Convert.ToInt32(item["stretch5"]),
                            RightObstacle1 = Convert.ToInt32(item["stretch1"]),
                            RightObstacle2 = Convert.ToInt32(item["stretch2"]),
                            RightObstacle3 = Convert.ToInt32(item["stretch3"]),
                            RightObstacle4 = Convert.ToInt32(item["stretch4"]),
                            RightObstacle5 = Convert.ToInt32(item["stretch5"]),
                            IsUse = 1
                        };
                        SelfDber.Insert(autotruck);
                        count++;
                    }
                    else
                    {
                        autotruck.Driver = item["driver"].ToString();
                        autotruck.CarriageLength = Convert.ToInt32(item["vlength"]);
                        autotruck.CarriageWidth = Convert.ToInt32(item["vwidth"]);
                        autotruck.CarriageBottomToFloor = Convert.ToInt32(item["vheight"]);
                        autotruck.CarType = "入厂煤";
                        autotruck.LeftObstacle1 = Convert.ToInt32(item["stretch1"]);
                        autotruck.LeftObstacle2 = Convert.ToInt32(item["stretch2"]);
                        autotruck.LeftObstacle3 = Convert.ToInt32(item["stretch3"]);
                        autotruck.LeftObstacle4 = Convert.ToInt32(item["stretch4"]);
                        autotruck.LeftObstacle5 = Convert.ToInt32(item["stretch5"]);
                        autotruck.RightObstacle1 = Convert.ToInt32(item["stretch1"]);
                        autotruck.RightObstacle2 = Convert.ToInt32(item["stretch2"]);
                        autotruck.RightObstacle3 = Convert.ToInt32(item["stretch3"]);
                        autotruck.RightObstacle4 = Convert.ToInt32(item["stretch4"]);
                        autotruck.RightObstacle5 = Convert.ToInt32(item["stretch5"]);
                        autotruck.IsUse = 1;
                        SelfDber.Update(autotruck);
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 向大唐的数据库同步车辆信息
        /// </summary>
        /// <param name="autotruck"></param>
        /// <returns></returns>
        public bool BossienInsertDTtb_vehicle_tare(CmcsAutotruck autotruck)
        {
            if (autotruck == null) return false;
            int stretchCount = 0;
            if (autotruck.LeftObstacle1 > 0)
                stretchCount++;
            if (autotruck.LeftObstacle2 > 0)
                stretchCount++;
            if (autotruck.LeftObstacle3 > 0)
                stretchCount++;
            if (autotruck.LeftObstacle4 > 0)
                stretchCount++;
            if (autotruck.LeftObstacle5 > 0)
                stretchCount++;
            if (autotruck.LeftObstacle6 > 0)
                stretchCount++;
            DTtb_vehicle_tare car = SqlServerDber.Entity<DTtb_vehicle_tare>(string.Format("where licenseplate='{0}'", autotruck.CarNumber));
            if (car == null)
            {
                car = new DTtb_vehicle_tare()
                {
                    licenseplate = autotruck.CarNumber,
                    driver = autotruck.Driver,
                    tare = 0,
                    taretime = DateTime.Now,
                    Operator = SelfVars.LoginUser.UserName,
                    descript = "博晟",
                    uploadStatus = 0,
                    vlength = autotruck.CarriageLength,
                    vwidth = autotruck.CarriageWidth,
                    vheight = autotruck.CarriageBottomToFloor,
                    stretchCount = stretchCount,
                    stretch1 = autotruck.LeftObstacle1,
                    stretch2 = autotruck.LeftObstacle2,
                    stretch3 = autotruck.LeftObstacle3,
                    stretch4 = autotruck.LeftObstacle4,
                    stretch5 = autotruck.LeftObstacle5
                };
                return SqlServerDber.Insert<DTtb_vehicle_tare>(car) > 0;
            }
            else
            {
                car.driver = autotruck.Driver;
                car.tare = 0;
                car.taretime = DateTime.Now;
                car.Operator = SelfVars.LoginUser.UserName;
                car.descript = "博晟";
                car.uploadStatus = 0;
                car.vlength = autotruck.CarriageLength;
                car.vwidth = autotruck.CarriageWidth;
                car.vheight = autotruck.CarriageBottomToFloor;
                car.stretchCount = stretchCount;
                car.stretch1 = autotruck.LeftObstacle1;
                car.stretch2 = autotruck.LeftObstacle2;
                car.stretch3 = autotruck.LeftObstacle3;
                car.stretch4 = autotruck.LeftObstacle4;
                car.stretch5 = autotruck.LeftObstacle5;
                return SqlServerDber.Update<DTtb_vehicle_tare>(car) > 0;
            }
        }

        /// <summary>
        ///向大唐数据库插入预制信息
        /// </summary>
        /// <param name="transport"></param>
        /// <returns></returns>
        public bool InsertDttb_record_preset_weigh(CmcsBuyFuelTransport transport)
        {
            CmcsAutotruck autotruck = SelfDber.Entity<CmcsAutotruck>("where CarNumber=:CarNumber", new { CarNumber = transport.CarNumber });
            CmcsAdvance advance = SelfDber.Entity<CmcsAdvance>("where TransportId=:TransportId", new { TransportId = transport.Id });
            CmcsMine mine = SelfDber.Entity<CmcsMine>("where Id=:Id", new { Id = transport.MineId });
            if (autotruck == null || advance == null || mine == null) return false;
            DataTable data = SqlServerDber.ExecuteDataTable(string.Format("select * from tb_record_preset_weigh where cardserial='{0}'", advance.Tag));
            string sql = string.Empty;
            autotruck.Driver = "博晟";
            if (data != null && data.Rows.Count > 0)
            {
                sql = string.Format(@"update tb_record_preset_weigh set licenseplate='{0}',driver='{1}',sendcorpCode='{2}',
                                           recvcorpCode='{3}',oretypeCode='{4}',carrierCode='{5}',presetWeight='{6}'
                                           where cardserial='{7}'", advance.CarNumber, autotruck.Driver, mine.NodeCode,
                                          "qy0001", "hw001", "", transport.TicketWeight, advance.Tag);
            }
            else
            {
                sql = string.Format(@"insert into tb_record_preset_weigh (cardserial,licenseplate,driver,sendcorpCode,recvcorpCode,
oretypeCode,productCode,carrierCode,direction,cardType,orderNo,presetWeight) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',
'{8}','{9}','{10}','{11}')", advance.Tag, advance.CarNumber, autotruck.Driver, mine.NodeCode, "qy0001", "hw001", "", "", "2", "2", "", transport.TicketWeight);
            }
            return SqlServerDber.Execute(sql) > 0;
        }

        /// <summary>
        /// 删除预制信息
        /// </summary>
        /// <param name="rfid"></param>
        /// <returns></returns>
        public bool DelDttb_record_preset_weigh(string rfid)
        {
            string sql = string.Format(@"delete tb_record_preset_weigh where cardserial='{0}'", rfid);
            return SqlServerDber.Execute(sql) > 0;
        }

        /// <summary>
        /// 同步矿点信息
        /// </summary>
        /// <returns></returns>
        public int InsertMine()
        {
            int count = 0;
            DataTable data = SqlServerDber.ExecuteDataTable("select * from tb_firm_info order by code desc ");
            if (data != null && data.Rows.Count > 0)
            {
                foreach (DataRow item in data.Rows)
                {
                    CmcsMine mine = SelfDber.Entity<CmcsMine>("where Name=:Name", new { Name = item["name"] });
                    if (mine == null)
                    {
                        mine = new CmcsMine()
                        {
                            Code = GetMineNewChildCode("00"),
                            Name = item["name"].ToString(),
                            NodeCode = item["code"].ToString(),
                            Valid = "有效",
                            ParentId = "-1",
                            DataFrom = GlobalVars.DataFrom
                        };
                        if (SelfDber.Insert(mine) > 0) count++;
                    }
                    else
                    {
                        mine.NodeCode = item["code"].ToString();
                        mine.DataFrom = GlobalVars.DataFrom;
                        if (SelfDber.Update(mine) > 0) count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 向大唐插入矿点信息
        /// </summary>
        /// <param name="mine"></param>
        /// <returns></returns>
        public bool BossienInsertDttb_firm_info(CmcsMine mine)
        {
            if (mine == null) return false;
            DTtb_firm_info dtmine = SqlServerDber.Entity<DTtb_firm_info>(string.Format("where code='{0}'", mine.NodeCode));
            if (dtmine == null)
            {
                dtmine = new DTtb_firm_info()
                {
                    code = mine.NodeCode,
                    name = mine.Name,
                    descript = "博晟"
                };
                return SqlServerDber.Insert<DTtb_firm_info>(dtmine) > 0;
            }
            else
            {
                dtmine.name = mine.Name;
                dtmine.descript = "博晟";
                return SqlServerDber.Update<DTtb_firm_info>(dtmine) > 0;
            }
        }

        /// <summary>
        /// 删除大唐矿点
        /// </summary>
        /// <param name="mine"></param>
        /// <returns></returns>
        public bool DelDttb_firm_info(CmcsMine mine)
        {
            DTtb_firm_info dtmine = SqlServerDber.Entity<DTtb_firm_info>(string.Format("where code='{0}'", mine.NodeCode));
            if (dtmine == null)
            {
                return SqlServerDber.Execute(string.Format("delete tb_firm_info where id={0}", dtmine.ID)) > 0;
                //return SqlServerDber.Delete<DTtb_firm_info>(dtmine.ID.ToString()) > 0;
            }
            return false;
        }

        /// <summary>
        /// 读取大唐的运输记录
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public int InsertBuyTransport(DateTime starttime, DateTime endtime)
        {
            int count = 0;
            IList<DTtb_record_weigh> transport = SqlServerDber.Entities<DTtb_record_weigh>(string.Format("where CONVERT(varchar(20),weightime,120)>='{0}' and CONVERT(varchar(20),weightime,120)<'{1}' and descript!='博晟程序过衡数据' and status='0'", starttime.ToString("yyyy-MM-dd HH:mm:ss"), endtime.ToString("yyyy-MM-dd HH:mm:ss")));
            if (transport != null && transport.Count > 0)
            {
                foreach (DTtb_record_weigh item in transport)
                {
                    CmcsBuyFuelTransport entity = SelfDber.Entity<CmcsBuyFuelTransport>("where CarNumber=:CarNumber and trunc(InFactoryTime)=:InFactoryTime order by InFactoryTime desc", new { CarNumber = item.licenseplate, InFactoryTime = item.weightime.ToString("yyyy-MM-dd") });
                    CmcsBuyFuelTransportDeduct entitydeduct = null;
                    if (entity == null)
                    {
                        entity = new CmcsBuyFuelTransport();
                        entity.CarNumber = item.licenseplate;
                        entity.TicketWeight = Convert.ToDecimal(item.shipWeigh / 1000);
                        entity.GrossWeight = Convert.ToDecimal(item.totalweight / 1000);
                        entity.TareWeight = Convert.ToDecimal(item.tare / 1000);
                        entity.SuttleWeight = (entity.GrossWeight - entity.TareWeight) > entity.TicketWeight ? entity.TicketWeight / 1000 : (entity.GrossWeight - entity.TareWeight) / 1000;
                        entity.DeductWeight = Convert.ToDecimal(item.buckle / 1000) + Convert.ToDecimal(item.buckleStone / 1000);
                        entity.KsWeight = Convert.ToDecimal(item.buckle / 1000);
                        entity.CheckWeight = item.neatweight / 1000;
                        entity.InFactoryTime = item.weightime;
                        entity.GrossTime = item.weightime;
                        entity.TareTime = item.oldWeighTime;
                        entity.Remark = "从大唐数据库同步";
                        entity.IsUse = 1;
                        entity.CreateDate = item.weightime;
                        entity.OperDate = item.weightime;
                        if (entity.DeductWeight > 0)
                        {
                            entitydeduct = new CmcsBuyFuelTransportDeduct();
                            entitydeduct.DeductType = "扣水";
                            entitydeduct.DeductWeight = entity.DeductWeight;
                            entitydeduct.TransportId = entity.Id;
                        }
                        if (SelfDber.Insert<CmcsBuyFuelTransport>(entity) > 0)
                        {
                            if (entitydeduct != null) SelfDber.Insert<CmcsBuyFuelTransportDeduct>(entitydeduct);
                            count++;
                        }
                    }
                    else
                    {
                        entity.TicketWeight = Convert.ToDecimal(item.shipWeigh / 1000);
                        entity.GrossWeight = Convert.ToDecimal(item.totalweight / 1000);
                        entity.TareWeight = Convert.ToDecimal(item.tare / 1000);
                        entity.SuttleWeight = (entity.GrossWeight - entity.TareWeight) > entity.TicketWeight ? entity.TicketWeight : (entity.GrossWeight - entity.TareWeight);
                        entity.DeductWeight = Convert.ToDecimal(item.buckle / 1000) + Convert.ToDecimal(item.buckleStone / 1000);
                        entity.KsWeight = Convert.ToDecimal(item.buckle / 1000);
                        entity.CheckWeight = item.neatweight;
                        entity.GrossTime = item.weightime;
                        entity.TareTime = item.oldWeighTime;
                        entity.Remark = "从大唐数据库同步";
                        entity.IsUse = 1;
                        if (entity.DeductWeight > 0)
                        {
                            entitydeduct = new CmcsBuyFuelTransportDeduct();
                            entitydeduct.DeductType = "扣水";
                            entitydeduct.DeductWeight = entity.DeductWeight;
                            entitydeduct.TransportId = entity.Id;
                        }
                        if (SelfDber.Update<CmcsBuyFuelTransport>(entity) > 0)
                        {
                            SelfDber.DeleteBySQL<CmcsBuyFuelTransportDeduct>("where TransportId=:TransportId", new { TransportId = entity.Id });
                            DelAdvance(entity.Id);
                            DelUnFinishTransport(entity.Id);
                            if (entitydeduct != null) SelfDber.Insert<CmcsBuyFuelTransportDeduct>(entitydeduct);
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 向大唐插入称重信息
        /// </summary>
        /// <param name="transport"></param>
        /// <returns></returns>
        public bool BossienInsertDttb_record_weigh(CmcsBuyFuelTransport transport)
        {
            //为保证过衡成功 插入数据失败也返回成功
            try
            {
                CmcsMine mine = commonDAO.SelfDber.Get<CmcsMine>(transport.MineId);
                if (transport == null || mine == null) return false;
                string deviceserial = string.Empty;
                //毛重记录
                DTtb_record_weigh dttransport = SqlServerDber.Entity<DTtb_record_weigh>(string.Format("where licenseplate='{0}' and CONVERT(varchar(10),weightime,120)='{1}' and status!='0' order by weightime desc", transport.CarNumber, transport.GrossTime.ToString("yyyy-MM-dd")));

                #region 毛重记录
                if (transport.GrossWeight > 0 && transport.TareWeight == 0)//插入毛重记录
                {
                    if (transport.GrossPlace.Contains("1"))
                        deviceserial = "d002";
                    else if (transport.GrossPlace.Contains("2"))
                        deviceserial = "d003";
                    else if (transport.GrossPlace.Contains("3"))
                        deviceserial = "d004";
                    else if (transport.GrossPlace.Contains("4"))
                        deviceserial = "d005";

                    if (dttransport == null)
                    {
                        dttransport = new DTtb_record_weigh()
                        {
                            weightID = GetWeightId(transport.GrossTime),
                            licenseplate = transport.CarNumber,
                            driver = "博晟",
                            sendcorpCode = mine.NodeCode,
                            recvcorpCode = "qy0001",
                            oretypeCode = "hw001",
                            productCode = "",
                            carrierCode = "",
                            orderNo = "",
                            direction = 2,
                            totalweight = Convert.ToDecimal(transport.GrossWeight * 1000),
                            tare = Convert.ToDecimal(transport.TareWeight * 1000),
                            buckle = Convert.ToDecimal(transport.DeductWeight * 1000),
                            neatweight = Convert.ToDecimal(transport.SuttleWeight * 1000),
                            shipWeigh = Convert.ToDecimal(transport.TicketWeight * 1000),
                            pl = Convert.ToDecimal(transport.ProfitAndLossWeight * 1000),
                            neatStr = "",
                            unitfee = 0,
                            salefee = 0,
                            salefeeStr = "",
                            Operator = SelfVars.LoginUser != null ? SelfVars.LoginUser.UserName : "",
                            weightime = transport.GrossTime,
                            printCount = 2,
                            deviceserial = deviceserial,
                            oldOperator = "",
                            oldDeviceserial = "",
                            oldWeightId = 0,
                            status = 1,
                            descript = "博晟程序过衡数据",
                            imgFilePath = "",
                            upLoadData = "0",
                            uploadStatus = "0"
                        };
                        return SqlServerDber.Insert<DTtb_record_weigh>(dttransport) > 0;
                    }
                    else
                    {
                        string sql = string.Format(@"update tb_record_weigh set sendcorpCode='{0}',totalweight='{1}',tare='{2}',buckle='{3}',neatweight='{4}',shipWeigh='{5}',pl='{6}',weightime='{7}',oldWeighTime='{8}',oldWeightId='{9}',status='0',descript = '博晟程序过衡数据' where weightID='{10}'", mine.NodeCode, transport.GrossWeight * 1000, transport.TareWeight * 1000, transport.DeductWeight * 1000, transport.SuttleWeight * 1000, transport.TicketWeight * 1000, transport.ProfitAndLossWeight * 1000, transport.TareTime, transport.GrossTime, dttransport != null ? dttransport.weightID : 0, dttransport.weightID);
                        return SqlServerDber.Execute(sql) > 0;
                    }
                }
                #endregion

                #region 皮重记录
                if (transport.TareWeight > 0)
                {
                    DTtb_record_weigh dttransporttare = SqlServerDber.Entity<DTtb_record_weigh>(string.Format("where licenseplate='{0}' and CONVERT(varchar(10),weightime,120)='{1}' and status='0' order by weightime desc", transport.CarNumber, transport.TareTime.ToString("yyyy-MM-dd")));
                    if (transport.TarePlace.Contains("1"))
                        deviceserial = "d002";
                    else if (transport.TarePlace.Contains("2"))
                        deviceserial = "d003";
                    else if (transport.TarePlace.Contains("3"))
                        deviceserial = "d004";
                    else if (transport.TarePlace.Contains("4"))
                        deviceserial = "d005";

                    if (dttransporttare == null)
                    {
                        dttransporttare = new DTtb_record_weigh();

                        dttransporttare.weightID = GetWeightId(transport.GrossTime);
                        dttransporttare.licenseplate = transport.CarNumber;
                        dttransporttare.driver = "博晟";
                        dttransporttare.sendcorpCode = mine.NodeCode;
                        dttransporttare.recvcorpCode = "qy0001";
                        dttransporttare.oretypeCode = "hw001";
                        dttransporttare.productCode = "";
                        dttransporttare.carrierCode = "";
                        dttransporttare.orderNo = "";
                        dttransporttare.direction = 2;
                        dttransporttare.totalweight = Convert.ToDecimal(transport.GrossWeight * 1000);
                        dttransporttare.tare = Convert.ToDecimal(transport.TareWeight * 1000);
                        dttransporttare.buckle = Convert.ToDecimal(transport.DeductWeight * 1000);
                        dttransporttare.neatweight = Convert.ToDecimal(transport.SuttleWeight * 1000);
                        dttransporttare.shipWeigh = Convert.ToDecimal(transport.TicketWeight * 1000);
                        dttransporttare.pl = Convert.ToDecimal(transport.ProfitAndLossWeight * 1000);
                        dttransporttare.neatStr = "";
                        dttransporttare.unitfee = 0;
                        dttransporttare.salefee = 0;
                        dttransporttare.salefeeStr = "";
                        dttransporttare.Operator = SelfVars.LoginUser != null ? SelfVars.LoginUser.UserName : "";
                        dttransporttare.weightime = transport.TareTime;
                        dttransporttare.printCount = 2;
                        dttransporttare.deviceserial = deviceserial;
                        dttransporttare.oldWeighTime = transport.GrossTime;
                        dttransporttare.oldOperator = transport.OperUser;
                        if (dttransport != null) dttransporttare.oldDeviceserial = dttransport.deviceserial;
                        if (dttransport != null) dttransporttare.oldWeightId = dttransport.weightID;
                        dttransporttare.status = 0;
                        dttransporttare.descript = "博晟程序过衡数据";
                        dttransporttare.imgFilePath = "";
                        dttransporttare.upLoadData = "0";
                        dttransporttare.uploadStatus = "0";

                        if (dttransport != null) dttransport.status = 2;
                        if (dttransport != null) SqlServerDber.Update<DTtb_record_weigh>(dttransport);
                        SqlServerDber.Insert(dttransporttare);
                        #region SQL 插入
                        //                    try
                        //                    {
                        //                        string sql = string.Format(@"insert into tb_record_weigh (weightID,
                        //                                                                                    licenseplate,
                        //                                                                                    driver,
                        //                                                                                    sendcorpCode,
                        //                                                                                    recvcorpCode,
                        //                                                                                    oretypeCode,
                        //                                                                                    productCode,
                        //                                                                                    carrierCode,
                        //                                                                                    orderNo,
                        //                                                                                    direction,
                        //                                                                                    totalweight,
                        //                                                                                    tare,
                        //                                                                                    buckle,
                        //                                                                                    buckleStone,
                        //                                                                                    neatweight,
                        //                                                                                    shipWeigh,
                        //                                                                                    pl,
                        //                                                                                    neatStr,
                        //                                                                                    unitfee,
                        //                                                                                    salefee,
                        //                                                                                    salefeeStr,
                        //                                                                                    Operator,
                        //                                                                                    weightime,
                        //                                                                                    printCount,
                        //                                                                                    deviceserial,
                        //                                                                                    oldWeighTime,
                        //                                                                                    oldOperator,
                        //                                                                                    oldDeviceserial,
                        //                                                                                    oldWeightId,
                        //                                                                                    status,
                        //                                                                                    descript,
                        //                                                                                    imgFilePath,
                        //                                                                                    upLoadData,
                        //                                                                                    uploadStatus)
                        //                                                                                    values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}',
                        //                                                                                    '{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}')",
                        //                                                                                 GetWeightId(transport.GrossTime),
                        //                                                                                 transport.CarNumber,
                        //                                                                                 "博晟",
                        //                                                                                 mine.NodeCode,
                        //                                                                                 "qy0001",
                        //                                                                                "hw001",
                        //                                                                                 "",
                        //                                                                                 "",
                        //                                                                                 "",
                        //                                                                                 2,
                        //                                                                                 transport.GrossWeight * 1000,
                        //                                                                                 transport.TareWeight * 1000,
                        //                                                                                 transport.KsWeight * 1000,
                        //                                                                                 transport.KgWeight * 1000,
                        //                                                                                 transport.SuttleWeight * 1000,
                        //                                                                                 transport.TicketWeight * 1000,
                        //                                                                                 transport.ProfitAndLossWeight * 1000,
                        //                                                                                  "",
                        //                                                                                  0,
                        //                                                                                  0,
                        //                                                                                  "",
                        //                                                                                   SelfVars.LoginUser != null ? SelfVars.LoginUser.UserName : "",
                        //                                                                                   transport.TareTime,
                        //                                                                                   2,
                        //                                                                                   deviceserial,
                        //                                                                                   dttransport != null ? dttransport.weightime : DateTime.Now,
                        //                                                                                   dttransport != null ? dttransport.Operator : "",
                        //                                                                                   dttransport != null ? dttransport.deviceserial : "",
                        //                                                                                   dttransport != null ? dttransport.deviceserial : "",
                        //                                                                                   dttransport != null ? dttransport.weightID : 0,
                        //                                                                                   0,
                        //                                                                                   "博晟程序过衡数据",
                        //                                                                                   "",
                        //                                                                                   "0",
                        //                                                                                   "0"
                        //                                                                                 );
                        //                        return SqlServerDber.Execute(sql) > 0;
                        //                    }
                        //                    catch (Exception)
                        //                    {
                        //                    }
                        #endregion
                    }
                    else
                    {
                        #region Dapper 框架问题 实体更新时插入了数据 导致没有设置主键的数据重复出现
                        //dttransporttare.driver = "博晟";
                        //dttransporttare.sendcorpCode = mine.NodeCode;
                        //dttransporttare.recvcorpCode = "qy0001";
                        //dttransporttare.oretypeCode = "hw001";
                        //dttransporttare.productCode = "";
                        //dttransporttare.carrierCode = "";
                        //dttransporttare.orderNo = "";
                        //dttransporttare.direction = 2;
                        //dttransporttare.totalweight = Convert.ToDecimal(transport.GrossWeight * 1000);
                        //dttransporttare.tare = Convert.ToDecimal(transport.TareWeight * 1000);
                        //dttransporttare.buckle = Convert.ToDecimal(transport.DeductWeight * 1000);
                        //dttransporttare.neatweight = Convert.ToDecimal(transport.SuttleWeight * 1000);
                        //dttransporttare.shipWeigh = Convert.ToDecimal(transport.TicketWeight * 1000);
                        //dttransporttare.pl = Convert.ToDecimal(transport.ProfitAndLossWeight * 1000);
                        //dttransporttare.neatStr = "";
                        //dttransporttare.unitfee = 0;
                        //dttransporttare.salefee = 0;
                        //dttransporttare.salefeeStr = "";
                        //dttransporttare.Operator = SelfVars.LoginUser != null ? SelfVars.LoginUser.UserName : "";
                        //dttransporttare.weightime = transport.TareTime;
                        //dttransporttare.printCount = 2;
                        //dttransporttare.deviceserial = deviceserial;
                        //dttransporttare.oldWeighTime = transport.GrossTime;
                        //dttransporttare.oldOperator = transport.OperUser;
                        //if (dttransport != null) dttransporttare.oldDeviceserial = dttransport.deviceserial;
                        //if (dttransport != null) dttransporttare.oldWeightId = dttransport.weightID;
                        //dttransporttare.status = 0;
                        //dttransporttare.descript = "博晟程序过衡数据";
                        //dttransporttare.imgFilePath = "";
                        //dttransporttare.upLoadData = "0";
                        //dttransporttare.uploadStatus = "0";

                        //if (dttransport != null) dttransport.status = 2;
                        //if (dttransport != null) SqlServerDber.Update<DTtb_record_weigh>(dttransport);
                        //return SqlServerDber.Update<DTtb_record_weigh>(dttransporttare) > 0;

                        #endregion

                        string sql = string.Format(@"update tb_record_weigh set sendcorpCode='{0}',totalweight='{1}',tare='{2}',buckle='{3}',neatweight='{4}',shipWeigh='{5}',pl='{6}',weightime='{7}',oldWeighTime='{8}',oldWeightId='{9}',status='0',descript = '博晟程序过衡数据' where weightID='{10}'", mine.NodeCode, transport.GrossWeight * 1000, transport.TareWeight * 1000, transport.DeductWeight * 1000, transport.SuttleWeight * 1000, transport.TicketWeight * 1000, transport.ProfitAndLossWeight * 1000, transport.TareTime, transport.GrossTime, dttransport != null ? dttransport.weightID : 0, dttransporttare.weightID);
                        return SqlServerDber.Execute(sql) > 0;
                    }
                }
                #endregion
            }
            catch (Exception)
            {
            }
            return true;
        }

        /// <summary>
        /// 获取称重记录id
        /// </summary>
        /// <param name="grosstime"></param>
        /// <returns></returns>
        public int GetWeightId(DateTime grosstime)
        {
            int weightid = 0;
            DTtb_record_weigh dttransport = SqlServerDber.Entity<DTtb_record_weigh>(string.Format("where weightime>= '{0}' order by weightID desc ", grosstime.ToString("yyyy-MM-dd")));
            if (dttransport != null)
            {
                weightid = (Convert.ToInt32(dttransport.weightID) + 1);
            }
            return weightid;
        }
        /// <summary>
        /// 修复大唐的数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int RepaireDT(DateTime starttime, DateTime endtime)
        {
            int count = 0;
            IList<DTtb_record_weigh> transport = SqlServerDber.Entities<DTtb_record_weigh>(string.Format("where CONVERT(varchar(20),oldweightime,120)>='{0}' and CONVERT(varchar(20),oldweightime,120)<'{1}'  and shipWeigh<'100' ", starttime.ToString("yyyy-MM-dd HH:mm:ss"), endtime.ToString("yyyy-MM-dd HH:mm:ss")));
            if (transport != null && transport.Count > 0)
            {
                foreach (DTtb_record_weigh item in transport)
                {
                    CmcsBuyFuelTransport entity = SelfDber.Entity<CmcsBuyFuelTransport>("where CarNumber=:CarNumber and trunc(InFactoryTime)=:InFactoryTime order by InFactoryTime desc", new { CarNumber = item.licenseplate, InFactoryTime = item.weightime.ToString("yyyy-MM-dd") });
                    if (entity != null)
                    {

                        if (SqlServerDber.Execute(string.Format(@"update tb_record_weigh set tare='{0}',neatweight='{1}',pl='{2}',totalweight='{3}',shipWeigh='{4}' where weightID='{5}'", entity.TareWeight * 1000, entity.CheckWeight * 1000, entity.ProfitAndLossWeight * 1000, entity.GrossWeight * 1000, entity.TicketWeight * 1000, item.weightID)) > 0)
                            count++;
                    }
                }
            }
            return count;
        }

        public int RepaireDT2(DateTime starttime, DateTime endtime)
        {
            int count = 0;
            IList<CmcsBuyFuelTransport> list = SelfDber.Entities<CmcsBuyFuelTransport>("where trunc(TareTime)=:StartTime and GrossWeight>0 and TareWeight>0", new { StartTime = starttime.ToString("yyyy-MM-dd") });
            foreach (CmcsBuyFuelTransport item in list)
            {
                if (BossienInsertDttb_record_weigh(item))
                    count++;
            }
            return count;
        }

        #endregion
    }
}