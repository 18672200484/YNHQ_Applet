using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;

namespace CMCS.DumblyConcealer.Tasks.AutoMaker_NCGM.Entities
{
    /// <summary>
    /// 南昌光明全自动制样机接口表 - 实时信号表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("InfTbMakeSignal")]
    public class InfMakeSignal : EntityBase2
    {
        private string _MachineCode;
        /// <summary>
        /// 设备编号
        /// </summary>
        public string MachineCode
        {
            get { return _MachineCode; }
            set { _MachineCode = value; }
        }

        private string _TagName;
        /// <summary>
        /// 信号名
        /// </summary>
        public string TagName
        {
            get { return _TagName; }
            set { _TagName = value; }
        }

        private string _TagValue;
        /// <summary>
        /// 信号值
        /// </summary>
        public string TagValue
        {
            get { return _TagValue; }
            set { _TagValue = value; }
        }

        private DateTime _UpdateTime;
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime
        {
            get { return _UpdateTime; }
            set { _UpdateTime = value; }
        }

        private int _DataFlag;
        /// <summary>
        /// 标识符
        /// </summary>
        public int DataFlag
        {
            get { return _DataFlag; }
            set { _DataFlag = value; }
        }

        private string _Remark;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }
    }
}
