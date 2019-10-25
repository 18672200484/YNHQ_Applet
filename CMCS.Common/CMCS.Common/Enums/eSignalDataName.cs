using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Enums
{
    /// <summary>
    /// 信号名
    /// </summary>
    public enum eSignalDataName
    {
        #region 综合

        系统,
        当前车号,
        当前车Id,

        #endregion

        #region 皮带秤

        速度,
        累计量,
        瞬时流量,

        #endregion

        #region 汽车智能化

        当前运输记录Id,

        地感1信号,
        地感2信号,
        地感3信号,
        地感4信号,
        地感5信号,
        地感6信号,
        地感7信号,
        地感8信号,

        对射1信号,
        对射2信号,
        对射3信号,

        信号灯1,
        信号灯2,

        道闸1升杆,
        道闸2升杆,
        道闸3升杆,
        道闸4升杆,

        上磅方向,
        地磅仪表_稳定,
        地磅仪表_实时重量,
        地磅仪表_称重重量,
        读卡器_连接状态,
        读卡器1_连接状态,
        读卡器2_连接状态,
        当前卡号,

        LED屏1_连接状态,
        LED屏2_连接状态,


        采样机LED屏_连接状态,
        卸煤沟LED屏1_连接状态,
        卸煤沟LED屏2_连接状态,
        卸煤沟LED屏3_连接状态,
        卸煤沟LED屏4_连接状态,
        卸煤沟LED屏5_连接状态,
        卸煤沟LED屏6_连接状态,

        地磅仪表_连接状态,
        IO控制器_连接状态,

        矿点,
        卸煤区域
        #endregion
    }
}
