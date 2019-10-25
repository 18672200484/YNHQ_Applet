
using LED.Listen;
namespace CMCS.CarTransport.Weighter.Core
{
    /// <summary>
    /// 硬件设备类
    /// </summary>
    public class Hardwarer
    {
        static IOC.PCI1761.PCI1761Iocer iocer = new IOC.PCI1761.PCI1761Iocer();
        /// <summary>
        /// IO控制器
        /// </summary>
        public static IOC.PCI1761.PCI1761Iocer Iocer
        {
            get { return iocer; }
        }

        static WB.TOLEDO.YAOHUA.TOLEDO_YAOHUAWber wber = new WB.TOLEDO.YAOHUA.TOLEDO_YAOHUAWber(Common.DAO.CommonDAO.GetInstance().GetAppletConfigInt32("地磅仪表_稳定时间") > 0 ? Common.DAO.CommonDAO.GetInstance().GetAppletConfigInt32("地磅仪表_稳定时间") : 4);
        /// <summary>
        /// 地磅仪表
        /// </summary>
        public static WB.TOLEDO.YAOHUA.TOLEDO_YAOHUAWber Wber
        {
            get { return wber; }
        }

        static RW.Hawkvor.HawkvorRwer rwer1 = new RW.Hawkvor.HawkvorRwer();
        /// <summary>
        /// 读卡器1
        /// </summary>
        public static RW.Hawkvor.HawkvorRwer Rwer1
        {
            get { return rwer1; }
        }

        static RW.Hawkvor.HawkvorRwer rwer2 = new RW.Hawkvor.HawkvorRwer();
        /// <summary>
        /// 读卡器2
        /// </summary>
        public static RW.Hawkvor.HawkvorRwer Rwer2
        {
            get { return rwer2; }
        }

        static LED.YIBO.YiBoDD251 led = new LED.YIBO.YiBoDD251();
        /// <summary>
        /// LED
        /// </summary>
        public static LED.YIBO.YiBoDD251 Led
        {
            get { return led; }
        }

        /// <summary>
        /// 采样机LED屏
        /// </summary>
        static LEDListenAreaLeder ledListenCYJ = new LEDListenAreaLeder();
        public static LEDListenAreaLeder LedListenCYJ
        {
            get { return ledListenCYJ; }
        }
    }
}
