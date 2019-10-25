using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys; 

namespace CMCS.Common.Entities.AutoCupboard
{
    /// <summary>
    /// 实时样品表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("InfTBCYGSAM")]
    public class InfCYGSam : EntityBase1
    {
        public virtual String MachineCode { get; set; }
        public virtual String Code { get; set; }
        public virtual String SamType { get; set; }
        public virtual DateTime UpdateTime { get; set; }
        public virtual Decimal IsNew { get; set; }
        public virtual Int32 CellIndex { get; set; }
        public virtual Int32 ColumnIndex { get; set; }
        public virtual Int32 AreaNumber { get; set; }
        public virtual Decimal DataFlag { get; set; }
    }
}
