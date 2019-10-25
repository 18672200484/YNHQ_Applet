using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.BaseInfo
{
    /// <summary>
    /// 基础信息-运输方式
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("fultbtransporttype")]
    public class CmcsTransportType : EntityBase1
    {
        private string _Code;
        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        private string _Name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Valid;
        /// <summary>
        /// 是否有效
        /// </summary>
        public string Valid
        {
            get { return _Valid; }
            set { _Valid = value; }
        }

        private string _DataFrom;
        /// <summary>
        /// 电厂编码
        /// </summary>
        public string DataFrom
        {
            get { return _DataFrom; }
            set { _DataFrom = value; }
        }
    }
}
