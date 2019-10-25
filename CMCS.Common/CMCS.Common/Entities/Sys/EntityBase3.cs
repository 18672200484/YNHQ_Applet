using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.DapperDber.Attrs;

namespace CMCS.Common.Entities.Sys
{
    [Serializable]
    public class EntityBase3 : EntityBase1
    {
        public EntityBase3()
        {
            this.DataFrom = GlobalVars.DataFrom;
        }

        public string DataFrom { get; set; }
    }
}
