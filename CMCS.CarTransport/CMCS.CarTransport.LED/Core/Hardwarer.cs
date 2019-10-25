

using LED.Listen;
namespace CMCS.CarTransport.LED.Core
{
    /// <summary>
    /// 硬件设备类
    /// </summary>
    public class Hardwarer
    {
        /// <summary>
        /// 采样机LED屏
        /// </summary>
        static LEDListenAreaLeder ledListenCYJ = new LEDListenAreaLeder();
        public static LEDListenAreaLeder LedListenCYJ
        {
            get { return ledListenCYJ; }
        }
        /// <summary>
        /// LED屏1
        /// </summary>
        static LEDListenAreaLeder ledListen1 = new LEDListenAreaLeder();
        public static LEDListenAreaLeder LedListen1
        {
            get { return ledListen1; }
        }
        /// <summary>
        /// LED屏2
        /// </summary>
        static LEDListenAreaLeder ledListen2 = new LEDListenAreaLeder();
        public static LEDListenAreaLeder LedListen2
        {
            get { return ledListen2; }
        }
        /// <summary>
        /// LED屏3
        /// </summary>
        static LEDListenAreaLeder ledListen3 = new LEDListenAreaLeder();
        public static LEDListenAreaLeder LedListen3
        {
            get { return ledListen3; }
        }
        /// <summary>
        /// LED屏4
        /// </summary>
        static LEDListenAreaLeder ledListen4 = new LEDListenAreaLeder();
        public static LEDListenAreaLeder LedListen4
        {
            get { return ledListen4; }
        }
        /// <summary>
        /// LED屏5
        /// </summary>
        static LEDListenAreaLeder ledListen5 = new LEDListenAreaLeder();
        public static LEDListenAreaLeder LedListen5
        {
            get { return ledListen5; }
        }
        /// <summary>
        /// LED屏6
        /// </summary>
        static LEDListenAreaLeder ledListen6 = new LEDListenAreaLeder();
        public static LEDListenAreaLeder LedListen6
        {
            get { return ledListen6; }
        }
    }
}
