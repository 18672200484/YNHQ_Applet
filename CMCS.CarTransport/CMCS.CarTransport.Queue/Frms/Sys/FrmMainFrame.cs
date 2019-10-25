using System;
using System.Windows.Forms;
//
using DevComponents.DotNetBar;
using CMCS.Common.DAO;
using DevComponents.DotNetBar.Metro;
using CMCS.CarTransport.Queue.Utilities;
using CMCS.CarTransport.Queue.Core;
using CMCS.Common.Enums;
using CMCS.Common;

namespace CMCS.CarTransport.Queue.Frms.Sys
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
                btnBuyFuelDetail.Visible = value;
                btnAllCollect.Visible = value;
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

            OpenQueuer();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
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
        /// 打开入厂排队界面
        /// </summary>
        public void OpenQueuer()
        {
            string uniqueKey = FrmQueuer.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                FrmQueuer frm = new FrmQueuer();
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

        private void btnOpenTransportCompanyLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Queue.Frms.BaseInfo.TransportCompany.FrmTransportCompany_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Queue.Frms.BaseInfo.TransportCompany.FrmTransportCompany_List frm = new CMCS.CarTransport.Queue.Frms.BaseInfo.TransportCompany.FrmTransportCompany_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        private void btnOpenAutotruckLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Queue.Frms.BaseInfo.Autotruck.FrmAutotruck_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Queue.Frms.BaseInfo.Autotruck.FrmAutotruck_List frm = new CMCS.CarTransport.Queue.Frms.BaseInfo.Autotruck.FrmAutotruck_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        private void btnOpenCarModelLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Queue.Frms.BaseInfo.CarModel.FrmCarModel_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Queue.Frms.BaseInfo.CarModel.FrmCarModel_List frm = new CMCS.CarTransport.Queue.Frms.BaseInfo.CarModel.FrmCarModel_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        private void btnOpenSupplyReceiveLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Queue.Frms.BaseInfo.SupplyReceive.FrmSupplyReceive_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Queue.Frms.BaseInfo.SupplyReceive.FrmSupplyReceive_List frm = new CMCS.CarTransport.Queue.Frms.BaseInfo.SupplyReceive.FrmSupplyReceive_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        private void btnOpenFuelKindlLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Queue.Frms.BaseInfo.FuelKind.FrmFuelKind_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Queue.Frms.BaseInfo.FuelKind.FrmFuelKind_List frm = new CMCS.CarTransport.Queue.Frms.BaseInfo.FuelKind.FrmFuelKind_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        private void btnOpenSupplierLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Queue.Frms.BaseInfo.Supplier.FrmSupplier_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Queue.Frms.BaseInfo.Supplier.FrmSupplier_List frm = new CMCS.CarTransport.Queue.Frms.BaseInfo.Supplier.FrmSupplier_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        private void btnOpenMineLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Queue.Frms.BaseInfo.Mine.FrmMine_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Queue.Frms.BaseInfo.Mine.FrmMine_List frm = new CMCS.CarTransport.Queue.Frms.BaseInfo.Mine.FrmMine_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        private void btnOpenGoodsTypeLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Queue.Frms.BaseInfo.GoodsType.FrmGoodsType_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Queue.Frms.BaseInfo.GoodsType.FrmGoodsType_List frm = new CMCS.CarTransport.Queue.Frms.BaseInfo.GoodsType.FrmGoodsType_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        private void btnOpenBuyFuelTransportLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Queue.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Queue.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_List frm = new CMCS.CarTransport.Queue.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        private void btnOpenGoodsTransportLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Queue.Frms.Transport.GoodsTransport.FrmGoodsTransport_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Queue.Frms.Transport.GoodsTransport.FrmGoodsTransport_List frm = new CMCS.CarTransport.Queue.Frms.Transport.GoodsTransport.FrmGoodsTransport_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        private void btnOpenVisitTransportLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Queue.Frms.Transport.VisitTransport.FrmVisitTransport_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Queue.Frms.Transport.VisitTransport.FrmVisitTransport_List frm = new CMCS.CarTransport.Queue.Frms.Transport.VisitTransport.FrmVisitTransport_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        private void btnOpenChangePassword_Click(object sender, EventArgs e)
        {
            FrmPassword frmpassword = new FrmPassword();
            frmpassword.ShowDialog();
            if (frmpassword.DialogResult == DialogResult.OK)
            {
                MessageBoxEx.Show("修改密码成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnOpenSaleFuelTransport_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Queue.Frms.Transport.SaleFuelTransport.FrmSaleFuelTransport_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Queue.Frms.Transport.SaleFuelTransport.FrmSaleFuelTransport_List frm = new CMCS.CarTransport.Queue.Frms.Transport.SaleFuelTransport.FrmSaleFuelTransport_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        #region
        /// <summary>
        /// 入厂煤报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenBuyFuelTransportCollectLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Queue.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_Collect.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Queue.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_Collect frm = new CMCS.CarTransport.Queue.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_Collect();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }

        /// <summary>
        /// 汇总报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenBuyFuelTransportAllCollectLoad_Click(object sender, EventArgs e)
        {
            string uniqueKey = CMCS.CarTransport.Queue.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_AllCollect.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Queue.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_AllCollect frm = new CMCS.CarTransport.Queue.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_AllCollect();
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
            string uniqueKey = CMCS.CarTransport.Queue.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_Detail.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Queue.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_Detail frm = new CMCS.CarTransport.Queue.Frms.Transport.BuyFuelTransport.FrmBuyFuelTransport_Detail();
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
            string uniqueKey = CMCS.CarTransport.Queue.Frms.BaseInfo.AppletLog.FrmAppletLog_List.UniqueKey;

            if (FrmMainFrame.superTabControlManager.GetTab(uniqueKey) == null)
            {
                CMCS.CarTransport.Queue.Frms.BaseInfo.AppletLog.FrmAppletLog_List frm = new CMCS.CarTransport.Queue.Frms.BaseInfo.AppletLog.FrmAppletLog_List();
                FrmMainFrame.superTabControlManager.CreateTab(frm.Text, uniqueKey, frm, true, true);
            }
            else
                FrmMainFrame.superTabControlManager.ChangeToTab(uniqueKey);
        }
        #endregion
    }
}
