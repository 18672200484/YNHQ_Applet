using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;

namespace CMCS.DumblyConcealer.Tasks.BeltSampler_NCGM.Entities
{
    /// <summary>
    /// 南昌光明火车皮带采样机接口表 - 卸样命令
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("inftbpdcyunloadcmd")]
    public class InfPDCYUnloadCMD : EntityBase2
    {
        private string machineCode;
        /// <summary>
        /// 设备编号
        /// </summary>
        public string MachineCode
        {
            get { return machineCode; }
            set { machineCode = value; }
        }

        private string sampleCode;
        /// <summary>
        /// 采样码
        /// </summary>
        public string SampleCode
        {
            get { return sampleCode; }
            set { sampleCode = value; }
        }

        private int unloadType;
        /// <summary>
        /// 卸样方式
        /// </summary>
        public int UnloadType
        {
            get { return unloadType; }
            set { unloadType = value; }
        }

        private int resultCode;
        /// <summary>
        /// 执行结果
        /// </summary>
        public int ResultCode
        {
            get { return resultCode; }
            set { resultCode = value; }
        }

        private int dataFlag;
        /// <summary>
        /// 标识符
        /// </summary>
        public int DataFlag
        {
            get { return dataFlag; }
            set { dataFlag = value; }
        }
    }
}
