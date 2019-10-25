using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;

namespace CMCS.DumblyConcealer.Tasks.AutoMaker_NCGM.Entities
{
    /// <summary>
    /// 南昌光明全自动制样机接口表 - 制样计划表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("InfTbMakePlan")]
    public class InfMakePlan : EntityBase2
    {
        private string _InFactoryBatchId;
        /// <summary>
        /// 批次Id
        /// </summary>
        public string InFactoryBatchId
        {
            get { return _InFactoryBatchId; }
            set { _InFactoryBatchId = value; }
        }

        private string _MakeCode;
        /// <summary>
        /// 制样码
        /// </summary>
        public string MakeCode
        {
            get { return _MakeCode; }
            set { _MakeCode = value; }
        }

        private int _FuelKindName;
        /// <summary>
        /// 煤种
        /// </summary>
        public int FuelKindName
        {
            get { return _FuelKindName; }
            set { _FuelKindName = value; }
        }

        private int _Mt;
        /// <summary>
        /// 水分 1=湿煤 2=一般湿煤 3=干煤
        /// </summary>
        public int Mt
        {
            get { return _Mt; }
            set { _Mt = value; }
        }

        private int _CoalSize;
        /// <summary>
        /// 煤炭粒度 1=大粒度(50mm≤100mm) 2=小粒度(0mm≤50mm)
        /// </summary>
        public int CoalSize
        {
            get { return _CoalSize; }
            set { _CoalSize = value; }
        }

        private int _MakeType;
        /// <summary>
        /// 制样方式 0=离线 1=在线
        /// </summary>
        public int MakeType
        {
            get { return _MakeType; }
            set { _MakeType = value; }
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
