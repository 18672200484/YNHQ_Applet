using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;

namespace CMCS.DumblyConcealer.Tasks.AutoMaker_NCGM.Entities
{
    /// <summary>
    /// 南昌光明全自动制样机接口表 - 制样计划样品子表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("InfTbMakeDetail")]
    public class InfMakeDetail : EntityBase2
    {
        private string _MakeCode;
        /// <summary>
        /// 制样码
        /// </summary>
        public string MakeCode
        {
            get { return _MakeCode; }
            set { _MakeCode = value; }
        }

        private string _BarrelCode;
        /// <summary>
        /// 样罐编码
        /// </summary>
        public string BarrelCode
        {
            get { return _BarrelCode; }
            set { _BarrelCode = value; }
        }

        private string _YPType;
        /// <summary>
        /// 样品类型   1=6mm全水分 2=3mm备查样 3=0.2mm分析样 4=0.2mm备查样 5=……
        /// </summary>
        public string YPType
        {
            get { return _YPType; }
            set { _YPType = value; }
        }

        private double _YPWeight;
        /// <summary>
        /// 样品重量 单位：克
        /// </summary>
        public double YPWeight
        {
            get { return _YPWeight; }
            set { _YPWeight = value; }
        }

        private DateTime _StartTime;
        /// <summary>
        /// 制样开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }

        private DateTime _EndTime;
        /// <summary>
        /// 出样时间
        /// </summary>
        public DateTime EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }

        private string _MakeUser;
        /// <summary>
        /// 制样员
        /// </summary>
        public string MakeUser
        {
            get { return _MakeUser; }
            set { _MakeUser = value; }
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
    }
}
