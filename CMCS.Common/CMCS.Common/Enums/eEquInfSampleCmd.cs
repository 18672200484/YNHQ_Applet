using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Enums
{
    /// <summary>
    /// 第三方设备接口 - 控制命令
    /// </summary>
    public enum eEquInfSampleCmd
    {
        开始采样 = 0,
        批次结束采样 = 1,
        批次临时停止 = 2
    }
}
