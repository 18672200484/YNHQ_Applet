using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.DumblyConcealer.Tasks.AutoCupboard_NCGM.Entities
{
    /// <summary>
    /// 南昌光明存样柜接口表 - 上位机运行状态表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("DATAFLAG")]
    public class InfCYGDataFlag
    {
        private int _DataFlag;
        /// <summary>
        /// 标识符
        /// </summary>
        [CMCS.DapperDber.Attrs.DapperPrimaryKey]
        public int DataFlag
        {
            get { return _DataFlag; }
            set { _DataFlag = value; }
        }
    }
}
