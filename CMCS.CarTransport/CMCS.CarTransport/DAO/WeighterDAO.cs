using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.DAO;
using CMCS.DapperDber.Dbs.OracleDb;
using CMCS.Common;
using CMCS.Common.Entities;
using CMCS.Common.Views;
using CMCS.DapperDber.Util;
using CMCS.Common.Enums;
using CMCS.Common.Utilities;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.Fuel;

namespace CMCS.CarTransport.DAO
{
    /// <summary>
    /// 汽车过衡业务
    /// </summary>
    public class WeighterDAO
    {
        private static WeighterDAO instance;

        public static WeighterDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new WeighterDAO();
            }

            return instance;
        }

        private WeighterDAO()
        { }

        public OracleDapperDber SelfDber
        {
            get { return Dbers.GetInstance().SelfDber; }
        }

        CommonDAO commonDAO = CommonDAO.GetInstance();
        CarTransportDAO carTransportDAO = CarTransportDAO.GetInstance();

        #region 入厂煤业务

        /// <summary>
        /// 获取指定日期已完成的入厂煤运输记录
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public List<View_BuyFuelTransport> GetFinishedBuyFuelTransport(DateTime dtStart, DateTime dtEnd)
        {
            return SelfDber.Entities<View_BuyFuelTransport>("where SuttleWeight!=0 and InFactoryTime>=:dtStart and InFactoryTime<:dtEnd order by TareTime desc", new { dtStart = dtStart, dtEnd = dtEnd });
        }

        /// <summary>
        /// 获取未完成的入厂煤运输记录
        /// </summary>
        /// <returns></returns>
        public List<View_BuyFuelTransport> GetUnFinishBuyFuelTransport()
        {
            return SelfDber.Entities<View_BuyFuelTransport>("where SuttleWeight=0 and IsUse=1 and UnFinishTransportId is not null order by grosstime desc,InFactoryTime desc");
        }

        /// <summary>
        /// 保存入厂煤运输记录
        /// </summary>
        /// <param name="transportId"></param>
        /// <param name="weight">重量</param>
        /// <param name="place"></param>
        /// <returns></returns>
        public bool SaveBuyFuelTransport(string transportId, decimal weight, DateTime dt, string place)
        {
            CmcsBuyFuelTransport transport = SelfDber.Get<CmcsBuyFuelTransport>(transportId);
            if (transport == null) return false;
            CmcsAdvance advance = SelfDber.Entity<CmcsAdvance>("where TransportId=:TransportId", new { TransportId = transport.Id });
            if (advance == null) return false;
            if (transport.GrossWeight == 0)
            {
                transport.StepName = eTruckInFactoryStep.重车.ToString();
                transport.GrossWeight = weight;
                transport.GrossPlace = place;
                transport.GrossTime = dt;
                //if (transport.InFactoryTime.Day < dt.Day) transport.InFactoryTime = dt;
                transport.FlowInfo += string.Format("{0} - {1}({2}){3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm"), "重车计量", CommonAppConfig.GetInstance().AppIdentifier, Environment.NewLine);
                SelfDber.Update(transport);

                try
                {
                    commonDAO.CreateSampleNums(transport.InFactoryBatchId, transport.CarNumber);
                }
                catch (Exception ex)
                {
                    Log4Neter.Error("采样", ex);
                }
            }
            else if (transport.TareWeight == 0)
            {
                if (transport.GrossWeight - weight < 10) return false;
                transport.StepName = eTruckInFactoryStep.轻车.ToString();
                transport.TareWeight = weight;
                transport.TarePlace = place;
                transport.TareTime = dt;
                transport.OutFactoryTime = dt;
                transport.SuttleWeight = transport.GrossWeight - transport.TareWeight;
                ////验收量大于票重时多余的量算到扣吨
                decimal deduct = transport.SuttleWeight > transport.TicketWeight ? (transport.SuttleWeight - transport.TicketWeight) : 0;
                decimal letterdeduct = 0;//抹去的小数位
                //transport.SuttleWeight -= deduct;
                transport.CheckWeight = OneDigit(transport.SuttleWeight - deduct - transport.KsWeight - transport.KgWeight, ref letterdeduct);
                deduct += letterdeduct;

                transport.ProfitAndLossWeight = transport.CheckWeight - transport.TicketWeight;
                transport.FlowInfo += string.Format("{0} - {1}({2}){3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm"), "轻皮计量", CommonAppConfig.GetInstance().AppIdentifier, Environment.NewLine);
                transport.AutoKsWeight = deduct;
                transport.DeductWeight = transport.AutoKsWeight + transport.KsWeight + transport.KgWeight;
                // 生成批次以及采制化三级编码数据 
                CmcsInFactoryBatch inFactoryBatch = carTransportDAO.GCQCInFactoryBatchByBuyFuelTransport(transport);
                if (inFactoryBatch != null) transport.InFactoryBatchId = inFactoryBatch.Id;
                // 回皮即完结
                transport.IsFinish = 1;
                if (SelfDber.Update(transport) > 0)
                {
                    carTransportDAO.DelDttb_record_preset_weigh(advance.Tag);
                    carTransportDAO.DelAdvance(transport.Id);
                    carTransportDAO.DelUnFinishTransport(transport.Id);
                    carTransportDAO.InsertAutoPrint(transport.Id);
                    this.commonDAO.InsertWaitForHandleEvent("汽车智能化_同步入厂煤运输记录到批次", transport.Id);
                }
            }
            else
                return false;
            //return carTransportDAO.BossienInsertDttb_record_weigh(transport);//不再向大唐数据库插入运输记录
            return true;
        }

        /// <summary>
        /// 保存重量
        /// </summary>
        /// <param name="transport"></param>    
        /// <returns></returns>
        public bool SaveBuyFuelTransport(CmcsBuyFuelTransport transport)
        {
            if (transport == null) return false;
            CmcsAdvance advance = SelfDber.Entity<CmcsAdvance>("where TransportId=:TransportId", new { TransportId = transport.Id });
            if (transport.TareWeight > 0 && transport.GrossWeight > 0)
            {
                if (transport.GrossTime.Year < 2000) { transport.GrossTime = DateTime.Now; transport.InFactoryTime = DateTime.Now; }
                if (transport.TareTime.Year < 2000) transport.TareTime = DateTime.Now;
                if (transport.OutFactoryTime.Year < 2000) transport.OutFactoryTime = DateTime.Now;

                transport.SuttleWeight = transport.GrossWeight - transport.TareWeight;
                ////验收量大于票重时多余的量算到扣吨
                decimal deduct = transport.SuttleWeight > transport.TicketWeight ? (transport.SuttleWeight - transport.TicketWeight) : 0;
                decimal letterdeduct = 0;//抹去的小数位
                //transport.SuttleWeight -= deduct;
                transport.CheckWeight = OneDigit(transport.SuttleWeight - deduct - transport.KsWeight - transport.KgWeight, ref letterdeduct);
                deduct += letterdeduct;

                transport.ProfitAndLossWeight = transport.CheckWeight - transport.TicketWeight;
                transport.AutoKsWeight = deduct;
                transport.DeductWeight = transport.AutoKsWeight + transport.KsWeight + transport.KgWeight;

                // 回皮即完结
                transport.IsFinish = 1;
                if (SelfDber.Update(transport) > 0)
                {
                    if (advance != null) carTransportDAO.DelDttb_record_preset_weigh(advance.Tag);
                    carTransportDAO.DelAdvance(transport.Id);
                    carTransportDAO.DelUnFinishTransport(transport.Id);
                    this.commonDAO.InsertWaitForHandleEvent("汽车智能化_同步入厂煤运输记录到批次", transport.Id);
                    return true;
                }
            }
            return SelfDber.Update(transport) > 0;
        }

        /// <summary>
        /// 舍去第二位小数，无论大小
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        decimal OneDigit(decimal value, ref decimal deductvalue)
        {

            decimal result = Math.Floor(value * 10) / 10m;
            deductvalue = value - result;
            return result;
        }

        /// <summary>
        /// 验证当前卡号是否在其他过衡端使用 true 正在被使用 false 没有被使用
        /// </summary>
        /// <param name="rfid"></param>
        /// <returns></returns>
        public bool CheckRfid(string rfid)
        {
            IList<CmcsSignalData> list = SelfDber.Entities<CmcsSignalData>("where SignalName='当前卡号' and SignalPrefix!=:SignalPrefix", new { SignalPrefix = CommonAppConfig.GetInstance().AppIdentifier });
            if (list != null)
            {
                if (list.Select(a => a.SignalValue).Contains(rfid))
                    return true;
            }
            return false;
        }

        #endregion

        #region 其他物资业务

        /// <summary>
        /// 保存其他物资运输记录
        /// </summary>
        /// <param name="transportId"></param>
        /// <param name="weight">重量</param>
        /// <param name="place"></param>
        /// <returns></returns>
        public bool SaveGoodsTransport(string transportId, decimal weight, DateTime dt, string place)
        {
            CmcsGoodsTransport transport = SelfDber.Get<CmcsGoodsTransport>(transportId);
            if (transport == null) return false;

            if (transport.FirstWeight == 0)
            {
                transport.StepName = eTruckInFactoryStep.重车.ToString();
                transport.FirstWeight = weight;
                transport.FirstPlace = place;
                transport.FirstTime = dt;
                SelfDber.Update(transport);
            }
            else if (transport.SecondWeight == 0)
            {
                transport.StepName = eTruckInFactoryStep.轻车.ToString();
                transport.SecondWeight = weight;
                transport.SecondPlace = place;
                transport.SecondTime = dt;
                transport.SuttleWeight = Math.Abs(transport.FirstWeight - transport.SecondWeight);

                // 回皮即完结
                transport.IsFinish = 1;
                if (SelfDber.Update(transport) > 0)
                {
                    carTransportDAO.DelAdvance(transport.Id);
                    carTransportDAO.DelUnFinishTransport(transport.Id);
                    carTransportDAO.InsertAutoPrint(transport.Id);
                    //this.commonDAO.InsertWaitForHandleEvent("汽车智能化_同步入厂煤运输记录到批次", transport.Id);
                }
            }
            else
                return false;

            return true;
        }


        #endregion

        #region 自动打印

        /// <summary>
        /// 插入自动打印
        /// </summary>
        /// <param name="buyfueltransport"></param>
        /// <param name="goodstransport"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool InsertPrint(CmcsBuyFuelTransport buyfueltransport, CmcsGoodsTransport goodstransport, eCarType type)
        {
            bool res = false;
            switch (type)
            {
                case eCarType.入厂煤:
                    res = this.commonDAO.SelfDber.Insert(new TransportPrint()
                    {
                        TransportId = buyfueltransport.Id,
                        IsPrint = (int)ePrintState.NoPrint,
                        CarType = type.ToString()
                    }) > 0;
                    break;
                case eCarType.其他物资:
                    res = this.commonDAO.SelfDber.Update(new TransportPrint()
                    {
                        TransportId = goodstransport.Id,
                        IsPrint = (int)ePrintState.NoPrint,
                        CarType = type.ToString()
                    }) > 0;
                    break;
                default:
                    break;
            }
            return res;
        }


        /// <summary>
        /// 更新运输记录自动打印表
        /// </summary>
        /// <param name="buyfueltransport"></param>
        /// <param name="goodstransport"></param>
        /// <param name="type">运输记录类型</param>
        /// <param name="print">打印状态</param>
        /// <returns></returns>
        public bool UpdatePrint(CmcsBuyFuelTransport buyfueltransport, CmcsGoodsTransport goodstransport, eCarType type, ePrintState print)
        {
            bool res = false;
            TransportPrint transport = null;
            switch (type)
            {
                case eCarType.入厂煤:
                    transport = this.commonDAO.SelfDber.Entity<TransportPrint>("where TransportId=:TransportId", new { TransportId = buyfueltransport.Id });
                    if (transport != null)
                    {
                        transport.IsPrint = (int)print;
                        res = commonDAO.SelfDber.Update(transport) > 0;
                    }
                    break;
                case eCarType.其他物资:
                    transport = this.commonDAO.SelfDber.Entity<TransportPrint>("where TransportId=:TransportId", new { TransportId = goodstransport.Id });
                    if (transport != null)
                    {
                        transport.IsPrint = (int)print;
                        res = this.commonDAO.SelfDber.Update(transport) > 0;
                    }
                    break;
                default:
                    break;
            }
            return res;
        }
        #endregion
    }
}
