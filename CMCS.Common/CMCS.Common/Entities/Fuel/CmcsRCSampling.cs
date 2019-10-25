using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.Fuel
{
    /// <summary>
    /// 入厂煤采样表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("FULTBRCHYSAMPLING")]
    public class CmcsRCSampling : EntityBase1
    {
        /// <summary>
        /// 关联批次ID
        /// </summary>
        public string InFactoryBatchId { get; set; }
        /// <summary>
        /// 采样时间
        /// </summary>
        public DateTime SamplingDate { get; set; }
        /// <summary>
        /// 采样员
        /// </summary>
        public string Sampler { get; set; }
        /// <summary>
        ///  总车数
        /// </summary>
        public int TotalNum { get; set; }
        /// <summary>
        /// 天气
        /// </summary>
        public string Weath { get; set; }
        /// <summary>
        /// 车号
        /// </summary>
        public string CarNums { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DataFrom { get; set; }
    }
}
