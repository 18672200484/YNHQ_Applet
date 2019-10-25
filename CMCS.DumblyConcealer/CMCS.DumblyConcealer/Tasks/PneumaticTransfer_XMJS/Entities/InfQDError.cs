using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.DumblyConcealer.Tasks.PneumaticTransfer_XMJS.Entities
{
    [CMCS.DapperDber.Attrs.DapperBind("QD_ERR_Tb")]
    public class InfQDError
    {
        public Decimal Errorcode { get; set; }
        public DateTime ErrorTime { get; set; }
        public String ErrorDec { get; set; }
        public Decimal DataStatus { get; set; }

    }
}
