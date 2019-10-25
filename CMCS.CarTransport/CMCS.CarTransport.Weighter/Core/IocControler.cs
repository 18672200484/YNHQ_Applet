//
using System.Threading;
using CMCS.Common.DAO;
using CMCS.Common;
using CMCS.CarTransport.Weighter.Frms.Sys;

namespace CMCS.CarTransport.Weighter.Core
{
    /// <summary>
    /// IO控制器设备控制
    /// </summary>
    public class IocControler
    {
        IOC.PCI1761.PCI1761Iocer Iocer;

        public IocControler(IOC.PCI1761.PCI1761Iocer iocer)
        {
            this.Iocer = iocer;
        }

        CommonDAO commonDAO = CommonDAO.GetInstance();

        /// <summary>
        /// 道闸1升杆
        /// </summary>
        public void Gate1Up()
        {
#if DEBUG
            FrmDebugConsole.GetInstance().Output("道闸1升杆");
#endif
            FrmDebugConsole.GetInstance().Output("道闸1升杆");
            int port = commonDAO.GetAppletConfigInt32("IO控制器_道闸1升杆端口");

            this.Iocer.Xihe(port);
            Thread.Sleep(100);
            this.Iocer.Shifang(port);
            Thread.Sleep(500);

            commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, "道闸1升杆", "1");
        }

        /// <summary>
        /// 道闸1降杆
        /// </summary>
        public void Gate1Down()
        {
#if DEBUG
            FrmDebugConsole.GetInstance().Output("道闸1降杆");
#endif
            int port = commonDAO.GetAppletConfigInt32("IO控制器_道闸1降杆端口");

            this.Iocer.Shifang(port);

            Thread.Sleep(500);

            commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, "道闸1升杆", "0");
        }

        /// <summary>
        /// 道闸2升杆
        /// </summary>
        public void Gate2Up()
        {
#if DEBUG
            FrmDebugConsole.GetInstance().Output("道闸2升杆");
#endif
            FrmDebugConsole.GetInstance().Output("道闸2升杆");
            int port = commonDAO.GetAppletConfigInt32("IO控制器_道闸2升杆端口");

            this.Iocer.Xihe(port);
            Thread.Sleep(100);
            this.Iocer.Shifang(port);
            Thread.Sleep(500);

            commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, "道闸2升杆", "1");
        }

        /// <summary>
        /// 道闸2降杆
        /// </summary>
        public void Gate2Down()
        {
#if DEBUG
            FrmDebugConsole.GetInstance().Output("道闸2降杆");
#endif
            int port = commonDAO.GetAppletConfigInt32("IO控制器_道闸2降杆端口");

            this.Iocer.Shifang(port);

            Thread.Sleep(500);

            commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, "道闸2升杆", "0");
        }

        /// <summary>
        /// 信号灯1红灯
        /// </summary>
        public void RedLight1()
        {
#if DEBUG
            FrmDebugConsole.GetInstance().Output("信号灯1红灯");
#endif
            this.Iocer.Shifang(commonDAO.GetAppletConfigInt32("IO控制器_信号灯1端口"));
            Thread.Sleep(500);

            commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, "信号灯1", "1");
        }

        /// <summary>
        /// 信号灯1绿灯
        /// </summary>
        public void GreenLight1()
        {
#if DEBUG
            FrmDebugConsole.GetInstance().Output("信号灯1绿灯");
#endif
            this.Iocer.Xihe(commonDAO.GetAppletConfigInt32("IO控制器_信号灯1端口"));
            Thread.Sleep(500);

            commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, "信号灯1", "0");
        }

        /// <summary>
        /// 信号灯2红灯
        /// </summary>
        public void RedLight2()
        {
#if DEBUG
            FrmDebugConsole.GetInstance().Output("信号灯2红灯");
#endif
            this.Iocer.Shifang(commonDAO.GetAppletConfigInt32("IO控制器_信号灯2端口"));
            Thread.Sleep(500);

            commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, "信号灯2", "1");
        }

        /// <summary>
        /// 信号灯2绿灯
        /// </summary>
        public void GreenLight2()
        {
#if DEBUG
            FrmDebugConsole.GetInstance().Output("信号灯2绿灯");
#endif
            this.Iocer.Xihe(commonDAO.GetAppletConfigInt32("IO控制器_信号灯2端口"));
            Thread.Sleep(500);

            commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, "信号灯2", "0");
        }
    }
}
