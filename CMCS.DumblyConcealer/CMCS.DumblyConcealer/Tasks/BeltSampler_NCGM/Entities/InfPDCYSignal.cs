using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;

namespace CMCS.DumblyConcealer.Tasks.BeltSampler_NCGM.Entities
{
    /// <summary>
    /// 南昌光明火车皮带采样机接口表 - 实时信号
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("inftbpdcysignal")]
    public class InfPDCYSignal : EntityBase2
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

        private string tagName;
        /// <summary>
        /// 信号名
        /// </summary>
        public string TagName
        {
            get { return tagName; }
            set { tagName = value; }
        }

        private string tagValue;
        /// <summary>
        /// 值
        /// </summary>
        public string TagValue
        {
            get { return tagValue; }
            set { tagValue = value; }
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

        private string remark;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
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
