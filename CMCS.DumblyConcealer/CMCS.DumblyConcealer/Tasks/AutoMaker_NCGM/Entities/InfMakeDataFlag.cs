using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace CMCS.DumblyConcealer.Tasks.AutoMaker_NCGM.Entities
{
    /// <summary>
    /// 上位机运行状态表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("DataFlag")]
    public class InfMakeDataFlag
    {
        /// <summary>
        /// 运行状态
        /// </summary>		
        private int _DataFlag;
        public int DataFlag
        {
            get { return _DataFlag; }
            set { _DataFlag = value; }
        }
    }
}