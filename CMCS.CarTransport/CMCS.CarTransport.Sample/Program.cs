using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CMCS.CarTransport.Sample.Frms.Sys;
using System.Diagnostics;
using BasisPlatform;
using CMCS.Common;
using CMCS.DotNetBar.Utilities;
using CMCS.Common.Enums;

namespace CMCS.CarTransport.Sample
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 检测更新
            AU.Updater updater = new AU.Updater();
            if (updater.NeedUpdate())
            {
                Process.Start("AutoUpdater.exe");
                Environment.Exit(0);
            }

            // BasisPlatform:应用程序初始化
            Basiser basiser = Basiser.GetInstance();
            basiser.EnabledEbiaSupport = true;
            basiser.InitBasisPlatform(CommonAppConfig.GetInstance().AppIdentifier, PlatformType.Winform);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

            DotNetBarUtil.InitLocalization();

            try
            {
                CMCS.Common.DAO.CommonDAO.GetInstance().SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.系统.ToString(), "1");
            }
            catch (Exception)
            {
            }

            Application.Run(new FrmLogin());
        }
        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            try
            {
                CMCS.Common.DAO.CommonDAO.GetInstance().SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.系统.ToString(), "0");
            }
            catch (Exception)
            {
            }
        }
    }
}
