using System;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

using CMCS.CarTransport.LED.Core;
using LED.Listen;
using CMCS.Common.DAO;
using CMCS.Common.Enums;
using CMCS.Common.Utilities;
using LED.Listen.Enums;
using DevComponents.DotNetBar.Controls;

namespace CMCS.CarTransport.LED.Frms
{
    public partial class FrmLED : DevComponents.DotNetBar.Metro.MetroForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmLED";

        public FrmLED()
        {
            InitializeComponent();
        }

        #region Vars

        CommonDAO commonDAO = null;
        CommonAppConfig commonAppConfig = CommonAppConfig.GetInstance();
        #endregion

        /// <summary>
        /// 窗体初始化
        /// </summary>
        private void InitForm()
        {

        }

        private void FrmWeighter_Load(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(commonAppConfig.Model))
            {
                txtCYJ1.Text = commonAppConfig.CYJLeftContent;
                txtCYJ2.Text = commonAppConfig.CYJRightContent;
                txtXMG1.Text = commonAppConfig.LedContent1;
                txtXMG2.Text = commonAppConfig.LedContent2;
                txtXMG3.Text = commonAppConfig.LedContent3;
                txtXMG4.Text = commonAppConfig.LedContent4;
                txtXMG5.Text = commonAppConfig.LedContent5;
                txtXMG6.Text = commonAppConfig.LedContent6;
            }
            else
            {
                commonDAO = CommonDAO.GetInstance();
            }
        }

        private void FrmWeighter_Shown(object sender, EventArgs e)
        {
            InitHardware();

            InitForm();
        }

        private void FrmQueuer_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 卸载设备
            UnloadHardware();
        }

        #region 设备相关
        #region LED
        void LedListenCYJ_OnStatusChange(bool status)
        {
            // 接收LED状态 
            InvokeEx(() =>
            {
                slightCyjLED.LightColor = (status ? Color.Green : Color.Red);

                if (!Convert.ToBoolean(commonAppConfig.Model)) commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.采样机LED屏_连接状态.ToString(), status ? "1" : "0");
            });
        }
        void LedListenCYJ_OnScanError(Exception error)
        {
            Log4Neter.Error("CYJLED屏", error);
        }

        void LedListen1_OnStatusChange(bool status)
        {
            // 接收LED状态 
            InvokeEx(() =>
            {
                slightLED1.LightColor = (status ? Color.Green : Color.Red);

                if (!Convert.ToBoolean(commonAppConfig.Model)) commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.卸煤沟LED屏1_连接状态.ToString(), status ? "1" : "0");
            });
        }
        void LedListen1_OnScanError(Exception error)
        {
            Log4Neter.Error("卸煤沟LED屏1", error);
        }


        void LedListen2_OnStatusChange(bool status)
        {
            // 接收LED状态 
            InvokeEx(() =>
            {
                slightLED2.LightColor = (status ? Color.Green : Color.Red);

                if (!Convert.ToBoolean(commonAppConfig.Model)) commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.卸煤沟LED屏2_连接状态.ToString(), status ? "1" : "0");
            });
        }
        void LedListen2_OnScanError(Exception error)
        {
            Log4Neter.Error("卸煤沟LED屏2", error);
        }

        void LedListen3_OnStatusChange(bool status)
        {
            // 接收LED状态 
            InvokeEx(() =>
            {
                slightLED3.LightColor = (status ? Color.Green : Color.Red);

                if (!Convert.ToBoolean(commonAppConfig.Model)) commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.卸煤沟LED屏3_连接状态.ToString(), status ? "1" : "0");
            });
        }
        void LedListen3_OnScanError(Exception error)
        {
            Log4Neter.Error("卸煤沟LED屏3", error);
        }

        void LedListen4_OnStatusChange(bool status)
        {
            // 接收LED状态 
            InvokeEx(() =>
            {
                slightLED4.LightColor = (status ? Color.Green : Color.Red);

                if (!Convert.ToBoolean(commonAppConfig.Model)) commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.卸煤沟LED屏4_连接状态.ToString(), status ? "1" : "0");
            });
        }
        void LedListen4_OnScanError(Exception error)
        {
            Log4Neter.Error("卸煤沟LED屏4", error);
        }

        void LedListen5_OnStatusChange(bool status)
        {
            // 接收LED状态 
            InvokeEx(() =>
            {
                slightLED5.LightColor = (status ? Color.Green : Color.Red);

                if (!Convert.ToBoolean(commonAppConfig.Model)) commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.卸煤沟LED屏5_连接状态.ToString(), status ? "1" : "0");
            });
        }
        void LedListen5_OnScanError(Exception error)
        {
            Log4Neter.Error("卸煤沟LED屏5", error);
        }

        void LedListen6_OnStatusChange(bool status)
        {
            // 接收LED状态 
            InvokeEx(() =>
            {
                slightLED6.LightColor = (status ? Color.Green : Color.Red);

                if (!Convert.ToBoolean(commonAppConfig.Model)) commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.卸煤沟LED屏6_连接状态.ToString(), status ? "6" : "0");
            });
        }
        void LedListen6_OnScanError(Exception error)
        {
            Log4Neter.Error("卸煤沟LED屏6", error);
        }
        #endregion

        #region 设备初始化与卸载

        /// <summary>
        /// 初始化外接设备
        /// </summary>
        private void InitHardware()
        {
            try
            {
                bool success = false;

                //LED
                Hardwarer.LedListenCYJ.OnStatusChange += new LEDListenAreaLeder.StatusChangeHandler(LedListenCYJ_OnStatusChange);
                Hardwarer.LedListenCYJ.OnScanError += new LEDListenAreaLeder.ScanErrorEventHandler(LedListenCYJ_OnScanError);
                success = Hardwarer.LedListenCYJ.Init(Convert.ToBoolean(commonAppConfig.Model) ? commonAppConfig.LedIpCYJ : commonDAO.GetAppletConfigString("LEDCYJ_IP"));
                if (!success) MessageBoxEx.Show("采样机Led屏连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Hardwarer.LedListen1.LedWidth = 192;
                Hardwarer.LedListen1.LedHeight = 48;
                Hardwarer.LedListen1.OnStatusChange += new LEDListenAreaLeder.StatusChangeHandler(LedListen1_OnStatusChange);
                Hardwarer.LedListen1.OnScanError += new LEDListenAreaLeder.ScanErrorEventHandler(LedListen1_OnScanError);
                success = Hardwarer.LedListen1.Init(Convert.ToBoolean(commonAppConfig.Model) ? commonAppConfig.LedIp1 : commonDAO.GetAppletConfigString("LED1_IP"));
                if (!success) MessageBoxEx.Show("Led屏1连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Hardwarer.LedListen2.LedWidth = 192;
                Hardwarer.LedListen2.LedHeight = 48;
                Hardwarer.LedListen2.OnStatusChange += new LEDListenAreaLeder.StatusChangeHandler(LedListen2_OnStatusChange);
                Hardwarer.LedListen2.OnScanError += new LEDListenAreaLeder.ScanErrorEventHandler(LedListen2_OnScanError);
                success = Hardwarer.LedListen2.Init(Convert.ToBoolean(commonAppConfig.Model) ? commonAppConfig.LedIp2 : commonDAO.GetAppletConfigString("LED2_IP"));
                if (!success) MessageBoxEx.Show("Led屏2连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Hardwarer.LedListen3.LedWidth = 192;
                Hardwarer.LedListen3.LedHeight = 48;
                Hardwarer.LedListen3.OnStatusChange += new LEDListenAreaLeder.StatusChangeHandler(LedListen3_OnStatusChange);
                Hardwarer.LedListen3.OnScanError += new LEDListenAreaLeder.ScanErrorEventHandler(LedListen3_OnScanError);
                success = Hardwarer.LedListen3.Init(Convert.ToBoolean(commonAppConfig.Model) ? commonAppConfig.LedIp3 : commonDAO.GetAppletConfigString("LED3_IP"));
                if (!success) MessageBoxEx.Show("Led屏3连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Hardwarer.LedListen4.LedWidth = 192;
                Hardwarer.LedListen4.LedHeight = 48;
                Hardwarer.LedListen4.OnStatusChange += new LEDListenAreaLeder.StatusChangeHandler(LedListen4_OnStatusChange);
                Hardwarer.LedListen4.OnScanError += new LEDListenAreaLeder.ScanErrorEventHandler(LedListen4_OnScanError);
                success = Hardwarer.LedListen4.Init(Convert.ToBoolean(commonAppConfig.Model) ? commonAppConfig.LedIp4 : commonDAO.GetAppletConfigString("LED4_IP"));
                if (!success) MessageBoxEx.Show("Led屏4连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Hardwarer.LedListen5.LedWidth = 192;
                Hardwarer.LedListen5.LedHeight = 48;
                Hardwarer.LedListen5.OnStatusChange += new LEDListenAreaLeder.StatusChangeHandler(LedListen5_OnStatusChange);
                Hardwarer.LedListen5.OnScanError += new LEDListenAreaLeder.ScanErrorEventHandler(LedListen5_OnScanError);
                success = Hardwarer.LedListen5.Init(Convert.ToBoolean(commonAppConfig.Model) ? commonAppConfig.LedIp5 : commonDAO.GetAppletConfigString("LED5_IP"));
                if (!success) MessageBoxEx.Show("Led屏5连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Hardwarer.LedListen6.LedWidth = 192;
                Hardwarer.LedListen6.LedHeight = 48;
                Hardwarer.LedListen6.OnStatusChange += new LEDListenAreaLeder.StatusChangeHandler(LedListen6_OnStatusChange);
                Hardwarer.LedListen6.OnScanError += new LEDListenAreaLeder.ScanErrorEventHandler(LedListen6_OnScanError);
                success = Hardwarer.LedListen6.Init(Convert.ToBoolean(commonAppConfig.Model) ? commonAppConfig.LedIp6 : commonDAO.GetAppletConfigString("LED6_IP"));
                if (!success) MessageBoxEx.Show("Led屏6连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                Log4Neter.Error("设备初始化", ex);
            }
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

        #endregion


        #region 其他函数

        /// <summary>
        /// Invoke封装
        /// </summary>
        /// <param name="action"></param>
        public void InvokeEx(Action action)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;

            this.Invoke(action);
        }

        #endregion

        #region 发送文字事件

        private void btnSend_CYJ_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCYJ1.Text) || string.IsNullOrEmpty(txtCYJ2.Text))
            {
                MessageBoxEx.Show("请输入采样机文字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (Convert.ToBoolean(commonAppConfig.Model))
                {
                    commonAppConfig.CYJLeftContent = txtCYJ1.Text;
                    commonAppConfig.CYJRightContent = txtCYJ2.Text;
                    commonAppConfig.Save();
                }
                else
                {
                    commonDAO.SetCommonAppletConfig("采样机LED显示屏_左内容", txtCYJ1.Text);
                    commonDAO.SetCommonAppletConfig("采样机LED显示屏_右内容", txtCYJ2.Text);
                }
                if (SendCYJLed(txtCYJ1.Text, txtCYJ2.Text, eInitStyle.立即显示))
                {
                    MessageBoxEx.Show("发送成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                    MessageBoxEx.Show("发送失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSend_LED_Click(object sender, EventArgs e)
        {
            ButtonX btn = (ButtonX)sender;
            TextBoxX txt = new TextBoxX();
            string code = string.Empty;
            LEDListenAreaLeder led = new LEDListenAreaLeder();
            switch (btn.Name)
            {
                case "btnSend_XMG1":
                    txt = txtXMG1;
                    code = "1";
                    led = Hardwarer.LedListen1;
                    if (Convert.ToBoolean(commonAppConfig.Model))
                        commonAppConfig.LedContent1 = txt.Text;
                    else
                        commonDAO.SetCommonAppletConfig("卸煤沟1显示屏内容", txt.Text);
                    break;
                case "btnSend_XMG2":
                    txt = txtXMG2;
                    code = "2";
                    led = Hardwarer.LedListen2;
                    if (Convert.ToBoolean(commonAppConfig.Model))
                        commonAppConfig.LedContent2 = txt.Text;
                    else
                        commonDAO.SetCommonAppletConfig("卸煤沟2显示屏内容", txt.Text);
                    break;
                case "btnSend_XMG3":
                    txt = txtXMG3;
                    code = "3";
                    led = Hardwarer.LedListen3;
                    if (Convert.ToBoolean(commonAppConfig.Model))
                        commonAppConfig.LedContent3 = txt.Text;
                    else
                        commonDAO.SetCommonAppletConfig("卸煤沟3显示屏内容", txt.Text);
                    break;
                case "btnSend_XMG4":
                    txt = txtXMG4;
                    code = "4";
                    led = Hardwarer.LedListen4;
                    if (Convert.ToBoolean(commonAppConfig.Model))
                        commonAppConfig.LedContent4 = txt.Text;
                    else
                        commonDAO.SetCommonAppletConfig("卸煤沟4显示屏内容", txt.Text);
                    break;
                case "btnSend_XMG5":
                    txt = txtXMG5;
                    code = "5";
                    led = Hardwarer.LedListen5;
                    if (Convert.ToBoolean(commonAppConfig.Model))
                        commonAppConfig.LedContent5 = txt.Text;
                    else
                        commonDAO.SetCommonAppletConfig("卸煤沟5显示屏内容", txt.Text);
                    break;
                case "btnSend_XMG6":
                    txt = txtXMG6;
                    code = "6";
                    led = Hardwarer.LedListen6;
                    if (Convert.ToBoolean(commonAppConfig.Model))
                        commonAppConfig.LedContent6 = txt.Text;
                    else
                        commonDAO.SetCommonAppletConfig("卸煤沟6显示屏内容", txt.Text);
                    break;
                default:
                    break;
            }

            if (string.IsNullOrEmpty(txt.Text))
            {
                MessageBoxEx.Show("请输入卸煤沟" + code + "文字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                commonAppConfig.Save();
                if (SendXMGLed(txt.Text, led, code))
                    MessageBoxEx.Show("发送成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBoxEx.Show("发送失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region 发送LED
        private bool SendCYJLed(string LeftContent, string RightContent, eInitStyle style)
        {

            bool success = false;
            success = Hardwarer.LedListenCYJ.InitProgram();
            if (!success)
            {
                Log4Neter.Info("采样机节目初始化失败:" + Hardwarer.LedListenCYJ.ErrStr);
            }
            //二号衡边框
            success = Hardwarer.LedListenCYJ.AddWaterBorder(0, 0, 288, 32, 1);
            if (!success)
            {
                //MessageBoxEx.Show("二号衡边框发送失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Log4Neter.Info("二号衡边框发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
            }
            //一号衡边框
            success = Hardwarer.LedListenCYJ.AddWaterBorder(288, 0, 288, 32, 2);
            if (!success)
            {
                Log4Neter.Info("一号衡边框发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
            }
            //二号衡文字边框
            success = Hardwarer.LedListenCYJ.AddWaterBorder(0, 32, 288, 64, 3);
            if (!success)
            {
                Log4Neter.Info("二号衡文字边框发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
            }
            //一号衡文字边框
            success = Hardwarer.LedListenCYJ.AddWaterBorder(288, 32, 288, 64, 4);
            if (!success)
            {
                Log4Neter.Info("一号衡文字边框发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
            }

            //二号衡标题
            success = Hardwarer.LedListenCYJ.SendMultiLineTextToImageTextArea("二号汽车衡", 1, 1, 286, 30, 18, eInitStyle.立即显示, 5, 2);
            if (!success)
            {
                Log4Neter.Info("二号衡标题发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
            }

            //一号衡标题
            success = Hardwarer.LedListenCYJ.SendMultiLineTextToImageTextArea("一号汽车衡", 289, 1, 286, 30, 18, eInitStyle.立即显示, 6, 2);
            if (!success)
            {
                Log4Neter.Info("一号衡标题发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
            }

            //二号衡内容
            success = Hardwarer.LedListenCYJ.SendMultiLineTextToImageTextArea(RightContent, 1, 33, 286, 62, 12, style, 7, 1);
            if (!success)
            {
                Log4Neter.Info("二号衡标题发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
            }
            //一号衡内容
            success = Hardwarer.LedListenCYJ.SendMultiLineTextToImageTextArea(LeftContent, 289, 33, 286, 62, 12, style, 8, 1);
            if (!success)
            {
                Log4Neter.Info("一号衡标题发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
            }
            success = Hardwarer.LedListenCYJ.Send(true);
            if (!success)
            {
                Log4Neter.Info("采样机节目发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
            }
            return success;
        }

        private bool SendXMGLed(string Content, LEDListenAreaLeder leder, string code)
        {

            LEDListenAreaLeder Leder = leder;
            Leder.InitProgram();
            Leder.LedWidth = 192;
            Leder.LedHeight = 48;
            bool success = false;
            //左区域边框
            success = Leder.AddWaterBorder(0, 0, 64, 48, 1);
            if (!success)
            {
                Log4Neter.Info("" + code + "号卸煤沟左区域边框发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
            }
            //右区域边框
            success = Leder.AddWaterBorder(64, 0, 128, 48, 2);
            if (!success)
            {
                Log4Neter.Info("" + code + "号卸煤沟右区域边框发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
            }
            //左区域文字
            success = Leder.SendSingleTextByArea("" + code + "区", 0, 0, 64, 48, 12, eInitStyle.立即显示, 3);
            if (!success)
            {
                Log4Neter.Info("" + code + "左区域文字发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
            }
            //右区域文字
            success = Leder.SendSingleTextByArea(Content, 64, 0, 128, 48, 12, eInitStyle.立即显示, 4);
            if (!success)
            {
                Log4Neter.Info("" + code + "右区域文字发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
            }
            return false;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //SendCYJLed(commonAppConfig.CYJLeftContent, commonAppConfig.CYJRightContent, eInitStyle.连续左移);
        }

        #endregion


    }
}
