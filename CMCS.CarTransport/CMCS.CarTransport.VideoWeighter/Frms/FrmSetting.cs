using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using System.IO.Ports;
using CMCS.Common.DAO;
using CMCS.Common;
using CMCS.DapperDber.Dbs.OracleDb;
using CMCS.Common.Utilities;
using CMCS.CarTransport.VideoWeighter.Core;
using CMCS.Common.Entities.BaseInfo;
using CMCS.CarTransport.DAO;

namespace CMCS.CarTransport.VideoWeighter.Frms
{
    public partial class FrmSetting : DevComponents.DotNetBar.Metro.MetroForm
    {
        CommonDAO commonDAO = CommonDAO.GetInstance();
        VideoDAO videoDAO = VideoDAO.GetInstance();
        CommonAppConfig commonAppConfig = CommonAppConfig.GetInstance();
        /// <summary>
        /// 新增 修改 标识
        /// </summary>
        string strEditMode = string.Empty;
        /// <summary>
        /// 选中的实体
        /// </summary>
        public CmcsCamare Output;

        public FrmSetting()
        {
            InitializeComponent();
        }

        void InitForm()
        {
            InitComPortComboBoxs(cmbIocerCom, cmbRwer1Com, cmbRwer2Com);
            InitBandrateComboBoxs(cmbIocerBandrate);
            InitNumberAscComboBoxs(5, 8, cmbIocerDataBits);
            InitNumberAscComboBoxs(1, 15, cmbInductorCoil1Port, cmbInductorCoil2Port, cmbInductorCoil3Port, cmbInductorCoil4Port, cmbGate1UpPort, cmbGate1DownPort, cmbGate2UpPort, cmbGate2DownPort, cmbSignalLight1Port, cmbSignalLight2Port);
            InitStopBitsComboBoxs(cmbIocerStopBits);
            InitParityComboBoxs(cmbIocerParity);
        }

        private void FrmSetting_Load(object sender, EventArgs e)
        {

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

            //一号汽车衡
            iptxtCameraIP.Value = commonDAO.GetAppletConfigString("刻录机IP");
            txtCamera_UserName.Text = commonDAO.GetAppletConfigString("刻录机用户名");
            txtCamera_PassWord.Text = commonDAO.GetAppletConfigString("刻录机密码");
            txtCamera_Port.Text = commonDAO.GetAppletConfigString("刻录机端口号");
            txtCTChannel.Text = commonDAO.GetAppletConfigString("刻录机通道号1");
            txtCZChannel.Text = commonDAO.GetAppletConfigString("刻录机通道号2");
            txtCWChannel.Text = commonDAO.GetAppletConfigString("刻录机通道号3");

            //二号汽车衡
            iptxtCameraIP2.Value = commonDAO.GetAppletConfigString("刻录机2IP");
            txtCamera_UserName2.Text = commonDAO.GetAppletConfigString("刻录机2用户名");
            txtCamera_PassWord2.Text = commonDAO.GetAppletConfigString("刻录机2密码");
            txtCamera_Port2.Text = commonDAO.GetAppletConfigString("刻录机2端口号");
            txtCTChannel2.Text = commonDAO.GetAppletConfigString("刻录机2通道号1");
            txtCZChannel2.Text = commonDAO.GetAppletConfigString("刻录机2通道号2");
            txtCWChannel2.Text = commonDAO.GetAppletConfigString("刻录机2通道号3");

            //三号汽车衡
            iptxtCameraIP3.Value = commonDAO.GetAppletConfigString("刻录机3IP");
            txtCamera_UserName3.Text = commonDAO.GetAppletConfigString("刻录机3用户名");
            txtCamera_PassWord3.Text = commonDAO.GetAppletConfigString("刻录机3密码");
            txtCamera_Port3.Text = commonDAO.GetAppletConfigString("刻录机3端口号");
            txtCTChannel3.Text = commonDAO.GetAppletConfigString("刻录机3通道号1");
            txtCZChannel3.Text = commonDAO.GetAppletConfigString("刻录机3通道号2");
            txtCWChannel3.Text = commonDAO.GetAppletConfigString("刻录机3通道号3");

            //四号汽车衡
            iptxtCameraIP4.Value = commonDAO.GetAppletConfigString("刻录机4IP");
            txtCamera_UserName4.Text = commonDAO.GetAppletConfigString("刻录机4用户名");
            txtCamera_PassWord4.Text = commonDAO.GetAppletConfigString("刻录机4密码");
            txtCamera_Port4.Text = commonDAO.GetAppletConfigString("刻录机4端口号");
            txtCTChannel4.Text = commonDAO.GetAppletConfigString("刻录机4通道号1");
            txtCZChannel4.Text = commonDAO.GetAppletConfigString("刻录机4通道号2");
            txtCWChannel4.Text = commonDAO.GetAppletConfigString("刻录机4通道号3");
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
            #region 保存配置
            //一号汽车衡
            commonDAO.SetAppletConfig("刻录机IP", iptxtCameraIP.Value);
            commonDAO.SetAppletConfig("刻录机用户名", txtCamera_UserName.Text);
            commonDAO.SetAppletConfig("刻录机密码", txtCamera_PassWord.Text);
            commonDAO.SetAppletConfig("刻录机端口号", txtCamera_Port.Text);
            commonDAO.SetAppletConfig("刻录机通道号1", txtCTChannel.Text);
            commonDAO.SetAppletConfig("刻录机通道号2", txtCZChannel.Text);
            commonDAO.SetAppletConfig("刻录机通道号3", txtCWChannel.Text);

            //二号汽车衡
            commonDAO.SetAppletConfig("刻录机2IP", iptxtCameraIP2.Value);
            commonDAO.SetAppletConfig("刻录机2用户名", txtCamera_UserName2.Text);
            commonDAO.SetAppletConfig("刻录机2密码", txtCamera_PassWord2.Text);
            commonDAO.SetAppletConfig("刻录机2端口号", txtCamera_Port2.Text);
            commonDAO.SetAppletConfig("刻录机2通道号1", txtCTChannel2.Text);
            commonDAO.SetAppletConfig("刻录机2通道号2", txtCZChannel2.Text);
            commonDAO.SetAppletConfig("刻录机2通道号3", txtCWChannel2.Text);

            //三号汽车衡
            commonDAO.SetAppletConfig("刻录机3IP", iptxtCameraIP3.Value);
            commonDAO.SetAppletConfig("刻录机3用户名", txtCamera_UserName3.Text);
            commonDAO.SetAppletConfig("刻录机3密码", txtCamera_PassWord3.Text);
            commonDAO.SetAppletConfig("刻录机3端口号", txtCamera_Port3.Text);
            commonDAO.SetAppletConfig("刻录机3通道号1", txtCTChannel3.Text);
            commonDAO.SetAppletConfig("刻录机3通道号2", txtCZChannel3.Text);
            commonDAO.SetAppletConfig("刻录机3通道号3", txtCWChannel3.Text);

            //四号汽车衡
            commonDAO.SetAppletConfig("刻录机4IP", iptxtCameraIP4.Value);
            commonDAO.SetAppletConfig("刻录机4用户名", txtCamera_UserName4.Text);
            commonDAO.SetAppletConfig("刻录机4密码", txtCamera_PassWord4.Text);
            commonDAO.SetAppletConfig("刻录机4端口号", txtCamera_Port4.Text);
            commonDAO.SetAppletConfig("刻录机4通道号1", txtCTChannel4.Text);
            commonDAO.SetAppletConfig("刻录机4通道号2", txtCZChannel4.Text);
            commonDAO.SetAppletConfig("刻录机4通道号3", txtCWChannel4.Text);

            #endregion
            return true;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ValidateInputEmpty(new List<string> { "程序唯一标识符", "数据库连接字符串" }, new List<Control> { txtAppIdentifier, txtSelfConnStr })) return;

            if (!SaveAppConfig()) return;
            FrmSetting_Load(null, null);
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