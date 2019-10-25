using System;
using System.Windows.Forms;
//
using DevComponents.DotNetBar;
using CMCS.Common.DAO;
using DevComponents.DotNetBar.Metro;
using CMCS.CarTransport.Weighter.Utilities;
using CMCS.CarTransport.Weighter.Core;
using CMCS.Common.Enums;
using CMCS.Common;

namespace CMCS.CarTransport.Weighter.Frms.Sys
{
    public partial class FrmMainFrame : MetroForm
    {
        CommonDAO commonDAO = CommonDAO.GetInstance();

        bool hasManagePower = false;
        /// <summary>
        /// 对否有维护权限
        /// </summary>
        public bool HasManagePower
        {
            get
            {
                return hasManagePower;
            }

            set
            {
                hasManagePower = value;
                btnOpenSetting.Enabled = value;
                btnAppletLog.Enabled = value;
            }
        }

        public static SuperTabControlManager superTabControlManager;

        public FrmMainFrame()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblVersion.Text = new AU.Updater().Version;
            HasManagePower = CommonDAO.GetInstance().HasResourcePowerByResCode(SelfVars.LoginUser.UserAccount, eUserRoleCodes.汽车智能化信息维护.ToString());
            this.superTabControl1.Tabs.Clear();
            FrmMainFrame.superTabControlManager = new SuperTabControlManager(this.superTabControl1);

            this.WindowState = FormWindowState.Maximized;

            OpenWeight();

            if (commonDAO.GetAppletConfigString("连接打印机") == "1")
                OpenPrint();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (SelfVars.LoginUser == null) SelfVars.LoginUser = commonDAO.GetAdminUser();
            if (SelfVars.LoginUser != null) lblLoginUserName.Text = SelfVars.LoginUser.UserName;

            commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.系统.ToString(), "1");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBoxEx.Show("确认退出系统？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.系统.ToString(), "0");

                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApplicationExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer_CurrentTime_Tick(object sender, EventArgs e)
        {
            lblCurrentTime.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
        }

        #region 打开/切换可视主界面

        #region 弹出窗体

        /// <summary>
        /// 打开过衡界面
        /// </summary>
        public void OpenWeight()
        {
            string uniqueKey = FrmWeighter.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                FrmWeighter frm = new FrmWeighter();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, false);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        /// <summary>
        /// 打开打印界面
        /// </summary>
        public void OpenPrint()
        {
            string uniqueKey = FrmPrint_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                FrmPrint_List frm = new FrmPrint_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, false);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        /// <summary>
        /// 打开参数设置界面
        /// </summary>
        public void OpenSetting()
        {
            FrmSetting frm = new FrmSetting();
            frm.ShowDialog();
        }

        #endregion

        /// <summary>
        /// 打开参数设置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenSetting_Click(object sender, EventArgs e)
        {
            OpenSetting();
        }

        #endregion

        #region  打开运输记录
        private void btnOpenBuyFuelTransportLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Weighter.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Weighter.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_List frm = new CMCS.CarTransport.Weighter.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        private void btnOpenGoodsTransportLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Weighter.Frms.Transport.GoodsTransport.FrmGoodsTransport_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Weighter.Frms.Transport.GoodsTransport.FrmGoodsTransport_List frm = new CMCS.CarTransport.Weighter.Frms.Transport.GoodsTransport.FrmGoodsTransport_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        #endregion

        #region
        /// <summary>
        /// 汇总报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenBuyFuelTransportCollectLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Weighter.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_Collect.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Weighter.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_Collect frm = new CMCS.CarTransport.Weighter.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_Collect();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }
        /// <summary>
        /// 明细报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuyFuelDetail_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Weighter.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_Detail.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Weighter.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_Detail frm = new CMCS.CarTransport.Weighter.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_Detail();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }
        #endregion

        #region 基础信息
        /// <summary>
        /// 车辆信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenAutotruckLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Weighter.Frms.BaseInfo.Autotruck.FrmAutotruck_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Weighter.Frms.BaseInfo.Autotruck.FrmAutotruck_List frm = new CMCS.CarTransport.Weighter.Frms.BaseInfo.Autotruck.FrmAutotruck_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }
        /// <summary>
        /// 车型信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenCarModelLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Weighter.Frms.BaseInfo.CarModel.FrmCarModel_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Weighter.Frms.BaseInfo.CarModel.FrmCarModel_List frm = new CMCS.CarTransport.Weighter.Frms.BaseInfo.CarModel.FrmCarModel_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }
        /// <summary>
        /// 煤种
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFuelKindlLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Weighter.Frms.BaseInfo.FuelKind.FrmFuelKind_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Weighter.Frms.BaseInfo.FuelKind.FrmFuelKind_List frm = new CMCS.CarTransport.Weighter.Frms.BaseInfo.FuelKind.FrmFuelKind_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }
        /// <summary>
        /// 供应商
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenSupplierLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Weighter.Frms.BaseInfo.Supplier.FrmSupplier_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Weighter.Frms.BaseInfo.Supplier.FrmSupplier_List frm = new CMCS.CarTransport.Weighter.Frms.BaseInfo.Supplier.FrmSupplier_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }
        /// <summary>
        /// 矿点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenMineLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Weighter.Frms.BaseInfo.Mine.FrmMine_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Weighter.Frms.BaseInfo.Mine.FrmMine_List frm = new CMCS.CarTransport.Weighter.Frms.BaseInfo.Mine.FrmMine_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }
        /// <summary>
        /// 物资类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenGoodsTypeLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Weighter.Frms.BaseInfo.GoodsType.FrmGoodsType_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Weighter.Frms.BaseInfo.GoodsType.FrmGoodsType_List frm = new CMCS.CarTransport.Weighter.Frms.BaseInfo.GoodsType.FrmGoodsType_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }
        /// <summary>
        /// 供货单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenSupplyReceiveLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Weighter.Frms.BaseInfo.SupplyReceive.FrmSupplyReceive_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Weighter.Frms.BaseInfo.SupplyReceive.FrmSupplyReceive_List frm = new CMCS.CarTransport.Weighter.Frms.BaseInfo.SupplyReceive.FrmSupplyReceive_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAppletLog_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Weighter.Frms.BaseInfo.AppletLog.FrmAppletLog_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Weighter.Frms.BaseInfo.AppletLog.FrmAppletLog_List frm = new CMCS.CarTransport.Weighter.Frms.BaseInfo.AppletLog.FrmAppletLog_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }
        #endregion


    }
}
