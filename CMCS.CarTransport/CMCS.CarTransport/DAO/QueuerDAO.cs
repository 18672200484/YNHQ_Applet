using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.DAO;
using CMCS.Common.Entities.CarTransport;
using CMCS.DapperDber.Dbs.OracleDb;
using CMCS.Common;
using CMCS.Common.Entities;
using CMCS.Common.Views;
using CMCS.DapperDber.Util;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.Fuel;
using CMCS.Common.Enums;
using CMCS.DapperDber.Dbs.SqlServerDb;
using System.Data;

namespace CMCS.CarTransport.DAO
{
    /// <summary>
    /// 汽车入厂排队业务
    /// </summary>
    public class QueuerDAO
    {
        private static QueuerDAO instance;

        public static QueuerDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new QueuerDAO();
            }

            return instance;
        }

        private QueuerDAO()
        { }

        public OracleDapperDber SelfDber
        {
            get { return Dbers.GetInstance().SelfDber; }
        }

        CommonDAO commonDAO = CommonDAO.GetInstance();
        CarTransportDAO carTransportDAO = CarTransportDAO.GetInstance();

        #region 入厂煤业务

        /// <summary>
        /// 生成入厂煤运输排队记录，同时生成批次信息以及采制化三级编码
        /// </summary>
        /// <param name="autotruck">车</param>
        /// <param name="supplier">供煤单位</param>
        /// <param name="mine">矿点</param>
        /// <param name="fuelKind">煤种</param>
        /// <param name="ticketWeight">矿发量</param>
        /// <param name="qch">指定地磅</param>
        /// <param name="TagValue">标签卡</param>
        /// <param name="unloadarea">卸煤区域</param>
        /// <param name="inFactoryTime">入厂时间</param>
        /// <param name="remark">备注</param>
        /// <param name="place">地点</param>
        /// <returns></returns>
        public bool JoinQueueBuyFuelTransport(CmcsAutotruck autotruck, CmcsSupplier supplier, CmcsMine mine, CmcsFuelKind fuelKind, decimal ticketWeight, string qch, string TagValue, string unloadarea, DateTime inFactoryTime, string remark, string place, string unloadtype, ref CmcsBuyFuelTransport transport)
        {
            transport = new CmcsBuyFuelTransport
           {
               SerialNumber = carTransportDAO.CreateNewTransportSerialNumber(eCarType.入厂煤, inFactoryTime),
               AutotruckId = autotruck.Id,
               CarNumber = autotruck.CarNumber,
               TicketWeight = ticketWeight,
               InFactoryTime = inFactoryTime,
               IsFinish = 0,
               IsUse = 1,
               StepName = eTruckInFactoryStep.入厂.ToString(),
               Remark = remark,
               Qch = qch,
               UnloadArea = unloadarea,
               UnloadType = unloadtype,
               EpcCard = TagValue,
               IsHidden = 0,
               FlowInfo = string.Format("{0} - {1}({2}){3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm"), "入厂登记", CommonAppConfig.GetInstance().AppIdentifier, Environment.NewLine)
           };
            if (supplier != null)
            {
                transport.SupplierId = supplier.Id;
                transport.SupplierName = supplier.Name;
            }
            if (fuelKind != null)
            {
                transport.FuelKindId = fuelKind.Id;
                transport.FuelKindName = fuelKind.FuelName;
            }
            if (mine != null)
            {
                transport.MineId = mine.Id;
                transport.MineName = mine.Name;
            }
            else
            {

            }

            // 生成批次以及采制化三级编码数据 
            CmcsInFactoryBatch inFactoryBatch = carTransportDAO.GCQCInFactoryBatchByBuyFuelTransport(transport);
            if (inFactoryBatch != null)
            {
                transport.InFactoryBatchId = inFactoryBatch.Id;

                if (SelfDber.Insert(transport) > 0)
                {
                    // 插入未完成运输记录及预制信息
                    return SelfDber.Insert(new CmcsUnFinishTransport
                    {
                        TransportId = transport.Id,
                        CarType = eCarType.入厂煤.ToString(),
                        AutotruckId = autotruck.Id,
                        PrevPlace = place
                    }) > 0 && SelfDber.Insert(new CmcsAdvance()
                    {
                        Tag = TagValue,
                        TransportId = transport.Id,
                        CarNumber = transport.CarNumber,
                        CarType = eCarType.入厂煤.ToString(),
                        IsFinish = "0"
                    }) > 0;
                }
            }

            return false;
        }

        /// <summary>
        /// 更改入厂煤运输记录的无效状态
        /// </summary>
        /// <param name="buyFuelTransportId"></param>
        /// <param name="isValid">是否有效</param>
        /// <returns></returns>
        public bool ChangeBuyFuelTransportToInvalid(string buyFuelTransportId, bool isValid)
        {
            if (isValid)
            {
                // 设置为有效
                CmcsBuyFuelTransport buyFuelTransport = SelfDber.Get<CmcsBuyFuelTransport>(buyFuelTransportId);
                if (buyFuelTransport != null)
                {
                    if (SelfDber.Execute("update " + EntityReflectionUtil.GetTableName<CmcsBuyFuelTransport>() + " set IsUse=1 where Id=:Id", new { Id = buyFuelTransportId }) > 0)
                    {
                        if (buyFuelTransport.IsFinish == 0)
                        {
                            SelfDber.Insert(new CmcsUnFinishTransport
                            {
                                AutotruckId = buyFuelTransport.AutotruckId,
                                CarType = eCarType.入厂煤.ToString(),
                                TransportId = buyFuelTransport.Id,
                                PrevPlace = "未知"
                            });
                        }

                        return true;
                    }
                }
            }
            else
            {
                // 设置为无效

                if (SelfDber.Execute("update " + EntityReflectionUtil.GetTableName<CmcsBuyFuelTransport>() + " set IsUse=0 where Id=:Id", new { Id = buyFuelTransportId }) > 0)
                {
                    SelfDber.DeleteBySQL<CmcsUnFinishTransport>("where TransportId=:TransportId", new { TransportId = buyFuelTransportId });

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 更改入厂煤运输记录的隐藏状态
        /// </summary>
        /// <param name="buyFuelTransportId"></param>
        /// <param name="isHidden"></param>
        /// <returns></returns>
        public bool ChangeBuyFuelTransportHidden(string buyFuelTransportId, bool isHidden)
        {
            CmcsBuyFuelTransport buyFuelTransport = SelfDber.Get<CmcsBuyFuelTransport>(buyFuelTransportId);
            if (buyFuelTransport != null)
            {
                if (isHidden)
                    buyFuelTransport.IsHidden = 1;
                else
                    buyFuelTransport.IsHidden = 0;
                return commonDAO.SelfDber.Update(buyFuelTransport) > 0;
            }
            return false;
        }

        /// <summary>
        /// 根据车牌号获取指定到达日期的入厂煤来煤预报
        /// </summary>
        /// <param name="carNumber">车牌号</param>
        /// <param name="inFactoryTime">预计到达日期</param>
        /// <returns></returns>
        public List<CmcsLMYB> GetBuyFuelForecastByCarNumber(string carNumber, DateTime inFactoryTime)
        {
            return SelfDber.Query<CmcsLMYB>("select l.* from " + EntityReflectionUtil.GetTableName<CmcsLMYBDetail>() + " ld left join " + EntityReflectionUtil.GetTableName<CmcsLMYB>() + " l on l.Id=ld.lmybid where ld.CarNumber=:CarNumber and to_char(InFactoryTime,'yyyymmdd')=to_char(:InFactoryTime,'yyyymmdd') order by l.InFactoryTime desc",
                new { CarNumber = carNumber.Trim(), InFactoryTime = inFactoryTime }).ToList();
        }

        #endregion

        #region 其他物资业务

        /// <summary>
        /// 生成其他物资运输排队记录
        /// </summary>
        /// <param name="autotruck">车辆</param>
        /// <param name="supply">供货单位</param>
        /// <param name="receive">收货单位</param>
        /// <param name="goodsType">物资类型</param>
        /// <param name="inFactoryTime">入厂时间</param>
        /// <param name="remark">备注</param>
        /// <param name="place">地点</param>
        /// <returns></returns>
        public bool JoinQueueGoodsTransport(CmcsAutotruck autotruck, CmcsSupplyReceive supply, CmcsSupplyReceive receive, CmcsGoodsType goodsType, DateTime inFactoryTime, string remark, string place, string TagValue, string qch)
        {
            CmcsGoodsTransport transport = new CmcsGoodsTransport
            {
                SerialNumber = carTransportDAO.CreateNewTransportSerialNumber(eCarType.其他物资, inFactoryTime),
                AutotruckId = autotruck.Id,
                CarNumber = autotruck.CarNumber,
                SupplyUnitId = supply.Id,
                SupplyUnitName = supply.UnitName,
                ReceiveUnitId = receive.Id,
                ReceiveUnitName = receive.UnitName,
                GoodsTypeId = goodsType.Id,
                GoodsTypeName = goodsType.GoodsName,
                InFactoryTime = inFactoryTime,
                IsFinish = 0,
                IsUse = 1,
                StepName = eTruckInFactoryStep.入厂.ToString(),
                Remark = remark,
                Qch = qch
            };

            if (SelfDber.Insert(transport) > 0)
            {
                // 插入未完成运输记录及预制信息
                return SelfDber.Insert(new CmcsUnFinishTransport
                {
                    TransportId = transport.Id,
                    CarType = eCarType.其他物资.ToString(),
                    AutotruckId = autotruck.Id,
                    PrevPlace = place,
                }) > 0 && SelfDber.Insert(new CmcsAdvance()
                {
                    Tag = TagValue,
                    TransportId = transport.Id,
                    CarNumber = transport.CarNumber,
                    CarType = eCarType.其他物资.ToString()
                }) > 0;
            }

            return false;
        }

        /// <summary>
        /// 更改其他物资运输记录的无效状态
        /// </summary>
        /// <param name="transportId"></param>
        /// <param name="isValid">是否有效</param>
        /// <returns></returns>
        public bool ChangeGoodsTransportToInvalid(string transportId, bool isValid)
        {
            if (isValid)
            {
                // 设置为有效
                CmcsGoodsTransport buyFuelTransport = SelfDber.Get<CmcsGoodsTransport>(transportId);
                if (buyFuelTransport != null)
                {
                    if (SelfDber.Execute("update " + EntityReflectionUtil.GetTableName<CmcsGoodsTransport>() + " set IsUse=1 where Id=:Id", new { Id = transportId }) > 0)
                    {
                        if (buyFuelTransport.IsFinish == 0)
                        {
                            SelfDber.Insert(new CmcsUnFinishTransport
                            {
                                AutotruckId = buyFuelTransport.AutotruckId,
                                CarType = eCarType.其他物资.ToString(),
                                TransportId = buyFuelTransport.Id,
                                PrevPlace = "未知"
                            });
                        }

                        return true;
                    }
                }
            }
            else
            {
                // 设置为无效

                if (SelfDber.Execute("update " + EntityReflectionUtil.GetTableName<CmcsGoodsTransport>() + " set IsUse=0 where Id=:Id", new { Id = transportId }) > 0)
                {
                    SelfDber.DeleteBySQL<CmcsUnFinishTransport>("where TransportId=:TransportId", new { TransportId = transportId });

                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
