using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using CMCS.Common.Entities;

namespace CMCS.DumblyConcealer.Tasks.AutoMaker_NCGM.Entities
{
    /// <summary>
    /// 控制命令表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("InftbMakeControlCMD")]
    public class InfMakeControlCMD : EntityBase2
    {
        /// <summary>
        /// 命令代码
        /// </summary>		
        private int _CMDCode;
        public int CMDCode
        {
            get { return _CMDCode; }
            set { _CMDCode = value; }
        }

        /// <summary>
        /// 制样码
        /// </summary>		
        private string _MakeCode;
        public string MakeCode
        {
            get { return _MakeCode; }
            set { _MakeCode = value; }
        }

        /// <summary>
        /// 附加值1
        /// </summary>		
        private string _Attached1;
        public string Attached1
        {
            get { return _Attached1; }
            set { _Attached1 = value; }
        }

        /// <summary>
        /// 附加值2
        /// </summary>		
        private string _Attached2;
        public string Attached2
        {
            get { return _Attached2; }
            set { _Attached2 = value; }
        }

        /// <summary>
        /// 附加值3
        /// </summary>		
        private string _Attached3;
        public string Attached3
        {
            get { return _Attached3; }
            set { _Attached3 = value; }
        }

        /// <summary>
        /// 执行结果
        /// </summary>		
        private int _ResultCode;
        public int ResultCode
        {
            get { return _ResultCode; }
            set { _ResultCode = value; }
        }

        /// <summary>
        /// 标识符
        /// </summary>		
        private int _DataFlag;
        public int DataFlag
        {
            get { return _DataFlag; }
            set { _DataFlag = value; }
        }


    }
}