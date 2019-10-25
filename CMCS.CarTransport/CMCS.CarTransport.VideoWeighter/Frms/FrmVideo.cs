using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using CMCS.CarTransport.VideoWeighter.Core;
using System.Threading;
using System.IO;
using CMCS.CarTransport.DAO;
using CMCS.Common.DAO;
using System.IO.Ports;
using CMCS.Common.Utilities;
using CMCS.CarTransport.VideoWeighter.Enums;
using CMCS.Common.Entities;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common;
using CMCS.CarTransport.VideoWeighter.Frms.Sys;
using DevComponents.DotNetBar.Controls;
using CMCS.Common.Views;
using DevComponents.DotNetBar.SuperGrid;
using CMCS.Common.Enums;
using CMCS.Common.Entities.Sys;
using CMCS.Common.Entities.BaseInfo;
using HikVisionSDK.Core;
using DevComponents.DotNetBar.SuperGrid.Style;

namespace CMCS.CarTransport.VideoWeighter.Frms
{
    public partial class FrmVideo : DevComponents.DotNetBar.Metro.MetroForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmVideoWeighter";

        public FrmVideo()
        {
            InitializeComponent();
        }

        #region Vars

        CarTransportDAO carTransportDAO = CarTransportDAO.GetInstance();
        OuterDAO outerDAO = OuterDAO.GetInstance();
        CommonDAO commonDAO = CommonDAO.GetInstance();
        VideoDAO videoDAO = VideoDAO.GetInstance();
        #endregion

        /// <summary>
        /// 窗体初始化
        /// </summary>
        private void InitForm()
        {
        }

        private void FrmVideo_Load(object sender, EventArgs e)
        {
            LoadTodayBuyFuelTransport();
        }

        private void FrmVideo_Shown(object sender, EventArgs e)
        {
            InitHardware();
            InitForm();
        }

        private void FrmVideo_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 卸载设备
            UnloadHardware();
        }

        #region 设备相关
        #region 海康视频

        /// <summary>
        /// 海康网络摄像机
        /// </summary>
        IPCer Qch1_iPCer1 = new IPCer();
        IPCer Qch1_iPCer2 = new IPCer();
        IPCer Qch1_iPCer3 = new IPCer();

        IPCer Qch2_iPCer1 = new IPCer();
        IPCer Qch2_iPCer2 = new IPCer();
        IPCer Qch2_iPCer3 = new IPCer();

        IPCer Qch3_iPCer1 = new IPCer();
        IPCer Qch3_iPCer2 = new IPCer();
        IPCer Qch3_iPCer3 = new IPCer();

        IPCer Qch4_iPCer1 = new IPCer();
        IPCer Qch4_iPCer2 = new IPCer();
        IPCer Qch4_iPCer3 = new IPCer();

        #endregion

        #region 设备初始化与卸载

        /// <summary>
        /// 初始化外接设备
        /// </summary>
        private void InitHardware()
        {
            try
            {
                #region 海康视频
                Log4Neter.Info("摄像机开始初始化");
                IPCer.InitSDK();

                //一号汽车衡
                string ip = commonDAO.GetAppletConfigString("刻录机IP");
                string username = commonDAO.GetAppletConfigString("刻录机用户名");
                string pwd = commonDAO.GetAppletConfigString("刻录机密码");
                int port = commonDAO.GetAppletConfigInt32("刻录机端口号");
                int channel1 = commonDAO.GetAppletConfigInt32("刻录机通道号1");
                int channel2 = commonDAO.GetAppletConfigInt32("刻录机通道号2");
                int channel3 = commonDAO.GetAppletConfigInt32("刻录机通道号3");

                Log4Neter.Info("摄像机开始登陆");
                if (Qch1_iPCer1.Login(ip, port, username, pwd))
                {
                    Log4Neter.Info(string.Format("摄像机登陆成功:参数 {0}", ip + username + pwd + port.ToString() + channel1.ToString()));
                    if (Qch1_iPCer1.StartPreview(qch1_panl1.Handle, channel1))
                        Log4Neter.Info("摄像机预览成功");
                }

                //二号汽车衡
                ip = commonDAO.GetAppletConfigString("刻录机2IP");
                username = commonDAO.GetAppletConfigString("刻录机2用户名");
                pwd = commonDAO.GetAppletConfigString("刻录机2密码");
                port = commonDAO.GetAppletConfigInt32("刻录机2端口号");
                channel1 = commonDAO.GetAppletConfigInt32("刻录机2通道号1");
                channel2 = commonDAO.GetAppletConfigInt32("刻录机2通道号2");
                channel3 = commonDAO.GetAppletConfigInt32("刻录机2通道号3");

                Log4Neter.Info("摄像机开始登陆");
                if (Qch2_iPCer1.Login(ip, port, username, pwd))
                {
                    Log4Neter.Info("摄像机登陆成功");
                    Qch2_iPCer1.StartPreview(qch1_panl2.Handle, channel1);
                    Log4Neter.Info("摄像机预览成功");
                }
                //三号汽车衡
                ip = commonDAO.GetAppletConfigString("刻录机3IP");
                username = commonDAO.GetAppletConfigString("刻录机3用户名");
                pwd = commonDAO.GetAppletConfigString("刻录机3密码");
                port = commonDAO.GetAppletConfigInt32("刻录机3端口号");
                channel1 = commonDAO.GetAppletConfigInt32("刻录机3通道号1");
                channel2 = commonDAO.GetAppletConfigInt32("刻录机3通道号2");
                channel3 = commonDAO.GetAppletConfigInt32("刻录机3通道号3");

                if (Qch3_iPCer1.Login(ip, port, username, pwd))
                    Qch3_iPCer1.StartPreview(qch1_panl3.Handle, channel1);

                //四号汽车衡
                ip = commonDAO.GetAppletConfigString("刻录机4IP");
                username = commonDAO.GetAppletConfigString("刻录机4用户名");
                pwd = commonDAO.GetAppletConfigString("刻录机4密码");
                port = commonDAO.GetAppletConfigInt32("刻录机4端口号");
                channel1 = commonDAO.GetAppletConfigInt32("刻录机4通道号1");
                channel2 = commonDAO.GetAppletConfigInt32("刻录机4通道号2");
                channel3 = commonDAO.GetAppletConfigInt32("刻录机4通道号3");

                if (Qch4_iPCer1.Login(ip, port, username, pwd))
                    Qch4_iPCer1.StartPreview(qch1_panl4.Handle, channel1);

                #endregion
            }
            catch (Exception ex)
            {
                Log4Neter.Error("设备初始化", ex);
            }
            finally { timer1.Start(); }
        }

        /// <summary>
        /// 加载运输记录
        /// </summary>
        void LoadTodayBuyFuelTransport()
        {
            //未完成运输记录
            IList<View_BuyFuelTransport> UnFinishlist = carTransportDAO.GetBuyFuelTransportByStrWhere(string.Format("where (InFactoryTime>='{0}' and InFactoryTime<'{1}') or (SuttleWeight=0 and IsUse=1 and UnFinishTransportId is not null) order by TareTime,GrossTime,InFactoryTime desc", DateTime.Now.Date, DateTime.Now.AddDays(1).Date));
            superGridControl1_BuyFuel.PrimaryGrid.DataSource = UnFinishlist;
        }

        /// <summary>
        /// 卸载设备
        /// </summary>
        private void UnloadHardware()
        {
            // 注意此段代码
            Application.DoEvents();

        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadTodayBuyFuelTransport();
        }

        #endregion

        #region DataGridView
        private void superTabItem1_DoubleClick(object sender, EventArgs e)
        {
            LoadTodayBuyFuelTransport();
        }

        private void superGridControl1_BuyFuel_GetRowHeaderText(object sender, GridGetRowHeaderTextEventArgs e)
        {
            e.Text = (e.GridRow.Index + 1).ToString();
        }
        #endregion
    }
}
