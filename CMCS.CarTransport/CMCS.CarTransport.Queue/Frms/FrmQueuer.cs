using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//
using DevComponents.DotNetBar;
using CMCS.CarTransport.Queue.Core;
using CMCS.CarTransport.DAO;
using CMCS.Common.DAO;
using CMCS.Common.Utilities;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common;
using CMCS.CarTransport.Queue.Frms.Sys;
using DevComponents.DotNetBar.Controls;
using CMCS.Common.Views;
using DevComponents.DotNetBar.SuperGrid;
using CMCS.Common.Enums;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.Fuel;
using CMCS.CarTransport.Queue.Frms.BaseInfo.Autotruck;
using System.Text;


namespace CMCS.CarTransport.Queue.Frms
{
    public partial class FrmQueuer : DevComponents.DotNetBar.Metro.MetroForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmQueuer";

        public FrmQueuer()
        {
            InitializeComponent();
        }

        #region Vars

        CarTransportDAO carTransportDAO = CarTransportDAO.GetInstance();
        QueuerDAO queuerDAO = QueuerDAO.GetInstance();
        CommonDAO commonDAO = CommonDAO.GetInstance();

        /// <summary>
        /// 语音播报
        /// </summary>
        VoiceSpeaker voiceSpeaker = new VoiceSpeaker();

        public static PassCarQueuer passCarQueuer = new PassCarQueuer();

        ImperfectCar currentImperfectCar;
        /// <summary>
        /// 识别或选择的车辆凭证
        /// </summary>
        public ImperfectCar CurrentImperfectCar
        {
            get { return currentImperfectCar; }
            set
            {
                currentImperfectCar = value;
            }
        }

        CmcsAutotruck currentAutotruck;
        /// <summary>
        /// 当前车
        /// </summary>
        public CmcsAutotruck CurrentAutotruck
        {
            get { return currentAutotruck; }
            set
            {
                currentAutotruck = value;
                if (value != null)
                {
                    if (this.superTabControl2.SelectedTab == this.superTabItem_BuyFuel)
                        this.txtCarNumber_BuyFuel.Text = value.CarNumber;
                    else if (this.superTabControl2.SelectedTab == this.superTabItem_Goods)
                        this.txtCarNumber_Goods.Text = value.CarNumber;
                }
            }
        }

        #endregion

        /// <summary>
        /// 窗体初始化
        /// </summary>
        private void InitForm()
        {
#if DEBUG
            //FrmDebugConsole.GetInstance().Show();
#else

#endif

            LoadFuelkind(cmbFuelName_BuyFuel);
            LoadQCH(cmbQch_BuyFuel);
            LoadQCH(cmbQch_Goods);
            LoadUnLoader(cmbUnLoader_BuyFuel);
            LoadUnLoadType(cmbUnloadType);
            //this.SelectedMine_BuyFuel = carTransportDAO.GetDefaultMine();
            this.SelectedSupplier_BuyFuel = carTransportDAO.GetDefaultSupplier();
            this.SelectedFuelKind_BuyFuel = carTransportDAO.GetDefaultFuelKind();
        }

        private void FrmQueuer_Load(object sender, EventArgs e)
        {
        }

        private void FrmQueuer_Shown(object sender, EventArgs e)
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

        #region 读卡器

        void Rwer_OnScanError(Exception ex)
        {
            Log4Neter.Error("读卡器", ex);
        }

        void Rwer_OnStatusChange(bool status)
        {
            // 接收读卡器状态 
            InvokeEx(() =>
            {
                slightRwer1.LightColor = (status ? Color.Green : Color.Red);

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.读卡器_连接状态.ToString(), status ? "1" : "0");
            });
        }

        /// <summary>
        /// 读卡器接收数据
        /// </summary>
        /// <param name="rfid"></param>
        void Rwer_OnReadSuccess(string rfid)
        {
            InvokeEx(() =>
            {
                if (this.superTabControl2.SelectedTab == this.superTabItem_BuyFuel)
                    txtTagId_BuyFuel.Text = rfid;
                else if (this.superTabControl2.SelectedTab == this.superTabItem_Goods)
                    txtTagId_Goods.Text = rfid;
            });
        }
        /// <summary>
        /// 读卡器错误信息
        /// </summary>
        /// <param name="error"></param>
        void Rwer_OnScanError(string error)
        {
            Log4Neter.Info(string.Format("读卡器错误:{0}", error));
        }
        #endregion

        #region LED

        void LedListen1_OnStatusChange(bool status)
        {
            // 接收读卡器状态 
            InvokeEx(() =>
            {
                slightLED1.LightColor = (status ? Color.Green : Color.Red);

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.卸煤沟LED屏1_连接状态.ToString(), status ? "1" : "0");
            });
        }
        void LedListen1_OnScanError(Exception error)
        {
            Log4Neter.Error("卸煤沟LED屏1", error);
        }


        void LedListen2_OnStatusChange(bool status)
        {
            // 接收读卡器状态 
            InvokeEx(() =>
            {
                slightLED2.LightColor = (status ? Color.Green : Color.Red);

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.卸煤沟LED屏2_连接状态.ToString(), status ? "1" : "0");
            });
        }
        void LedListen2_OnScanError(Exception error)
        {
            Log4Neter.Error("卸煤沟LED屏2", error);
        }

        void LedListen3_OnStatusChange(bool status)
        {
            // 接收读卡器状态 
            InvokeEx(() =>
            {
                slightLED3.LightColor = (status ? Color.Green : Color.Red);

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.卸煤沟LED屏3_连接状态.ToString(), status ? "1" : "0");
            });
        }
        void LedListen3_OnScanError(Exception error)
        {
            Log4Neter.Error("卸煤沟LED屏3", error);
        }

        void LedListen4_OnStatusChange(bool status)
        {
            // 接收读卡器状态 
            InvokeEx(() =>
            {
                slightLED4.LightColor = (status ? Color.Green : Color.Red);

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.卸煤沟LED屏4_连接状态.ToString(), status ? "1" : "0");
            });
        }
        void LedListen4_OnScanError(Exception error)
        {
            Log4Neter.Error("卸煤沟LED屏4", error);
        }

        void LedListen5_OnStatusChange(bool status)
        {
            // 接收读卡器状态 
            InvokeEx(() =>
            {
                slightLED5.LightColor = (status ? Color.Green : Color.Red);

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.卸煤沟LED屏5_连接状态.ToString(), status ? "1" : "0");
            });
        }
        void LedListen5_OnScanError(Exception error)
        {
            Log4Neter.Error("卸煤沟LED屏5", error);
        }

        void LedListen6_OnStatusChange(bool status)
        {
            // 接收读卡器状态 
            InvokeEx(() =>
            {
                slightLED6.LightColor = (status ? Color.Green : Color.Red);

                commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.卸煤沟LED屏6_连接状态.ToString(), status ? "6" : "0");
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

                // 读卡器
                Hardwarer.Rwer.OnStatusChange += new RW.HawkvorCom.HawkvorComRwer.StatusChangeHandler(Rwer_OnStatusChange);
                Hardwarer.Rwer.OnReadSucess += new RW.HawkvorCom.HawkvorComRwer.ReadSucessHandler(Rwer_OnReadSuccess);
                Hardwarer.Rwer.OnScanError += new RW.HawkvorCom.HawkvorComRwer.ScanErrorEventHandler(Rwer_OnScanError);
                success = Hardwarer.Rwer.OpenCom(commonDAO.GetAppletConfigInt32("读卡器_串口"), commonDAO.GetAppletConfigInt32("读卡器_波特率"));
                if (!success) MessageBoxEx.Show("读卡器连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //LED
                //Hardwarer.LedListen1.OnStatusChange += new LED.Listen.LEDListenAreaLeder.StatusChangeHandler(LedListen1_OnStatusChange);
                //Hardwarer.LedListen1.OnScanError += new LED.Listen.LEDListenAreaLeder.ScanErrorEventHandler(LedListen1_OnScanError);
                //success = Hardwarer.LedListen1.Init(commonDAO.GetAppletConfigString("LED1_IP"));
                //if (!success) MessageBoxEx.Show("Led屏1连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //Hardwarer.LedListen2.OnStatusChange += new LED.Listen.LEDListenAreaLeder.StatusChangeHandler(LedListen2_OnStatusChange);
                //Hardwarer.LedListen2.OnScanError += new LED.Listen.LEDListenAreaLeder.ScanErrorEventHandler(LedListen2_OnScanError);
                //success = Hardwarer.LedListen2.Init(commonDAO.GetAppletConfigString("LED2_IP"));
                //if (!success) MessageBoxEx.Show("Led屏2连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //Hardwarer.LedListen3.OnStatusChange += new LED.Listen.LEDListenAreaLeder.StatusChangeHandler(LedListen3_OnStatusChange);
                //Hardwarer.LedListen3.OnScanError += new LED.Listen.LEDListenAreaLeder.ScanErrorEventHandler(LedListen3_OnScanError);
                //success = Hardwarer.LedListen3.Init(commonDAO.GetAppletConfigString("LED3_IP"));
                //if (!success) MessageBoxEx.Show("Led屏3连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //Hardwarer.LedListen4.OnStatusChange += new LED.Listen.LEDListenAreaLeder.StatusChangeHandler(LedListen4_OnStatusChange);
                //Hardwarer.LedListen4.OnScanError += new LED.Listen.LEDListenAreaLeder.ScanErrorEventHandler(LedListen4_OnScanError);
                //success = Hardwarer.LedListen4.Init(commonDAO.GetAppletConfigString("LED4_IP"));
                //if (!success) MessageBoxEx.Show("Led屏4连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //Hardwarer.LedListen5.OnStatusChange += new LED.Listen.LEDListenAreaLeder.StatusChangeHandler(LedListen5_OnStatusChange);
                //Hardwarer.LedListen5.OnScanError += new LED.Listen.LEDListenAreaLeder.ScanErrorEventHandler(LedListen5_OnScanError);
                //success = Hardwarer.LedListen5.Init(commonDAO.GetAppletConfigString("LED5_IP"));
                //if (!success) MessageBoxEx.Show("Led屏5连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //Hardwarer.LedListen6.OnStatusChange += new LED.Listen.LEDListenAreaLeder.StatusChangeHandler(LedListen6_OnStatusChange);
                //Hardwarer.LedListen6.OnScanError += new LED.Listen.LEDListenAreaLeder.ScanErrorEventHandler(LedListen6_OnScanError);
                //success = Hardwarer.LedListen6.Init(commonDAO.GetAppletConfigString("LED6_IP"));
                //if (!success) MessageBoxEx.Show("Led屏6连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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

            try
            {
                Hardwarer.Rwer.CloseCom();
            }
            catch { }

        }

        #endregion

        #endregion

        #region 公共业务

        /// <summary>
        /// 慢速任务 刷新列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            // 三分钟执行一次
            timer2.Interval = 180000;
            try
            {
                // 入厂煤
                LoadTodayBuyFuelTransport();
                // 其他物资
                LoadTodayGoodsTransport();
            }
            catch (Exception ex)
            {
                Log4Neter.Error("timer2_Tick", ex);
            }
            finally
            {
                timer2.Start();
            }
        }

        /// <summary>
        /// 加载煤种
        /// </summary>
        void LoadFuelkind(ComboBoxEx comboBoxEx)
        {
            comboBoxEx.DisplayMember = "FuelName";
            comboBoxEx.ValueMember = "Id";
            comboBoxEx.DataSource = Dbers.GetInstance().SelfDber.Entities<CmcsFuelKind>("where Valid='有效' and ParentId is not null order by Sequence");
        }
        /// <summary>
        /// 选择煤种
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbFuelName_BuyFuel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedFuelKind_BuyFuel = cmbFuelName_BuyFuel.SelectedItem as CmcsFuelKind;
        }

        /// <summary>
        /// 加载汽车衡
        /// </summary>
        void LoadQCH(ComboBoxEx comboBoxEx)
        {
            comboBoxEx.DisplayMember = "Content";
            comboBoxEx.ValueMember = "Code";
            comboBoxEx.DataSource = commonDAO.GetCodeContentByKind("地磅编号");
            //comboBoxEx.SelectedIndex = 0;
        }

        /// <summary>
        /// 加载卸煤区域
        /// </summary>
        public void LoadUnLoader(ComboBoxEx comboBoxEx)
        {
            comboBoxEx.DisplayMember = "Content";
            comboBoxEx.ValueMember = "Code";
            comboBoxEx.DataSource = commonDAO.GetCodeContentByKind("卸煤区域");
            //comboBoxEx.SelectedIndex = 0;
        }

        /// <summary>
        /// 加载卸煤类型
        /// </summary>
        void LoadUnLoadType(ComboBoxEx comboBoxEx)
        {
            comboBoxEx.DisplayMember = "Content";
            comboBoxEx.ValueMember = "Code";
            comboBoxEx.DataSource = commonDAO.GetCodeContentByKind("卸煤类型");
            //comboBoxEx.SelectedIndex = 0;
        }

        #endregion

        #region 入厂煤业务

        private CmcsSupplier selectedSupplier_BuyFuel;
        /// <summary>
        /// 选择的供煤单位
        /// </summary>
        public CmcsSupplier SelectedSupplier_BuyFuel
        {
            get { return selectedSupplier_BuyFuel; }
            set
            {
                selectedSupplier_BuyFuel = value;

                if (value != null)
                {
                    txtSupplierName_BuyFuel.Text = value.Name;
                }
                else
                {
                    txtSupplierName_BuyFuel.ResetText();
                }
            }
        }

        private CmcsMine selectedMine_BuyFuel;
        /// <summary>
        /// 选择的矿点
        /// </summary>
        public CmcsMine SelectedMine_BuyFuel
        {
            get { return selectedMine_BuyFuel; }
            set
            {
                selectedMine_BuyFuel = value;

                if (value != null)
                {
                    txtMineName_BuyFuel.Text = value.Name;
                    CmcsBuyFuelTransport transport = commonDAO.SelfDber.Entity<CmcsBuyFuelTransport>("where MineId=:MineId and trunc(CreateDate)=trunc(Sysdate) and UnloadArea is not null order by CreateDate desc", new { MineId = value.Id });
                    if (transport != null)
                        cmbUnLoader_BuyFuel.Text = transport.UnloadArea;
                }
                else
                {
                    txtMineName_BuyFuel.ResetText();
                }
            }
        }

        private CmcsFuelKind selectedFuelKind_BuyFuel;
        /// <summary>
        /// 选择的煤种
        /// </summary>
        public CmcsFuelKind SelectedFuelKind_BuyFuel
        {
            get { return selectedFuelKind_BuyFuel; }
            set
            {
                if (value != null)
                {
                    selectedFuelKind_BuyFuel = value;
                    cmbFuelName_BuyFuel.Text = value.FuelName;
                }
                else
                {
                    cmbFuelName_BuyFuel.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// 选择车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAutotruck_BuyFuel_Click(object sender, EventArgs e)
        {
            FrmAutotruck_Select frm = new FrmAutotruck_Select(" and IsUse=1 order by CarNumber asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                passCarQueuer.Enqueue(frm.Output.CarNumber, false);
                this.CurrentImperfectCar = passCarQueuer.Dequeue();
                this.CurrentAutotruck = commonDAO.GetAutotruckByCarNumber(CurrentImperfectCar.Voucher);
            }
        }

        /// <summary>
        /// 选择供煤单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectSupplier_BuyFuel_Click(object sender, EventArgs e)
        {
            FrmSupplier_Select frm = new FrmSupplier_Select("where IsStop='0' order by Name asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedSupplier_BuyFuel = frm.Output;
            }
        }

        /// <summary>
        /// 选择矿点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectMine_BuyFuel_Click(object sender, EventArgs e)
        {
            FrmMine_Select frm = new FrmMine_Select("where Valid='有效' and name like '鹤淇%' and  code is not null order by Name asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedMine_BuyFuel = frm.Output;
                this.cmbUnLoader_BuyFuel.SelectedValue = frm.Output.DisChargeArea;
                txtTicketWeight_BuyFuel.Focus();
            }
        }

        /// <summary>
        /// 新车登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewAutotruck_BuyFuel_Click(object sender, EventArgs e)
        {
            new FrmAutotruck_Oper("", true).Show();
        }

        /// <summary>
        /// 选择入厂煤来煤预报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectForecast_BuyFuel_Click(object sender, EventArgs e)
        {
            FrmBuyFuelForecast_Select frm = new FrmBuyFuelForecast_Select();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedFuelKind_BuyFuel = commonDAO.SelfDber.Get<CmcsFuelKind>(frm.Output.FuelKindId);
                this.SelectedMine_BuyFuel = commonDAO.SelfDber.Get<CmcsMine>(frm.Output.MineId);
                this.SelectedSupplier_BuyFuel = commonDAO.SelfDber.Get<CmcsSupplier>(frm.Output.SupplierId);
            }
        }

        /// <summary>
        /// 保存入厂煤运输记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveTransport_BuyFuel_Click(object sender, EventArgs e)
        {
            SaveBuyFuelTransport();
        }

        /// <summary>
        /// 保存运输记录
        /// </summary>
        /// <returns></returns>
        bool SaveBuyFuelTransport()
        {
            if (this.CurrentAutotruck == null)
            {
                MessageBoxEx.Show("请选择车辆", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrEmpty(this.txtTagId_BuyFuel.Text))
            {
                MessageBoxEx.Show("请刷标签卡", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedFuelKind_BuyFuel == null)
            {
                MessageBoxEx.Show("请选择煤种", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedMine_BuyFuel == null)
            {
                MessageBoxEx.Show("请选择矿点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrEmpty(txtMineName_BuyFuel.Text))
            {
                MessageBoxEx.Show("请填写矿点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedSupplier_BuyFuel == null)
            {
                MessageBoxEx.Show("请选择供煤单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtTicketWeight_BuyFuel.Value <= 0)
            {
                MessageBoxEx.Show("请输入有效的矿发量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrEmpty(this.cmbQch_BuyFuel.Text))
            {
                MessageBoxEx.Show("请选择地磅", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrEmpty(this.cmbUnLoader_BuyFuel.Text))
            {
                MessageBoxEx.Show("请选择卸煤区域", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            try
            {
                bool hasUnFinish = false;

                QueueMessageBox mes = new QueueMessageBox(txtCarNumber_BuyFuel.Text.Trim(), txtTicketWeight_BuyFuel.Value.ToString(), txtMineName_BuyFuel.Text.Trim(), txtSupplierName_BuyFuel.Text.Trim(), cmbFuelName_BuyFuel.Text, cmbQch_BuyFuel.Text.Trim(), cmbUnLoader_BuyFuel.Text.Trim(), cmbUnloadType.Text.Trim());
                if (mes.ShowDialog() == DialogResult.OK)
                {
                    CmcsUnFinishTransport unFinishTransport = carTransportDAO.GetUnFinishTransportByAutotruckId(this.CurrentAutotruck.Id, this.CurrentAutotruck.CarType);
                    if (unFinishTransport != null)
                    {
                        FrmTransport_Confirm frm = new FrmTransport_Confirm(unFinishTransport.TransportId, unFinishTransport.CarType);
                        if (frm.ShowDialog() == DialogResult.Yes)
                        {
                            hasUnFinish = false;
                        }
                        else
                        {
                            hasUnFinish = true;
                        }
                    }
                    else
                    {
                        hasUnFinish = false;
                    }
                    if (!hasUnFinish)
                    {
                        CmcsBuyFuelTransport transport = new CmcsBuyFuelTransport();
                        // 生成入厂煤排队记录，同时生成批次信息以及采制化三级编码
                        if (queuerDAO.JoinQueueBuyFuelTransport(this.CurrentAutotruck, this.SelectedSupplier_BuyFuel, this.SelectedMine_BuyFuel, this.SelectedFuelKind_BuyFuel, (decimal)txtTicketWeight_BuyFuel.Value, this.cmbQch_BuyFuel.Text, this.txtTagId_BuyFuel.Text, this.cmbUnLoader_BuyFuel.Text, DateTime.Now, txtRemark_BuyFuel.Text, CommonAppConfig.GetInstance().AppIdentifier, cmbUnloadType.Text, ref transport))
                        {
                            if (carTransportDAO.InsertDttb_record_preset_weigh(transport))
                            {
                                btnSaveTransport_BuyFuel.Enabled = false;

                                MessageBoxEx.Show("排队成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                LoadTodayBuyFuelTransport();
                                ResetBuyFuel();
                                return true;
                            }
                        }
                    }
                    else
                    {
                        ResetBuyFuel();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("保存失败\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Log4Neter.Error("保存运输记录", ex);
            }

            return false;
        }

        /// <summary>
        /// 重置入厂煤运输记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_BuyFuel_Click(object sender, EventArgs e)
        {
            ResetBuyFuel();
        }

        /// <summary>
        /// 重置信息
        /// </summary>
        void ResetBuyFuel()
        {
            this.CurrentAutotruck = null;
            this.SelectedMine_BuyFuel = null;
            this.SelectedSupplier_BuyFuel = null;

            txtTagId_BuyFuel.ResetText();
            txtTicketWeight_BuyFuel.Value = 0;
            txtRemark_BuyFuel.ResetText();
            cmbUnLoader_BuyFuel.ResetText();
            txtCarNumber_BuyFuel.ResetText();
            cmbQch_BuyFuel.ResetText();
            cmbUnloadType.Text = "人工";
            cmbQch_BuyFuel.ResetText();
            btnSaveTransport_BuyFuel.Enabled = true;

            //this.SelectedMine_BuyFuel = carTransportDAO.GetDefaultMine();
            this.SelectedSupplier_BuyFuel = carTransportDAO.GetDefaultSupplier();
            this.SelectedFuelKind_BuyFuel = carTransportDAO.GetDefaultFuelKind();
            // 最后重置
            this.CurrentImperfectCar = null;
        }

        /// <summary>
        /// 加载运输记录
        /// </summary>
        void LoadTodayBuyFuelTransport()
        {
            //未完成运输记录
            IList<View_BuyFuelTransport> UnFinishlist = carTransportDAO.GetUnFinishBuyFuelTransport("order by InFactoryTime desc");
            superGridControl1_BuyFuel.PrimaryGrid.DataSource = UnFinishlist;
            //指定日期已完成的入厂煤记录
            IList<View_BuyFuelTransport> Finishlist = carTransportDAO.GetFinishedBuyFuelTransport(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
            superGridControl2_BuyFuel.PrimaryGrid.DataSource = Finishlist;

            labNumber.Text = string.Format("已登记：{0}  已称重：{1}  已回皮：{2}  未回皮：{3}", UnFinishlist.Count + Finishlist.Count(), UnFinishlist.Where(a => a.GrossWeight > 0).Count() + Finishlist.Count(), Finishlist.Where(a => a.GrossWeight > 0 && a.TareWeight > 0).Count(), UnFinishlist.Where(a => a.GrossWeight > 0 && a.TareWeight == 0).Count());
        }

        /// <summary>
        /// 提取预报信息
        /// </summary>
        /// <param name="lMYB">来煤预报</param>
        void BorrowForecast_BuyFuel(CmcsLMYB lMYB)
        {
            if (lMYB == null) return;

            this.SelectedFuelKind_BuyFuel = commonDAO.SelfDber.Get<CmcsFuelKind>(lMYB.FuelKindId);
            this.SelectedMine_BuyFuel = commonDAO.SelfDber.Get<CmcsMine>(lMYB.MineId);
            this.SelectedSupplier_BuyFuel = commonDAO.SelfDber.Get<CmcsSupplier>(lMYB.SupplierId);
        }

        #region DataGridView
        /// <summary>
        /// 双击行时，自动填充供煤单位、矿点等信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void superGridControl_BuyFuel_CellDoubleClick(object sender, DevComponents.DotNetBar.SuperGrid.GridCellDoubleClickEventArgs e)
        {
            GridRow gridRow = (sender as SuperGridControl).PrimaryGrid.ActiveRow as GridRow;
            if (gridRow == null) return;

            View_BuyFuelTransport entity = (gridRow.DataItem as View_BuyFuelTransport);
            if (entity != null)
            {
                this.SelectedFuelKind_BuyFuel = commonDAO.SelfDber.Get<CmcsFuelKind>(entity.FuelKindId);
                this.SelectedMine_BuyFuel = commonDAO.SelfDber.Get<CmcsMine>(entity.MineId);
                this.SelectedSupplier_BuyFuel = commonDAO.SelfDber.Get<CmcsSupplier>(entity.SupplierId);
                this.cmbUnLoader_BuyFuel.Text = entity.UnloadArea;
                cmbQch_BuyFuel.Text = entity.Qch;
                cmbUnloadType.Text = entity.UnloadType;
            }
            txtTicketWeight_BuyFuel.Focus();
        }

        private void superGridControl1_BuyFuel_BeginEdit(object sender, DevComponents.DotNetBar.SuperGrid.GridEditEventArgs e)
        {
            // 取消编辑
            e.Cancel = true;
        }
        private void superGridControl1_BuyFuel_CellClick(object sender, GridCellClickEventArgs e)
        {
            View_BuyFuelTransport entity = e.GridCell.GridRow.DataItem as View_BuyFuelTransport;
            if (entity == null) return;

            // 更改有效状态
            if (e.GridCell.GridColumn.Name == "ChangeIsUse") queuerDAO.ChangeBuyFuelTransportToInvalid(entity.Id, Convert.ToBoolean(e.GridCell.Value));
        }

        private void superGridControl1_BuyFuel_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            try
            {
                foreach (GridRow gridRow in e.GridPanel.Rows)
                {
                    View_BuyFuelTransport entity = gridRow.DataItem as View_BuyFuelTransport;
                    if (entity == null) return;

                    // 填充有效状态
                    gridRow.Cells["ChangeIsUse"].Value = Convert.ToBoolean(entity.IsUse);
                    gridRow.Cells["clmKzWeight"].Value = entity.KsWeight + entity.KgWeight;
                    gridRow.Cells["clmKsWeight"].Value = entity.AutoKsWeight + entity.KsWeight;
                }
            }
            catch (Exception ex)
            {
                Log4Neter.Error("GridView加载完成事件", ex);
            }
        }

        private void superGridControl2_BuyFuel_CellClick(object sender, GridCellClickEventArgs e)
        {
            View_BuyFuelTransport entity = e.GridCell.GridRow.DataItem as View_BuyFuelTransport;
            if (entity == null) return;

            // 更改有效状态
            if (e.GridCell.GridColumn.Name == "ChangeIsUse") queuerDAO.ChangeBuyFuelTransportToInvalid(entity.Id, Convert.ToBoolean(e.GridCell.Value));
        }

        private void superGridControl2_BuyFuel_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            foreach (GridRow gridRow in e.GridPanel.Rows)
            {
                View_BuyFuelTransport entity = gridRow.DataItem as View_BuyFuelTransport;
                if (entity == null) return;

                // 填充有效状态
                gridRow.Cells["ChangeIsUse"].Value = Convert.ToBoolean(entity.IsUse);
                gridRow.Cells["clmKzWeight"].Value = entity.KsWeight + entity.KgWeight;
                gridRow.Cells["clmKsWeight"].Value = entity.AutoKsWeight + entity.KsWeight;
            }
        }

        private void superGridControl1_BuyFuel_GetRowHeaderText(object sender, GridGetRowHeaderTextEventArgs e)
        {
            e.Text = (e.GridRow.RowIndex + 1).ToString();
        }
        #endregion

        #endregion

        #region 其他物资业务

        private CmcsSupplyReceive selectedSupplyUnit_Goods;
        /// <summary>
        /// 选择的供货单位
        /// </summary>
        public CmcsSupplyReceive SelectedSupplyUnit_Goods
        {
            get { return selectedSupplyUnit_Goods; }
            set
            {
                selectedSupplyUnit_Goods = value;

                if (value != null)
                {
                    txtSupplyUnitName_Goods.Text = value.UnitName;
                }
                else
                {
                    txtSupplyUnitName_Goods.ResetText();
                }
            }
        }

        private CmcsSupplyReceive selectedReceiveUnit_Goods;
        /// <summary>
        /// 选择的收货单位
        /// </summary>
        public CmcsSupplyReceive SelectedReceiveUnit_Goods
        {
            get { return selectedReceiveUnit_Goods; }
            set
            {
                selectedReceiveUnit_Goods = value;

                if (value != null)
                {
                    txtReceiveUnitName_Goods.Text = value.UnitName;
                }
                else
                {
                    txtReceiveUnitName_Goods.ResetText();
                }
            }
        }

        private CmcsGoodsType selectedGoodsType_Goods;
        /// <summary>
        /// 选择的物资类型
        /// </summary>
        public CmcsGoodsType SelectedGoodsType_Goods
        {
            get { return selectedGoodsType_Goods; }
            set
            {
                selectedGoodsType_Goods = value;

                if (value != null)
                {
                    txtGoodsTypeName_Goods.Text = value.GoodsName;
                }
                else
                {
                    txtGoodsTypeName_Goods.ResetText();
                }
            }
        }

        /// <summary>
        /// 选择车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAutotruck_Goods_Click(object sender, EventArgs e)
        {
            FrmAutotruck_Select frm = new FrmAutotruck_Select(" and IsUse=1 order by CarNumber asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                passCarQueuer.Enqueue(frm.Output.CarNumber, false);
                this.CurrentImperfectCar = passCarQueuer.Dequeue();
                this.CurrentAutotruck = commonDAO.GetAutotruckByCarNumber(CurrentImperfectCar.Voucher);
            }
        }

        /// <summary>
        /// 选择供货单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnbtnSelectSupply_Goods_Click(object sender, EventArgs e)
        {
            FrmSupplyReceive_Select frm = new FrmSupplyReceive_Select("where IsValid=1 order by UnitName asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedSupplyUnit_Goods = frm.Output;
            }
        }

        /// <summary>
        /// 选择收货单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectReceive_Goods_Click(object sender, EventArgs e)
        {
            FrmSupplyReceive_Select frm = new FrmSupplyReceive_Select("where IsValid=1 order by UnitName asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedReceiveUnit_Goods = frm.Output;
            }
        }

        /// <summary>
        /// 选择物资类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectGoodsType_Goods_Click(object sender, EventArgs e)
        {
            FrmGoodsType_Select frm = new FrmGoodsType_Select();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.SelectedGoodsType_Goods = frm.Output;
            }
        }

        /// <summary>
        /// 新车登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewAutotruck_Goods_Click(object sender, EventArgs e)
        {
            // eCarType.其他物资 

            new FrmAutotruck_Oper().Show();
        }

        /// <summary>
        /// 保存排队记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveTransport_Goods_Click(object sender, EventArgs e)
        {
            SaveGoodsTransport();
        }

        /// <summary>
        /// 保存运输记录
        /// </summary>
        /// <returns></returns>
        bool SaveGoodsTransport()
        {
            if (string.IsNullOrEmpty(this.txtTagId_Goods.Text))
            {
                MessageBoxEx.Show("请刷标签卡", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.CurrentAutotruck == null)
            {
                MessageBoxEx.Show("请选择车辆", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedSupplyUnit_Goods == null)
            {
                MessageBoxEx.Show("请选择供货单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedReceiveUnit_Goods == null)
            {
                MessageBoxEx.Show("请选择收货单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (this.SelectedGoodsType_Goods == null)
            {
                MessageBoxEx.Show("请选择物资类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrEmpty(this.cmbQch_Goods.Text))
            {
                MessageBoxEx.Show("请选择地磅", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            try
            {
                // 生成排队记录
                if (queuerDAO.JoinQueueGoodsTransport(this.CurrentAutotruck, this.SelectedSupplyUnit_Goods, this.SelectedReceiveUnit_Goods, this.SelectedGoodsType_Goods, DateTime.Now, txtRemark_Goods.Text, CommonAppConfig.GetInstance().AppIdentifier, this.txtTagId_Goods.Text, this.cmbQch_Goods.Text))
                {
                    btnSaveTransport_Goods.Enabled = false;
                    MessageBoxEx.Show("排队成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadTodayGoodsTransport();

                    btnSaveTransport_Goods.Enabled = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("保存失败\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Log4Neter.Error("保存运输记录", ex);
            }

            return false;
        }

        /// <summary>
        /// 重置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Goods_Click(object sender, EventArgs e)
        {
            ResetGoods();
        }

        /// <summary>
        /// 重置信息
        /// </summary>
        void ResetGoods()
        {
            this.CurrentAutotruck = null;
            this.SelectedSupplyUnit_Goods = null;
            this.SelectedReceiveUnit_Goods = null;

            txtTagId_Goods.ResetText();
            txtRemark_Goods.ResetText();

            btnSaveTransport_Goods.Enabled = true;

            // 最后重置
            this.CurrentImperfectCar = null;
        }

        /// <summary>
        /// 加载运输记录
        /// </summary>
        void LoadTodayGoodsTransport()
        {
            //未完成运输记录
            IList<CmcsGoodsTransport> UnFinishlist = carTransportDAO.GetUnFinishGoodsTransport();
            superGridControl1_Goods.PrimaryGrid.DataSource = UnFinishlist;
            //指定日期已完成的入厂煤记录
            IList<CmcsGoodsTransport> Finishlist = carTransportDAO.GetFinishedGoodsTransport(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
            superGridControl2_Goods.PrimaryGrid.DataSource = Finishlist;

            labNumber_Goods.Text = string.Format("已登记：{0}  已称重：{1}  已回皮：{2}  未回皮：{3}", UnFinishlist.Count + Finishlist.Count(), UnFinishlist.Where(a => a.FirstWeight > 0).Count() + Finishlist.Count(), Finishlist.Where(a => a.FirstWeight > 0 && a.SecondWeight > 0).Count(), UnFinishlist.Where(a => a.FirstWeight > 0 && a.SecondWeight == 0).Count());
        }

        #region DataGridView

        /// <summary>
        /// 双击行时，自动填充录入信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void superGridControl_Goods_CellDoubleClick(object sender, DevComponents.DotNetBar.SuperGrid.GridCellDoubleClickEventArgs e)
        {
            GridRow gridRow = (sender as SuperGridControl).PrimaryGrid.ActiveRow as GridRow;
            if (gridRow == null) return;

            CmcsGoodsTransport entity = (gridRow.DataItem as CmcsGoodsTransport);
            if (entity != null)
            {
                this.SelectedSupplyUnit_Goods = commonDAO.SelfDber.Get<CmcsSupplyReceive>(entity.SupplyUnitId);
                this.SelectedReceiveUnit_Goods = commonDAO.SelfDber.Get<CmcsSupplyReceive>(entity.ReceiveUnitId);
                this.SelectedGoodsType_Goods = commonDAO.SelfDber.Get<CmcsGoodsType>(entity.GoodsTypeId);
            }
        }

        private void superGridControl1_Goods_CellClick(object sender, GridCellClickEventArgs e)
        {
            CmcsGoodsTransport entity = e.GridCell.GridRow.DataItem as CmcsGoodsTransport;
            if (entity == null) return;

            // 更改有效状态
            if (e.GridCell.GridColumn.Name == "ChangeIsUse") queuerDAO.ChangeGoodsTransportToInvalid(entity.Id, Convert.ToBoolean(e.GridCell.Value));
        }

        private void superGridControl1_Goods_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            foreach (GridRow gridRow in e.GridPanel.Rows)
            {
                CmcsGoodsTransport entity = gridRow.DataItem as CmcsGoodsTransport;
                if (entity == null) return;

                // 填充有效状态
                gridRow.Cells["ChangeIsUse"].Value = Convert.ToBoolean(entity.IsUse);
            }
        }

        /// <summary>
        /// 行单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void superGridControl2_Goods_CellClick(object sender, GridCellClickEventArgs e)
        {
            CmcsGoodsTransport entity = e.GridCell.GridRow.DataItem as CmcsGoodsTransport;
            if (entity == null) return;

            // 更改有效状态
            if (e.GridCell.GridColumn.Name == "ChangeIsUse") queuerDAO.ChangeGoodsTransportToInvalid(entity.Id, Convert.ToBoolean(e.GridCell.Value));
        }

        /// <summary>
        /// 加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void superGridControl2_Goods_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            foreach (GridRow gridRow in e.GridPanel.Rows)
            {
                CmcsGoodsTransport entity = gridRow.DataItem as CmcsGoodsTransport;
                if (entity == null) return;

                // 填充有效状态
                gridRow.Cells["ChangeIsUse"].Value = Convert.ToBoolean(entity.IsUse);
            }
        }
        /// <summary>
        /// 设置行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void superGridControl1_Goods_GetRowHeaderText(object sender, GridGetRowHeaderTextEventArgs e)
        {
            e.Text = (e.GridRow.RowIndex + 1).ToString();
        }

        private void superGridControl2_Goods_GetRowHeaderText(object sender, GridGetRowHeaderTextEventArgs e)
        {
            e.Text = (e.GridRow.RowIndex + 1).ToString();
        }

        private void superGridControl_BeginEdit(object sender, DevComponents.DotNetBar.SuperGrid.GridEditEventArgs e)
        {
            if (e.GridCell.GridColumn.DataPropertyName != "IsUse")
            {
                // 取消进入编辑
                e.Cancel = true;
            }
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

        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTB_Click(object sender, EventArgs e)
        {
            //系统第一次同步数据时使用，现在已废弃
            //carTransportDAO.InsertAutoTruck();
            carTransportDAO.InsertMine();
            //carTransportDAO.BossienInsertDttb_record_weigh(entity);
            //carTransportDAO.RepaireDT(DateTime.Now.Date.AddDays(-2), DateTime.Now.Date.AddDays(-1));
            //carTransportDAO.RepaireDT2(DateTime.Now.Date.AddDays(-1), DateTime.Now.Date);
        }

        #endregion

    }
}
