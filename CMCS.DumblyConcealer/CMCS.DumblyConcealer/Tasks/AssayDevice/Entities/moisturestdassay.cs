using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;

namespace CMCS.DumblyConcealer.Tasks.AssayDevice.Entities
{
    /// <summary>
    /// 水分仪
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("hytbmg_sdtga300")]
    public class moisturestdassay : EntityBase2
    {
        public string PKID { get; set; }
        public string FACILITYNUMBER { get; set; }
        public string MANNUMB { get; set; }
        public decimal SAMPLEMASS { get; set; }
        public decimal POTMASS { get; set; }
        public decimal WATERPER { get; set; }
        public string TESTTIME { get; set; }
        public string METAGEWAY { get; set; }
        public string APPANUMB { get; set; }
        public string WATERTYPE { get; set; }
        public DateTime TESTDATE { get; set; }
        public string TESTMAN { get; set; }
        public string AUTONUMB { get; set; }
        public decimal SPARE1 { get; set; }
        public decimal SPARE2 { get; set; }
        public string SPARE3 { get; set; }
        public string SPARE4 { get; set; }
        public string SPARE5 { get; set; }
    }
}
