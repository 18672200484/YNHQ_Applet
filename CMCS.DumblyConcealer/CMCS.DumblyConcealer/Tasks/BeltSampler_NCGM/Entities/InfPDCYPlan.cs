using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;

namespace CMCS.DumblyConcealer.Tasks.BeltSampler_NCGM.Entities
{
    /// <summary>
    /// 南昌光明火车皮带采样机接口表 - 采样计划
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("inftbpdcyplan")]
    public class InfPDCYPlan : EntityBase2
    {
        private string inFactoryBatchId;
        /// <summary>
        /// 批次Id
        /// </summary>
        public string InFactoryBatchId
        {
            get { return inFactoryBatchId; }
            set { inFactoryBatchId = value; }
        }

        private string sampleCode;
        /// <summary>
        /// 采样码
        /// </summary>
        public string SampleCode
        {
            get { return sampleCode; }
            set { sampleCode = value; }
        }

        private int fuelKindName;
        /// <summary>
        /// 煤种
        /// </summary>
        public int FuelKindName
        {
            get { return fuelKindName; }
            set { fuelKindName = value; }
        }

        private int mt;
        /// <summary>
        /// 外水分
        /// </summary>
        public int Mt
        {
            get { return mt; }
            set { mt = value; }
        }

        private double ticketWeight;
        /// <summary>
        /// 该批次矿发量
        /// </summary>
        public double TicketWeight
        {
            get { return ticketWeight; }
            set { ticketWeight = value; }
        }

        private double carCount;
        /// <summary>
        /// 该批次车节数
        /// </summary>
        public double CarCount
        {
            get { return carCount; }
            set { carCount = value; }
        }

        private int sampleType;
        /// <summary>
        /// 采样方式
        /// </summary>
        public int SampleType
        {
            get { return sampleType; }
            set { sampleType = value; }
        }

        private DateTime startTime;
        /// <summary>
        /// 采样开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private DateTime endTime;
        /// <summary>
        /// 采样结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private string sampleUser;
        /// <summary>
        /// 采样员
        /// </summary>
        public string SampleUser
        {
            get { return sampleUser; }
            set { sampleUser = value; }
        }

        private int dataFlag;
        /// <summary>
        /// 标识符
        /// </summary>
        public int DataFlag
        {
            get { return dataFlag; }
            set { dataFlag = value; }
        }
    }
}
