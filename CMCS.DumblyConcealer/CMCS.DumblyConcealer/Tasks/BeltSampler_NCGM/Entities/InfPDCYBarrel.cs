using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;

namespace CMCS.DumblyConcealer.Tasks.BeltSampler_NCGM.Entities
{
    /// <summary>
    /// 南昌光明火车皮带采样机接口表 - 实时集样罐
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("inftbpdcybarrel")]
    public class InfPDCYBarrel : EntityBase2
    {
        private string machineCode;
        /// <summary>
        /// 设备编号
        /// </summary>
        public string MachineCode
        {
            get { return machineCode; }
            set { machineCode = value; }
        }

        private string barrelNumber;
        /// <summary>
        /// 罐号
        /// </summary>
        public string BarrelNumber
        {
            get { return barrelNumber; }
            set { barrelNumber = value; }
        }

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

        private int sampleCount;
        /// <summary>
        /// 子样数
        /// </summary>
        public int SampleCount
        {
            get { return sampleCount; }
            set { sampleCount = value; }
        }

        private int isCurrent;
        /// <summary>
        /// 是当前进料罐
        /// </summary>
        public int IsCurrent
        {
            get { return isCurrent; }
            set { isCurrent = value; }
        }

        private int isFull;
        /// <summary>
        /// 桶满状态
        /// </summary>
        public int IsFull
        {
            get { return isFull; }
            set { isFull = value; }
        }

        private DateTime updateTime;
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime
        {
            get { return updateTime; }
            set { updateTime = value; }
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
