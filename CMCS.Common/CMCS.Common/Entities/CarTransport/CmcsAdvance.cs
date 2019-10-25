using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CMCS.Common.Entities.Sys; 

namespace CMCS.Common.Entities.CarTransport
{
    /// <summary>
    /// 汽车智能化-预制信息
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("CmcsTbAdvance")]
    public class CmcsAdvance : EntityBase1
    {
        /// <summary>
        /// 标识卡卡号
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 运输记录Id
        /// </summary>
        public string TransportId { get; set; }
        
        /// <summary>
        /// 车号
        /// </summary>
        public string CarNumber { get; set; }
        /// <summary>
        /// 车辆类别 入厂煤 其他物资
        /// </summary>
        public string CarType { get; set; }

        /// <summary>
        /// 是否已完结 0 未完结  1 已完结
        /// </summary>
        public string IsFinish { get; set; }
    }
}
