using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.CarTransport.Weighter.Enums
{
    /// <summary>
    /// 流程标识
    /// </summary>
    public enum eFlowFlag
    {
        等待车辆,
        验证信息,
        等待上磅,
        称重验证,
        等待稳定,
        等待离开
    }
}
