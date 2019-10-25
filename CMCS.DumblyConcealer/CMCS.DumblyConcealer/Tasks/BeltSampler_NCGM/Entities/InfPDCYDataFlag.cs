using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities; 

namespace CMCS.DumblyConcealer.Tasks.BeltSampler_NCGM.Entities
{
    /// <summary>
    /// 南昌光明火车皮带采样机接口表 - 上位机运行状态表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("dataflag")]
    public class InfPDCYDataFlag
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
