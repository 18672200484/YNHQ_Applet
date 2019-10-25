using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;

namespace CMCS.DumblyConcealer.Tasks.AssayDevice.Entities
{
    /// <summary>
    /// 量热仪
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("hytbhg_sdc5015")]
    public class heatstdassay : EntityBase2
    {
        public string PKID { get; set; }

        public string FACILITYNUMBER { get; set; }

        public string AUTONUM { get; set; }

        public string HANDNUM { get; set; }

        public decimal SYZL { get; set; }

        public decimal DTFRL { get; set; }

        public decimal KGJGW { get; set; }

        public decimal GJGW { get; set; }

        public decimal SDJDW { get; set; }

        public decimal YQRRL { get; set; }

        public decimal DHR { get; set; }

        public decimal TJR { get; set; }

        public decimal QSF { get; set; }

        public decimal FXSF { get; set; }

        public decimal QL { get; set; }

        public decimal FXQ { get; set; }

        public decimal WS { get; set; }

        public decimal HFF { get; set; }

        public decimal HF { get; set; }

        public decimal JZTX { get; set; }

        public DateTime CSTIME { get; set; }

        public DateTime LASTTIME { get; set; }

        public string HYY { get; set; }

        public string SYLX { get; set; }

        public string CSFF { get; set; }

        public string YDBH { get; set; }

        public string BZ { get; set; }

        public decimal V0_FIRST_WD { get; set; }

        public decimal T0 { get; set; }

        public decimal V0 { get; set; }

        public decimal TN { get; set; }

        public decimal ZQWS { get; set; }

        public decimal ZQSJ { get; set; }

        public decimal ZQJFZ { get; set; }

        public decimal MQWD { get; set; }

        public decimal VN { get; set; }

        public decimal C { get; set; }

        public string RSD1 { get; set; }

        public string RSD2 { get; set; }

        public string RSD3 { get; set; }

        public string ID { get; set; }

        public string DEVICEID { get; set; }

        public decimal ADJUST { get; set; }

        public string CANID { get; set; }

        public string KWD { get; set; }

        public string YL1 { get; set; }

        public string YL2 { get; set; }

        public string YL3 { get; set; }

        public decimal DTFRL2 { get; set; }

        public decimal KGJGW2 { get; set; }

        public decimal GJGW2 { get; set; }

        public decimal SDJDW2 { get; set; }

        public decimal KGJDW { get; set; }

        public decimal KGJDW2 { get; set; }

    }
}
