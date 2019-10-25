
namespace CMCS.CarTransport.Queue.Core
{
    /// <summary>
    /// 硬件设备类
    /// </summary>
    public class Hardwarer
    {
        static RW.HawkvorCom.HawkvorComRwer rwer = new RW.HawkvorCom.HawkvorComRwer();
        /// <summary>
        /// 读卡器
        /// </summary>
        public static RW.HawkvorCom.HawkvorComRwer Rwer
        {
            get { return rwer; }
        }

        /// <summary>
        /// LED屏1
        /// </summary>
        static LED.Listen.LEDListenAreaLeder ledListen1 = new LED.Listen.LEDListenAreaLeder();
        public static LED.Listen.LEDListenAreaLeder LedListen1
        {
            get { return ledListen1; }
        }
        /// <summary>
        /// LED屏2
        /// </summary>
        static LED.Listen.LEDListenAreaLeder ledListen2 = new LED.Listen.LEDListenAreaLeder();
        public static LED.Listen.LEDListenAreaLeder LedListen2
        {
            get { return ledListen2; }
        }
        /// <summary>
        /// LED屏3
        /// </summary>
        static LED.Listen.LEDListenAreaLeder ledListen3 = new LED.Listen.LEDListenAreaLeder();
        public static LED.Listen.LEDListenAreaLeder LedListen3
        {
            get { return ledListen3; }
        }
        /// <summary>
        /// LED屏4
        /// </summary>
        static LED.Listen.LEDListenAreaLeder ledListen4 = new LED.Listen.LEDListenAreaLeder();
        public static LED.Listen.LEDListenAreaLeder LedListen4
        {
            get { return ledListen4; }
        }
        /// <summary>
        /// LED屏5
        /// </summary>
        static LED.Listen.LEDListenAreaLeder ledListen5 = new LED.Listen.LEDListenAreaLeder();
        public static LED.Listen.LEDListenAreaLeder LedListen5
        {
            get { return ledListen5; }
        }
        /// <summary>
        /// LED屏6
        /// </summary>
        static LED.Listen.LEDListenAreaLeder ledListen6 = new LED.Listen.LEDListenAreaLeder();
        public static LED.Listen.LEDListenAreaLeder LedListen6
        {
            get { return ledListen6; }
        }
    }
}
