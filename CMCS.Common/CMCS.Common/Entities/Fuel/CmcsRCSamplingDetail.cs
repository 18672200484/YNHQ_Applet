using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.DapperDber.Attrs;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.Fuel
{
    /// <summary>
    /// 入厂煤采样明细
    /// </summary>
    [Serializable]
    [DapperBind("FULTBRCHYSAMPLINGDETAIL")]
    public class CmcsRCSamplingDetail : EntityBase1
    {
        /// <summary>
        /// 关联采样ID
        /// </summary>
        public string SamplingId { get; set; }
        /// <summary>
        /// 采样员
        /// </summary>
        public string SamplingPle { get; set; }
        /// <summary>
        ///  桶数
        /// </summary>
        public int BagNum { get; set; }
        /// <summary>
        /// 采样方式
        /// </summary>
        public string SamplingType { get; set; }
        /// <summary>
        /// 采样码
        /// </summary>
        public string BillNumber { get; set; }
        /// <summary>
        ///  车数
        /// </summary>
        public int CarCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DataFrom { get; set; }
    }
}
