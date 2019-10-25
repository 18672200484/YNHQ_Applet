using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using CMCS.CarTransport.Weighter.Core;
using System.IO.Ports;
using CMCS.Common.DAO;
using CMCS.Common;
using CMCS.DapperDber.Dbs.OracleDb;
using CMCS.Common.Utilities;

namespace CMCS.CarTransport.Weighter.Frms
{
    public partial class FrmSetting : DevComponents.DotNetBar.Metro.MetroForm
    {
        CommonDAO commonDAO = CommonDAO.GetInstance();

        CommonAppConfig commonAppConfig = CommonAppConfig.GetInstance();

        public FrmSetting()
        {
            InitializeComponent();
        }

        void InitForm()
        {
            InitNumberAscComboBoxs(0, 15, cmbInductorCoil1Port, cmbInductorCoil2Port, cmbInductorCoil3Port, cmbInductorCoil4Port, cmbInfraredSensor1Port, cmbInfraredSensor2Port, cmbGate1UpPort, cmbGate1DownPort, cmbGate2UpPort, cmbGate2DownPort, cmbSignalLight1Port, cmbSignalLight2Port);
        }

        private void FrmSetting_Load(object sender, EventArgs e)
        {
            InitComPortComboBox(cmbWberCom);
            InitBandrateComboBox(cmbWberBandrate);
            InitNumberAscComboBox(1, 8, cmbWberStopBits);
            InitStopBitsComboBox(cmbWberDataBits);
            InitParityComboBox(cmbWberParity);
        }

        private void FrmSetting_Shown(object sender, EventArgs e)
        {
            InitForm();

            LoadAppConfig();
        }

        /// <summary>
        /// 测试数据库连接
        /// </summary>
        /// <returns></returns>
        private bool TestDBConnect()
        {
            if (string.IsNullOrEmpty(txtSelfConnStr.Text.Trim()))
            {
                MessageBoxEx.Show("请先输入数据库连接字符串", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            try
            {
                OracleDapperDber dber = new OracleDapperDber(txtSelfConnStr.Text.Trim());
                dber.Connection.Open();
                dber.Connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("数据库连接失败，" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        void LoadAppConfig()
        {
            txtAppIdentifier.Text = commonAppConfig.AppIdentifier;
            txtSelfConnStr.Text = commonAppConfig.SelfConnStr;
            chbStartup.Checked = (commonDAO.GetAppletConfigString("开机启动") == "1");
            chbPrint.Checked = (commonDAO.GetAppletConfigString("连接打印机") == "1");
            // IO控制器
            SelectedComboBoxItem(cmbInductorCoil1Port, commonDAO.GetAppletConfigInt32("IO控制器_地感1端口").ToString());
            SelectedComboBoxItem(cmbInductorCoil2Port, commonDAO.GetAppletConfigInt32("IO控制器_地感2端口").ToString());
            SelectedComboBoxItem(cmbInductorCoil3Port, commonDAO.GetAppletConfigInt32("IO控制器_地感3端口").ToString());
            SelectedComboBoxItem(cmbInductorCoil4Port, commonDAO.GetAppletConfigInt32("IO控制器_地感4端口").ToString());
            SelectedComboBoxItem(cmbInfraredSensor1Port, commonDAO.GetAppletConfigInt32("IO控制器_对射1端口").ToString());
            chkUseInfraredSensor1.Checked = commonDAO.GetAppletConfigString("IO控制器_对射1启用") == "1";
            SelectedComboBoxItem(cmbInfraredSensor2Port, commonDAO.GetAppletConfigInt32("IO控制器_对射2端口").ToString());
            chkUseInfraredSensor2.Checked = commonDAO.GetAppletConfigString("IO控制器_对射2启用") == "1";
            SelectedComboBoxItem(cmbGate1UpPort, commonDAO.GetAppletConfigInt32("IO控制器_道闸1升杆端口").ToString());
            SelectedComboBoxItem(cmbGate1DownPort, commonDAO.GetAppletConfigInt32("IO控制器_道闸1降杆端口").ToString());
            SelectedComboBoxItem(cmbGate2UpPort, commonDAO.GetAppletConfigInt32("IO控制器_道闸2升杆端口").ToString());
            SelectedComboBoxItem(cmbGate2DownPort, commonDAO.GetAppletConfigInt32("IO控制器_道闸2降杆端口").ToString());
            SelectedComboBoxItem(cmbSignalLight1Port, commonDAO.GetAppletConfigInt32("IO控制器_信号灯1端口").ToString());
            SelectedComboBoxItem(cmbSignalLight2Port, commonDAO.GetAppletConfigInt32("IO控制器_信号灯2端口").ToString());

            // 地磅仪表
            SelectedComboBoxItem(cmbWberCom, commonDAO.GetAppletConfigInt32("地磅仪表_串口").ToString());
            SelectedComboBoxItem(cmbWberBandrate, commonDAO.GetAppletConfigInt32("地磅仪表_波特率").ToString());
            SelectedComboBoxItem(cmbWberDataBits, commonDAO.GetAppletConfigInt32("地磅仪表_数据位").ToString());
            SelectedComboBoxItem(cmbWberStopBits, commonDAO.GetAppletConfigInt32("地磅仪表_停止位").ToString());
            SelectedComboBoxItem(cmbWberParity, commonDAO.GetAppletConfigInt32("地磅仪表_校验位").ToString());
            dbtxtMinWeight.Value = commonDAO.GetAppletConfigDouble("地磅仪表_最小称重");
            dbtxtSteady.Value = commonDAO.GetAppletConfigDouble("地磅仪表_稳定时间");
            // 读卡器
            ipRW1.Value = commonDAO.GetAppletConfigString("读卡器1_Ip");
            txtIpCom1.Text = commonDAO.GetAppletConfigString("读卡器1_端口");
            ipRW2.Value = commonDAO.GetAppletConfigString("读卡器2_Ip");
            txtIpCom2.Text = commonDAO.GetAppletConfigString("读卡器2_端口");

            // LED显示屏
            iptxtLED1IP.Value = commonDAO.GetAppletConfigString("LED显示屏_IP地址");
            txtLEDPort.Text = commonDAO.GetAppletConfigString("LED显示屏_端口");
            chb_UseLED.Checked = commonDAO.GetAppletConfigString("启用LED显示屏") == "1";
            iptxt_CyjLed.Value = commonDAO.GetAppletConfigString("采样机LED显示屏_IP地址");
            txt_LeftLedContent.Text = commonDAO.GetCommonAppletConfigString("采样机LED显示屏_左内容");
            txt_RightLedContent.Text = commonDAO.GetCommonAppletConfigString("采样机LED显示屏_右内容");
            //视频
            iptxtCameraIP.Value = commonDAO.GetAppletConfigString("刻录机IP");
            txtCamera_UserName.Text = commonDAO.GetAppletConfigString("刻录机用户名");
            txtCamera_PassWord.Text = commonDAO.GetAppletConfigString("刻录机密码");
            txtCamera_Port.Text = commonDAO.GetAppletConfigString("刻录机端口号");
            txtCTChannel.Text = commonDAO.GetAppletConfigString("刻录机通道号1");
            txtCZChannel.Text = commonDAO.GetAppletConfigString("刻录机通道号2");
            txtCWChannel.Text = commonDAO.GetAppletConfigString("刻录机通道号3");
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        bool SaveAppConfig()
        {
            if (!TestDBConnect()) return false;

            commonAppConfig.AppIdentifier = txtAppIdentifier.Text.Trim();
            commonAppConfig.SelfConnStr = txtSelfConnStr.Text;
            commonAppConfig.Save();
            commonDAO.SetAppletConfig("开机启动", Convert.ToInt16(chbStartup.Checked).ToString());
            commonDAO.SetAppletConfig("连接打印机", Convert.ToInt16(chbPrint.Checked).ToString());

            try
            {
#if DEBUG

#else
                // 添加、取消开机启动
                if (chbStartup.Checked)
                    StartUpUtil.InsertStartUp(Application.ProductName, Application.ExecutablePath);
                else
                    StartUpUtil.DeleteStartUp(Application.ProductName);
#endif
            }
            catch { }

            // IO控制器
            commonDAO.SetAppletConfig("IO控制器_地感1端口", (cmbInductorCoil1Port.SelectedItem as DataItem).Value);
            commonDAO.SetAppletConfig("IO控制器_地感2端口", (cmbInductorCoil2Port.SelectedItem as DataItem).Value);
            commonDAO.SetAppletConfig("IO控制器_地感3端口", (cmbInductorCoil3Port.SelectedItem as DataItem).Value);
            commonDAO.SetAppletConfig("IO控制器_地感4端口", (cmbInductorCoil4Port.SelectedItem as DataItem).Value);
            commonDAO.SetAppletConfig("IO控制器_对射1端口", (cmbInfraredSensor1Port.SelectedItem as DataItem).Value);
            commonDAO.SetAppletConfig("IO控制器_对射1启用", chkUseInfraredSensor1.Checked ? "1" : "0");
            commonDAO.SetAppletConfig("IO控制器_对射2端口", (cmbInfraredSensor2Port.SelectedItem as DataItem).Value);
            commonDAO.SetAppletConfig("IO控制器_对射2启用", chkUseInfraredSensor2.Checked ? "1" : "0");
            commonDAO.SetAppletConfig("IO控制器_道闸1升杆端口", (cmbGate1UpPort.SelectedItem as DataItem).Value);
            commonDAO.SetAppletConfig("IO控制器_道闸1降杆端口", (cmbGate1DownPort.SelectedItem as DataItem).Value);
            commonDAO.SetAppletConfig("IO控制器_道闸2升杆端口", (cmbGate2UpPort.SelectedItem as DataItem).Value);
            commonDAO.SetAppletConfig("IO控制器_道闸2降杆端口", (cmbGate2DownPort.SelectedItem as DataItem).Value);
            commonDAO.SetAppletConfig("IO控制器_信号灯1端口", (cmbSignalLight1Port.SelectedItem as DataItem).Value);
            commonDAO.SetAppletConfig("IO控制器_信号灯2端口", (cmbSignalLight2Port.SelectedItem as DataItem).Value);

            // 地磅仪表
            commonDAO.SetAppletConfig("地磅仪表_串口", (cmbWberCom.SelectedItem as DataItem).Value);
            commonDAO.SetAppletConfig("地磅仪表_波特率", (cmbWberBandrate.SelectedItem as DataItem).Value);
            commonDAO.SetAppletConfig("地磅仪表_数据位", (cmbWberDataBits.SelectedItem as DataItem).Value);
            commonDAO.SetAppletConfig("地磅仪表_停止位", (cmbWberStopBits.SelectedItem as DataItem).Value);
            commonDAO.SetAppletConfig("地磅仪表_校验位", (cmbWberParity.SelectedItem as DataItem).Value);
            commonDAO.SetAppletConfig("地磅仪表_最小称重", dbtxtMinWeight.Value.ToString());
            commonDAO.SetAppletConfig("地磅仪表_稳定时间", dbtxtSteady.Value.ToString());
            // 读卡器
            commonDAO.SetAppletConfig("读卡器1_Ip", ipRW1.Value);
            commonDAO.SetAppletConfig("读卡器1_端口", txtIpCom1.Text);
            commonDAO.SetAppletConfig("读卡器2_Ip", ipRW2.Value);
            commonDAO.SetAppletConfig("读卡器2_端口", txtIpCom2.Text);
            // LED显示屏
            commonDAO.SetAppletConfig("LED显示屏_IP地址", iptxtLED1IP.Value);
            commonDAO.SetAppletConfig("LED显示屏_端口", txtLEDPort.Text);
            commonDAO.SetAppletConfig("启用LED显示屏", chb_UseLED.Checked ? "1" : "0");
            commonDAO.SetAppletConfig("采样机LED显示屏_IP地址", iptxt_CyjLed.Value);
            commonDAO.SetCommonAppletConfig("采样机LED显示屏_左内容", txt_LeftLedContent.Text);
            commonDAO.SetCommonAppletConfig("采样机LED显示屏_右内容", txt_RightLedContent.Text);
            //视频
            commonDAO.SetAppletConfig("刻录机IP", iptxtCameraIP.Value);
            commonDAO.SetAppletConfig("刻录机用户名", txtCamera_UserName.Text);
            commonDAO.SetAppletConfig("刻录机密码", txtCamera_PassWord.Text);
            commonDAO.SetAppletConfig("刻录机端口号", txtCamera_Port.Text);
            commonDAO.SetAppletConfig("刻录机通道号1", txtCTChannel.Text);
            commonDAO.SetAppletConfig("刻录机通道号2", txtCZChannel.Text);
            commonDAO.SetAppletConfig("刻录机通道号3", txtCWChannel.Text);

            return true;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ValidateInputEmpty(new List<string> { "程序唯一标识符", "数据库连接字符串" }, new List<Control> { txtAppIdentifier, txtSelfConnStr })) return;

            if (!SaveAppConfig()) return;

            if (MessageBoxEx.Show("更改的配置需要重启程序才能生效，是否立刻重启？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Restart();
            else
                this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 其他函数

        /// <summary>
        /// 验证批量控件为空，并提示
        /// </summary>
        /// <param name="tipsNames"></param>
        /// <param name="controls"></param>
        /// <returns></returns>
        public static bool ValidateInputEmpty(List<string> tipsNames, List<Control> controls)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                Control control = controls[i];

                if (control is TextBoxX && string.IsNullOrEmpty(((TextBoxX)control).Text))
                {
                    control.Focus();
                    MessageBoxEx.Show("请输入" + tipsNames[i] + "！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 选中下拉框选项
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="text"></param>
        private void SelectedComboBoxItem(ComboBoxEx cmb, string value)
        {
            foreach (DataItem dataItem in cmb.Items)
            {
                if (dataItem.Value == value) cmb.SelectedItem = dataItem;
            }
        }

        /// <summary>
        /// 初始化串口下拉框
        /// </summary>
        /// <param name="cmb"></param>
        void InitComPortComboBox(ComboBoxEx cmb)
        {
            cmb.Items.Clear();

            cmb.DisplayMember = "Text";
            cmb.ValueMember = "Value";

            for (int i = 1; i < 20; i++)
            {
                cmb.Items.Add(new DataItem("COM" + i.ToString(), i.ToString()));
            }

            cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化串口下拉框
        /// </summary>
        /// <param name="cmbs"></param>
        void InitComPortComboBoxs(params ComboBoxEx[] cmbs)
        {
            foreach (ComboBoxEx cmb in cmbs)
            {
                InitComPortComboBox(cmb);
            }
        }

        /// <summary>
        /// 初始化波特率下拉框
        /// </summary>
        /// <param name="cmb"></param>
        private void InitBandrateComboBox(ComboBoxEx cmb)
        {
            cmb.Items.Clear();

            cmb.DisplayMember = "Text";
            cmb.ValueMember = "Value";

            cmb.Items.Add(new DataItem("1200"));
            cmb.Items.Add(new DataItem("4800"));
            cmb.Items.Add(new DataItem("9600"));
            cmb.Items.Add(new DataItem("14400"));
            cmb.Items.Add(new DataItem("19200"));
            cmb.Items.Add(new DataItem("38400"));
            cmb.Items.Add(new DataItem("56000"));
            cmb.Items.Add(new DataItem("57600"));
            cmb.Items.Add(new DataItem("115200"));

            cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化波特率下拉框
        /// </summary>
        /// <param name="cmbs"></param>
        void InitBandrateComboBoxs(params ComboBoxEx[] cmbs)
        {
            foreach (ComboBoxEx cmb in cmbs)
            {
                InitBandrateComboBox(cmb);
            }
        }

        /// <summary>
        /// 初始化数字下拉框
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        void InitNumberAscComboBox(int start, int end, ComboBoxEx cmb)
        {
            cmb.Items.Clear();

            cmb.DisplayMember = "Text";
            cmb.ValueMember = "Value";

            for (int i = start; i <= end; i++)
            {
                cmb.Items.Add(new DataItem(i.ToString()));
            }

            if (cmb.Items.Count > 0) cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化数字下拉框
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        void InitNumberAscComboBoxs(int start, int end, params ComboBoxEx[] cmbs)
        {
            foreach (ComboBoxEx cmb in cmbs)
            {
                InitNumberAscComboBox(start, end, cmb);
            }
        }

        /// <summary>
        /// 初始化停止位下拉框
        /// </summary>
        /// <param name="cmb"></param>
        void InitStopBitsComboBox(ComboBoxEx cmb)
        {
            cmb.Items.Clear();

            cmb.DisplayMember = "Text";
            cmb.ValueMember = "Value";

            cmb.Items.Add(new DataItem(StopBits.None.ToString(), ((int)StopBits.None).ToString()));
            cmb.Items.Add(new DataItem(StopBits.One.ToString(), ((int)StopBits.One).ToString()));
            cmb.Items.Add(new DataItem(StopBits.OnePointFive.ToString(), ((int)StopBits.OnePointFive).ToString()));
            cmb.Items.Add(new DataItem(StopBits.Two.ToString(), ((int)StopBits.Two).ToString()));

            cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化停止位下拉框
        /// </summary>
        /// <param name="cmbs"></param>
        void InitStopBitsComboBoxs(params ComboBoxEx[] cmbs)
        {
            foreach (ComboBoxEx cmb in cmbs)
            {
                InitStopBitsComboBox(cmb);
            }
        }

        /// <summary>
        /// 初始化校验位下拉框
        /// </summary>
        /// <param name="cmb"></param>
        void InitParityComboBox(ComboBoxEx cmb)
        {
            cmb.Items.Clear();

            cmb.DisplayMember = "Text";
            cmb.ValueMember = "Value";

            cmb.Items.Add(new DataItem(Parity.None.ToString(), ((int)Parity.None).ToString()));
            cmb.Items.Add(new DataItem(Parity.Odd.ToString(), ((int)Parity.Odd).ToString()));
            cmb.Items.Add(new DataItem(Parity.Even.ToString(), ((int)Parity.Even).ToString()));
            cmb.Items.Add(new DataItem(Parity.Mark.ToString(), ((int)Parity.Mark).ToString()));
            cmb.Items.Add(new DataItem(Parity.Space.ToString(), ((int)Parity.Space).ToString()));

            cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化校验位下拉框
        /// </summary>
        /// <param name="cmbs"></param>
        void InitParityComboBoxs(params ComboBoxEx[] cmbs)
        {
            foreach (ComboBoxEx cmb in cmbs)
            {
                InitParityComboBox(cmb);
            }
        }

        #endregion
    }
}