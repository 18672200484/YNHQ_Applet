using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using CMCS.CarTransport.Out.Core;
using System.Threading;
using System.IO;
using CMCS.CarTransport.DAO;
using CMCS.Common.DAO;
using System.IO.Ports;
using CMCS.Common.Utilities;
using CMCS.CarTransport.Out.Enums;
using CMCS.Common.Entities;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common;
using CMCS.CarTransport.Out.Frms.Sys;
using DevComponents.DotNetBar.Controls;
using CMCS.Common.Views;
using DevComponents.DotNetBar.SuperGrid;
using CMCS.Common.Enums;
using CMCS.Common.Entities.Sys;
using CMCS.Common.Entities.BaseInfo;
using HikVisionSDK.Core;

namespace CMCS.CarTransport.Out.Frms
{
    public partial class FrmVideo : DevComponents.DotNetBar.Metro.MetroForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmVideo";

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

                IPCer.InitSDK();

                CmcsCamare qch1_video1 = commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "一号汽车衡_摄像机1" });
                if (qch1_video1 != null)
                {
                    if (Qch1_iPCer1.Login(qch1_video1.Ip, qch1_video1.Port, qch1_video1.UserName, qch1_video1.Password))
                        Qch1_iPCer1.StartPreview(qch1_panl1.Handle, qch1_video1.Channel);
                }

                CmcsCamare qch1_video2 = commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "一号汽车衡_摄像机2" });
                if (qch1_video2 != null)
                {
                    if (Qch1_iPCer2.Login(qch1_video2.Ip, qch1_video2.Port, qch1_video2.UserName, qch1_video2.Password))
                        Qch1_iPCer2.StartPreview(qch1_panl2.Handle, qch1_video2.Channel);
                }
                CmcsCamare qch1_video3 = commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "一号汽车衡_摄像机3" });
                if (qch1_video3 != null)
                {
                    if (Qch1_iPCer3.Login(qch1_video3.Ip, qch1_video3.Port, qch1_video3.UserName, qch1_video3.Password))
                        Qch1_iPCer3.StartPreview(qch1_panl3.Handle, qch1_video3.Channel);
                }


                CmcsCamare qch2_video1 = commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "二号汽车衡_摄像机1" });
                if (qch2_video1 != null)
                {
                    if (Qch2_iPCer1.Login(qch2_video1.Ip, qch2_video1.Port, qch2_video1.UserName, qch2_video1.Password))
                        Qch2_iPCer1.StartPreview(qch2_panl1.Handle, qch2_video1.Channel);
                }

                CmcsCamare qch2_video2 = commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "二号汽车衡_摄像机2" });
                if (qch2_video2 != null)
                {
                    if (Qch2_iPCer2.Login(qch2_video2.Ip, qch2_video2.Port, qch2_video2.UserName, qch2_video2.Password))
                        Qch2_iPCer2.StartPreview(qch2_panl2.Handle, qch2_video2.Channel);
                }
                CmcsCamare qch2_video3 = commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "二号汽车衡_摄像机3" });
                if (qch2_video3 != null)
                {
                    if (Qch2_iPCer3.Login(qch2_video3.Ip, qch2_video3.Port, qch2_video3.UserName, qch2_video3.Password))
                        Qch2_iPCer3.StartPreview(qch2_panl3.Handle, qch2_video3.Channel);
                }


                CmcsCamare qch3_video1 = commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "三号汽车衡_摄像机1" });
                if (qch3_video1 != null)
                {
                    if (Qch3_iPCer1.Login(qch3_video1.Ip, qch3_video1.Port, qch3_video1.UserName, qch3_video1.Password))
                        Qch3_iPCer1.StartPreview(qch3_panl1.Handle, qch3_video1.Channel);
                }

                CmcsCamare qch3_video2 = commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "三号汽车衡_摄像机2" });
                if (qch3_video2 != null)
                {
                    if (Qch3_iPCer2.Login(qch3_video2.Ip, qch3_video2.Port, qch3_video2.UserName, qch3_video2.Password))
                        Qch3_iPCer2.StartPreview(qch3_panl2.Handle, qch3_video2.Channel);
                }
                CmcsCamare qch3_video3 = commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "三号汽车衡_摄像机3" });
                if (qch3_video3 != null)
                {
                    if (Qch3_iPCer3.Login(qch3_video3.Ip, qch3_video3.Port, qch3_video3.UserName, qch3_video3.Password))
                        Qch3_iPCer3.StartPreview(qch3_panl3.Handle, qch3_video3.Channel);
                }


                CmcsCamare qch4_video1 = commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "四号汽车衡_摄像机1" });
                if (qch4_video1 != null)
                {
                    if (Qch4_iPCer1.Login(qch4_video1.Ip, qch4_video1.Port, qch4_video1.UserName, qch4_video1.Password))
                        Qch4_iPCer1.StartPreview(qch4_panl1.Handle, qch4_video1.Channel);
                }

                CmcsCamare qch4_video2 = commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "四号汽车衡_摄像机2" });
                if (qch4_video2 != null)
                {
                    if (Qch4_iPCer2.Login(qch4_video2.Ip, qch4_video2.Port, qch4_video2.UserName, qch4_video2.Password))
                        Qch4_iPCer2.StartPreview(qch4_panl2.Handle, qch4_video2.Channel);
                }
                CmcsCamare qch4_video3 = commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "四号汽车衡_摄像机3" });
                if (qch4_video3 != null)
                {
                    if (Qch4_iPCer3.Login(qch4_video3.Ip, qch4_video3.Port, qch4_video3.UserName, qch4_video3.Password))
                        Qch4_iPCer3.StartPreview(qch4_panl3.Handle, qch4_video3.Channel);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Log4Neter.Error("设备初始化", ex);
            }
            finally { timer1.Start(); }
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
            try
            {
                decimal[] weight = videoDAO.GetAllQchWeight();
                for (int i = 1; i < 5; i++)
                {
                    Control[] control = this.Controls.Find(string.Format("lebqch{0}_weight", i.ToString()), true);
                    if (control != null)
                    {
                        ((LabelX)control[0]).Text = "当前重量：" + weight[i - 1].ToString();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally { timer1.Start(); }
        }
        #endregion

    }
}
