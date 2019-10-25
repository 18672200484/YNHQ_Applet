using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.DapperDber.Attrs;
using CMCS.Common.Entities.Sys;
//


namespace CMCS.Common.Entities.CarTransport
{
    /// <summary>
    /// 汽车自动打印磅单
    /// </summary>
    [DapperBind("cmcstbtransportprint")]
    public class TransportPrint:EntityBase1
    {
        private string _transportid;
        /// <summary>
        /// 运输记录id
        /// </summary>
        public string TransportId { get { return _transportid; } set { _transportid = value; } }

        private int _isprint;
        /// <summary>
        /// 是否已打印 0：已打印；1：未打印；
        /// </summary>
        public int IsPrint { get { return _isprint; } set { _isprint = value; } }

        /// <summary>
        /// 车辆类别 入厂煤 其他物资
        /// </summary>
        public string CarType { get; set; }
    }
}
