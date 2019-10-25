using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;

namespace CMCS.DumblyConcealer.Tasks.AssayDevice.Entities
{
    /// <summary>
    /// 测硫仪
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("hytbsg_sds616")]
    public class sulfurstdassay : EntityBase2
    {
        public string PKID { get; set; }

        public string FACILITYNUMBER { get; set; }

        public decimal AUTONUMB { get; set; }

        public string MANNUMB { get; set; }

        public decimal SAMPLEMASS { get; set; }

        public decimal WATERPER { get; set; }

        public decimal SAD { get; set; }

        public decimal DRYBSAD { get; set; }

        public string TESTMAN { get; set; }

        public DateTime TESTDATE { get; set; }

        public string MINENAME { get; set; }

        public string COALCLASS { get; set; }

        public DateTime SAMPLINGTIME { get; set; }

        public decimal TARE { get; set; }

        public decimal MT { get; set; }

        public decimal AVERAGE { get; set; }

        public string FLAG { get; set; }

        public decimal SPARE1 { get; set; }

        public decimal SPARE2 { get; set; }

        public string SPARE3 { get; set; }

        public string SPARE4 { get; set; }

        public string SPARE5 { get; set; }
    }
}
