using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.DumblyConcealer.Tasks.AutoMaker_NCGM.Enums
{
    public static class EnumClass
    {
        /// <summary>
        /// 南昌光明全自动制样机接口 - 煤种
        /// </summary>
        public enum eFuelKindName
        {
            褐煤 = 1,
            烟煤 = 2,
            其它 = 3,
        }

        /// <summary>
        /// 南昌光明全自动制样机接口 - 设备编码
        /// </summary>
        public enum eMachineCode
        {
            /// <summary>
            /// #1制样机
            /// </summary>
            ZYJ01
        }

        /// <summary>
        /// 南昌光明全自动制样机接口 - 水分
        /// </summary>
        public enum eMt
        {
            湿煤 = 1,
            一般湿煤 = 2,
            干煤 = 3,
        }
    }
}
