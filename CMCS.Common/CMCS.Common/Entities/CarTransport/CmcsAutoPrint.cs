using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.CarTransport
{/// <summary>
    /// 汽车智能化-预制信息
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("CmcsTbAutoPrint")]
    public class CmcsAutoPrint : EntityBase1
    {
        /// <summary>
        /// 运输记录Id
        /// </summary>
        public string TransportId { get; set; }

        /// <summary>
        /// 打印次数
        /// </summary>
        public int PrintCount { get; set; }

        /// <summary>
        /// 状态  0 未打印 1 已打印
        /// </summary>
        public int Status { get; set; }
    }
}
