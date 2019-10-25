using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common;
using CMCS.Common.Entities.Sys;
using CMCS.DumblyConcealer.Tasks.CarSynchronous.Enums;
using CMCS.DapperDber.Dbs.OracleDb;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.Fuel;
using CMCS.DumblyConcealer.Enums;
using CMCS.Common.DAO;

namespace CMCS.DumblyConcealer.Tasks.CarSynchronous
{
    /// <summary>
    /// 综合事件处理
    /// </summary>
    public class DataHandlerDAO
    {
        private static DataHandlerDAO instance;

        public static DataHandlerDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new DataHandlerDAO();
            }
            return instance;
        }

        CommonDAO commonDAO = CommonDAO.GetInstance();

        private DataHandlerDAO()
        { }

        /// <summary>
        /// 开始处理
        /// </summary> 
        /// <returns></returns>
        public void Start(Action<string, eOutputType> output)
        {
            foreach (CmcsWaitForHandleEvent item in commonDAO.SelfDber.Entities<CmcsWaitForHandleEvent>("where DataFlag=0"))
            {
                bool isSuccess = false;

                eEventCode eventCode;
                bool a = Enum.TryParse<eEventCode>(item.EventCode, out eventCode);
                if (!Enum.TryParse<eEventCode>(item.EventCode, out eventCode)) continue;

                switch (eventCode)
                {
                    case eEventCode.汽车智能化_同步入厂煤运输记录到批次:

                        if (SyncToBatch(output, item.ObjectId))
                        {
                            isSuccess = true;

                            output(string.Format("事件：{0}  ObjectId：{1}", eEventCode.汽车智能化_同步入厂煤运输记录到批次.ToString(), item.ObjectId), eOutputType.Normal);
                        }

                        break;
                }

                if (isSuccess)
                {
                    item.DataFlag = 1;
                    commonDAO.SelfDber.Update(item);
                }
            }
        }

        /// <summary>
        /// 将汽车入厂煤运输记录同步到批次明细中
        /// </summary>
        /// <param name="transportId">汽车入厂煤运输记录Id</param>
        /// <returns></returns>
        private bool SyncToBatch(Action<string, eOutputType> output, string transportId)
        {
            bool res = false;
            CmcsBuyFuelTransport transport = commonDAO.SelfDber.Get<CmcsBuyFuelTransport>(transportId);
            if (transport == null) return true;

            CmcsInFactoryBatch batch = commonDAO.SelfDber.Get<CmcsInFactoryBatch>(transport.InFactoryBatchId);
            if (batch == null) return true;

            CmcsTransport truck = commonDAO.SelfDber.Entity<CmcsTransport>("where InFactoryBatchId=:InFactoryBatchId and PKID=:PKID", new { InFactoryBatchId = batch.Id, PKID = transport.Id });
            if (truck != null)
            {
                truck.TransportNo = transport.CarNumber;
                truck.OperDate = transport.OperDate;
                truck.TransportStyle = "汽车";
                truck.ArriveTime = transport.CreateDate;
                truck.GrossTime = transport.GrossTime;
                truck.SkinTime = transport.TareTime;
                truck.UnloadTime = transport.UploadTime;
                truck.LeaveTime = transport.OutFactoryTime;
                truck.TicketWeight = transport.TicketWeight;
                truck.GrossWeight = transport.GrossWeight;
                truck.SkinWeight = transport.TareWeight;
                truck.StandardWeight = transport.SuttleWeight;
                truck.KdWeight = transport.DeductWeight;
                truck.CheckQty = transport.SuttleWeight - transport.DeductWeight;
                truck.MarginWeight = transport.SuttleWeight - transport.DeductWeight - transport.TicketWeight;
                truck.InFactoryBatchId = transport.InFactoryBatchId;
                truck.PKID = transport.Id;
                truck.MesureMan = "汽车智能化";
                res = commonDAO.SelfDber.Update(truck) > 0;
            }
            else
            {
                truck = new CmcsTransport()
                {
                    TransportNo = transport.CarNumber,
                    OperDate = transport.OperDate,
                    TransportStyle = "汽车",
                    ArriveTime = transport.CreateDate,
                    GrossTime = transport.GrossTime,
                    SkinTime = transport.TareTime,
                    UnloadTime = transport.UploadTime,
                    LeaveTime = transport.OutFactoryTime,
                    TicketWeight = transport.TicketWeight,
                    GrossWeight = transport.GrossWeight,
                    SkinWeight = transport.TareWeight,
                    StandardWeight = transport.SuttleWeight,
                    KdWeight = transport.DeductWeight,
                    CheckQty = transport.SuttleWeight - transport.DeductWeight,
                    MarginWeight = transport.SuttleWeight - transport.DeductWeight - transport.TicketWeight,
                    InFactoryBatchId = transport.InFactoryBatchId,
                    PKID = transport.Id,
                    MesureMan = "汽车智能化"
                };

                res = commonDAO.SelfDber.Insert(truck) > 0;
            }

            if (res)
            {
                // 更新批次的量 

                List<CmcsTransport> trucks = commonDAO.SelfDber.Entities<CmcsTransport>("where InFactoryBatchId=:InFactoryBatchId", new { InFactoryBatchId = batch.Id });
                commonDAO.SelfDber.Execute("update " + DapperDber.Util.EntityReflectionUtil.GetTableName<CmcsInFactoryBatch>() + " set IsCheck=0 where Id=:Id",
                    new
                    {
                        Id = batch.Id,
                    });
            }

            return res;
        }
    }
}
