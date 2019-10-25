using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using CMCS.Common;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.Fuel;
using DevComponents.DotNetBar.Controls;
using CMCS.Common.DAO;
using CMCS.CarTransport.DAO;
using CMCS.CarTransport.Queue.Core;
using CMCS.Common.Enums;
using CMCS.CarTransport.Queue.Utilities;
using CMCS.Common.Utilities;

namespace CMCS.CarTransport.Queue.Frms.Transport.BuyFuelTransport
{
    public partial class FrmBuyFuelTransport_Oper : DevComponents.DotNetBar.Metro.MetroForm
    {
        String id = String.Empty;
        bool edit = false;
        CmcsBuyFuelTransport cmcsBuyFuelTransport;
        CommonDAO commonDAO = CommonDAO.GetInstance();

        #region Vars

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
                    this.txt_CarNumber.Text = value.CarNumber;
                }
                else
                {
                    this.txt_CarNumber.ResetText();
                }
            }
        }

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
                    txt_SupplierName.Text = value.Name;
                }
                else
                {
                    txt_SupplierName.ResetText();
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
                    txt_MineName.Text = value.Name;
                }
                else
                {
                    txt_MineName.ResetText();
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
                dbi_GrossWeight.IsInputReadOnly = !value;
                dbi_TareWeight.IsInputReadOnly = !value;
            }
        }

        #endregion

        //List<CmcsBuyFuelTransportDeduct> cmcsbuyfueltransportdeducts;
        public FrmBuyFuelTransport_Oper()
        {
            InitializeComponent();
        }
        public FrmBuyFuelTransport_Oper(String pId, bool pEdit)
        {
            InitializeComponent();
            id = pId;
            edit = pEdit;
        }

        /// <summary>
        /// 加载汽车衡
        /// </summary>
        void LoadQCH(ComboBoxEx comboBoxEx)
        {
            comboBoxEx.DisplayMember = "Content";
            comboBoxEx.ValueMember = "Code";
            comboBoxEx.DataSource = CommonDAO.GetInstance().GetCodeContentByKind("地磅编号");
            //comboBoxEx.SelectedIndex = 0;
        }

        /// <summary>
        /// 加载卸煤区域
        /// </summary>
        void LoadUnloadArea(ComboBoxEx comboBoxEx)
        {
            comboBoxEx.DisplayMember = "Content";
            comboBoxEx.ValueMember = "Code";
            comboBoxEx.DataSource = CommonDAO.GetInstance().GetCodeContentByKind("卸煤区域");
            //comboBoxEx.SelectedIndex = 0;
        }

        private void cmbFuelName_BuyFuel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedFuelKind_BuyFuel = cmbFuelName_BuyFuel.SelectedItem as CmcsFuelKind;
        }
        private void FrmBuyFuelTransport_Oper_Load(object sender, EventArgs e)
        {
            cmbFuelName_BuyFuel.DisplayMember = "FuelName";
            cmbFuelName_BuyFuel.ValueMember = "Id";
            cmbFuelName_BuyFuel.DataSource = Dbers.GetInstance().SelfDber.Entities<CmcsFuelKind>("where Valid='有效' and ParentId is not null");
            cmbFuelName_BuyFuel.SelectedIndex = 0;
            HasManagePower = CommonDAO.GetInstance().HasResourcePowerByResCode(SelfVars.LoginUser.UserAccount, eUserRoleCodes.汽车智能化信息维护.ToString());
            LoadQCH(cmb_Qch);
            LoadUnloadArea(cmb_UnloadArea);
            if (!String.IsNullOrEmpty(id))
            {
                this.cmcsBuyFuelTransport = Dbers.GetInstance().SelfDber.Get<CmcsBuyFuelTransport>(this.id);
                txt_SerialNumber.Text = cmcsBuyFuelTransport.SerialNumber;
                txt_CarNumber.Text = cmcsBuyFuelTransport.CarNumber;
                CmcsInFactoryBatch cmcsinfactorybatch = Dbers.GetInstance().SelfDber.Get<CmcsInFactoryBatch>(cmcsBuyFuelTransport.InFactoryBatchId);
                if (cmcsinfactorybatch != null)
                {
                    txt_InFactoryBatchNumber.Text = cmcsinfactorybatch.Batch;
                }
                txt_SupplierName.Text = cmcsBuyFuelTransport.SupplierName;
                txt_MineName.Text = cmcsBuyFuelTransport.MineName;
                cmb_Qch.Text = cmcsBuyFuelTransport.Qch;
                cmbFuelName_BuyFuel.Text = cmcsBuyFuelTransport.FuelKindName;
                dbi_TicketWeight.Value = (double)cmcsBuyFuelTransport.TicketWeight;
                dbi_GrossWeight.Value = (double)cmcsBuyFuelTransport.GrossWeight;
                dbi_TareWeight.Value = (double)cmcsBuyFuelTransport.TareWeight;
                dbi_DeductWeight.Value = (double)cmcsBuyFuelTransport.DeductWeight;
                dbi_AutoKsWeight.Value = (double)cmcsBuyFuelTransport.AutoKsWeight;
                dbi_KsWeight.Value = (double)cmcsBuyFuelTransport.KsWeight;
                dbi_KgWeight.Value = (double)cmcsBuyFuelTransport.KgWeight;
                dbi_SuttleWeight.Value = (double)cmcsBuyFuelTransport.SuttleWeight;
                dbi_CheckWeight.Value = (double)cmcsBuyFuelTransport.CheckWeight;
                cmb_Qch.Text = cmcsBuyFuelTransport.Qch;
                cmb_UnloadArea.Text = cmcsBuyFuelTransport.UnloadArea;
                cmbUnloadType.Text = cmcsBuyFuelTransport.UnloadType;
                txt_InFactoryTime.Text = cmcsBuyFuelTransport.InFactoryTime.Year == 1 ? "" : cmcsBuyFuelTransport.InFactoryTime.ToString();
                txt_GrossTime.Text = cmcsBuyFuelTransport.GrossTime.Year == 1 ? "" : cmcsBuyFuelTransport.GrossTime.ToString();
                txt_UploadTime.Text = cmcsBuyFuelTransport.UploadTime.Year == 1 ? "" : cmcsBuyFuelTransport.UploadTime.ToString();
                txt_TareTime.Text = cmcsBuyFuelTransport.TareTime.Year == 1 ? "" : cmcsBuyFuelTransport.TareTime.ToString();
                txt_OutFactoryTime.Text = cmcsBuyFuelTransport.OutFactoryTime.Year == 1 ? "" : cmcsBuyFuelTransport.OutFactoryTime.ToString();
                txt_Flows.Text = cmcsBuyFuelTransport.FlowInfo;
                txt_Remark.Text = cmcsBuyFuelTransport.Remark;
                chb_IsFinish.Checked = (cmcsBuyFuelTransport.IsFinish == 1);
                chb_IsUse.Checked = (cmcsBuyFuelTransport.IsUse == 1);
            }
            if (!edit)
            {
                btnSubmit.Enabled = false;
                CMCS.CarTransport.Queue.Utilities.Helper.ControlReadOnly(panelEx2);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txt_CarNumber.Text.Length == 0)
            {
                MessageBoxEx.Show("车牌号不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ((cmcsBuyFuelTransport == null || cmcsBuyFuelTransport.CarNumber != txt_CarNumber.Text) && Dbers.GetInstance().SelfDber.Entities<CmcsBuyFuelTransport>(" where CarNumber=:CarNumber and IsFinish ='0'", new { CarNumber = txt_CarNumber.Text }).Count > 0)
            {
                MessageBoxEx.Show("车牌号不可重复！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmcsBuyFuelTransport != null)
            {
                if (string.IsNullOrEmpty(txt_Remark.Text))
                {
                    MessageBoxEx.Show("备注不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (cmcsBuyFuelTransport.CarNumber != txt_CarNumber.Text)
                {
                    Dbers.GetInstance().SelfDber.Execute(string.Format("update cmcstbadvance set carnumber='{0}' where carnumber='{1}'", txt_CarNumber.Text, cmcsBuyFuelTransport.CarNumber));
                }
                cmcsBuyFuelTransport.SerialNumber = txt_SerialNumber.Text;
                cmcsBuyFuelTransport.CarNumber = txt_CarNumber.Text;
                if (this.SelectedSupplier_BuyFuel != null)
                {
                    cmcsBuyFuelTransport.SupplierId = this.SelectedSupplier_BuyFuel.Id;
                    cmcsBuyFuelTransport.SupplierName = this.SelectedSupplier_BuyFuel.Name;
                }
                if (this.SelectedMine_BuyFuel != null)
                {
                    cmcsBuyFuelTransport.MineId = this.SelectedMine_BuyFuel.Id;
                    cmcsBuyFuelTransport.MineName = this.SelectedMine_BuyFuel.Name;
                }
                if (this.SelectedFuelKind_BuyFuel != null)
                {
                    cmcsBuyFuelTransport.FuelKindId = this.SelectedFuelKind_BuyFuel.Id;
                    cmcsBuyFuelTransport.FuelKindName = this.SelectedFuelKind_BuyFuel.FuelName;
                }

                string logValue = "修改前：" + Environment.NewLine;
                logValue += "车号：" + cmcsBuyFuelTransport.CarNumber + Environment.NewLine;
                logValue += "矿点：" + cmcsBuyFuelTransport.MineName + "   煤种：" + cmcsBuyFuelTransport.FuelKindName + Environment.NewLine;
                logValue += "入厂时间：" + cmcsBuyFuelTransport.InFactoryTime + "   矿发量：" + cmcsBuyFuelTransport.TicketWeight + Environment.NewLine;
                logValue += "毛重时间：" + cmcsBuyFuelTransport.GrossTime + "   毛重：" + cmcsBuyFuelTransport.GrossWeight + Environment.NewLine;
                logValue += "皮重时间：" + cmcsBuyFuelTransport.TareTime + "   皮重：" + cmcsBuyFuelTransport.TareWeight + Environment.NewLine;
                logValue += "扣矸：" + cmcsBuyFuelTransport.KgWeight + "   扣水：" + cmcsBuyFuelTransport.KsWeight + "   自动扣水：" + cmcsBuyFuelTransport.AutoKsWeight + Environment.NewLine;
                logValue += "出厂时间：" + cmcsBuyFuelTransport.OutFactoryTime + "   验收量：" + cmcsBuyFuelTransport.CheckWeight + Environment.NewLine;

                cmcsBuyFuelTransport.Remark = txt_Remark.Text;
                cmcsBuyFuelTransport.TicketWeight = (decimal)dbi_TicketWeight.Value;
                cmcsBuyFuelTransport.GrossWeight = (decimal)dbi_GrossWeight.Value;
                cmcsBuyFuelTransport.DeductWeight = (decimal)dbi_DeductWeight.Value;
                cmcsBuyFuelTransport.TareWeight = (decimal)dbi_TareWeight.Value;
                cmcsBuyFuelTransport.SuttleWeight = (decimal)dbi_SuttleWeight.Value;
                txt_Remark.Text = cmcsBuyFuelTransport.Remark;
                cmcsBuyFuelTransport.Qch = cmb_Qch.Text;
                cmcsBuyFuelTransport.UnloadArea = cmb_UnloadArea.Text;
                cmcsBuyFuelTransport.UnloadType = cmbUnloadType.Text;
                cmcsBuyFuelTransport.IsFinish = (chb_IsFinish.Checked ? 1 : 0);
                cmcsBuyFuelTransport.IsUse = (chb_IsUse.Checked ? 1 : 0);
                CmcsInFactoryBatch inFactoryBatch = CarTransportDAO.GetInstance().GCQCInFactoryBatchByBuyFuelTransport(cmcsBuyFuelTransport);
                if (inFactoryBatch != null)
                {
                    cmcsBuyFuelTransport.InFactoryBatchId = inFactoryBatch.Id;
                }

                cmcsBuyFuelTransport.KsWeight = (decimal)dbi_KsWeight.Value;
                cmcsBuyFuelTransport.KgWeight = (decimal)dbi_KgWeight.Value;
                WeighterDAO.GetInstance().SaveBuyFuelTransport(cmcsBuyFuelTransport);

                logValue += "修改后：" + Environment.NewLine;
                logValue += "车号：" + cmcsBuyFuelTransport.CarNumber + Environment.NewLine;
                logValue += "矿点：" + cmcsBuyFuelTransport.MineName + "   煤种：" + cmcsBuyFuelTransport.FuelKindName + Environment.NewLine;
                logValue += "入厂时间：" + cmcsBuyFuelTransport.InFactoryTime + "   矿发量：" + cmcsBuyFuelTransport.TicketWeight + Environment.NewLine;
                logValue += "毛重时间：" + cmcsBuyFuelTransport.GrossTime + "   毛重：" + cmcsBuyFuelTransport.GrossWeight + Environment.NewLine;
                logValue += "皮重时间：" + cmcsBuyFuelTransport.TareTime + "   皮重：" + cmcsBuyFuelTransport.TareWeight + Environment.NewLine;
                logValue += "扣矸：" + cmcsBuyFuelTransport.KgWeight + "   扣水：" + cmcsBuyFuelTransport.KsWeight + "   自动扣水：" + cmcsBuyFuelTransport.AutoKsWeight + Environment.NewLine;
                logValue += "出厂时间：" + cmcsBuyFuelTransport.OutFactoryTime + "   验收量：" + cmcsBuyFuelTransport.CheckWeight + Environment.NewLine;
                logValue += "修改人：" + SelfVars.LoginUser.UserName;
                commonDAO.SaveAppletLog(eAppletLogLevel.Info, "修改运输记录", logValue, SelfVars.LoginUser.UserAccount);
            }
            else
            {
                cmcsBuyFuelTransport = new CmcsBuyFuelTransport();
                cmcsBuyFuelTransport.SerialNumber = txt_SerialNumber.Text;
                cmcsBuyFuelTransport.CarNumber = txt_CarNumber.Text;
                if (this.SelectedSupplier_BuyFuel != null)
                {
                    cmcsBuyFuelTransport.SupplierId = this.SelectedSupplier_BuyFuel.Id;
                    cmcsBuyFuelTransport.SupplierName = this.SelectedSupplier_BuyFuel.Name;
                }
                if (this.SelectedMine_BuyFuel != null)
                {
                    cmcsBuyFuelTransport.MineId = this.SelectedMine_BuyFuel.Id;
                    cmcsBuyFuelTransport.MineName = this.SelectedMine_BuyFuel.Name;
                }
                if (this.SelectedFuelKind_BuyFuel != null)
                {
                    cmcsBuyFuelTransport.FuelKindId = this.SelectedFuelKind_BuyFuel.Id;
                    cmcsBuyFuelTransport.FuelKindName = this.SelectedFuelKind_BuyFuel.FuelName;
                }
                cmcsBuyFuelTransport.TicketWeight = (decimal)dbi_TicketWeight.Value;
                cmcsBuyFuelTransport.GrossWeight = (decimal)dbi_GrossWeight.Value;
                cmcsBuyFuelTransport.DeductWeight = (decimal)dbi_DeductWeight.Value;
                cmcsBuyFuelTransport.TareWeight = (decimal)dbi_TareWeight.Value;
                cmcsBuyFuelTransport.SuttleWeight = (decimal)dbi_SuttleWeight.Value;
                cmcsBuyFuelTransport.UnloadArea = cmb_UnloadArea.Text;
                cmcsBuyFuelTransport.UnloadType = cmbUnloadType.Text;
                txt_Remark.Text = cmcsBuyFuelTransport.Remark;
                cmcsBuyFuelTransport.Qch = cmb_Qch.Text;
                cmcsBuyFuelTransport.IsFinish = (chb_IsFinish.Checked ? 1 : 0);
                cmcsBuyFuelTransport.IsUse = (chb_IsUse.Checked ? 1 : 0);
                CmcsInFactoryBatch inFactoryBatch = CarTransportDAO.GetInstance().GCQCInFactoryBatchByBuyFuelTransport(cmcsBuyFuelTransport);
                if (inFactoryBatch != null)
                {
                    cmcsBuyFuelTransport.InFactoryBatchId = inFactoryBatch.Id;
                }
                Dbers.GetInstance().SelfDber.Insert(cmcsBuyFuelTransport);
                WeighterDAO.GetInstance().SaveBuyFuelTransport(cmcsBuyFuelTransport);
            }

            CarTransportDAO.GetInstance().InsertDttb_record_preset_weigh(cmcsBuyFuelTransport);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void SaveAndUpdate(CmcsBuyFuelTransport item, List<CmcsBuyFuelTransportDeduct> details)
        {
            List<CmcsBuyFuelTransportDeduct> olds = Dbers.GetInstance().SelfDber.Entities<CmcsBuyFuelTransportDeduct>(" where TransportId=:TransportId", new { TransportId = item.Id });
            foreach (CmcsBuyFuelTransportDeduct old in olds)
            {
                CmcsBuyFuelTransportDeduct del = details.Where(a => a.Id == old.Id).FirstOrDefault();
                if (del == null)
                {
                    Dbers.GetInstance().SelfDber.Delete<CmcsBuyFuelTransportDeduct>(old.Id);
                }
            }
            foreach (var detail in details)
            {
                detail.TransportId = item.Id;
                CmcsBuyFuelTransportDeduct insertorupdate = olds.Where(a => a.Id == detail.Id).FirstOrDefault();
                if (insertorupdate == null)
                {
                    Dbers.GetInstance().SelfDber.Insert(detail);
                }
                else
                {
                    Dbers.GetInstance().SelfDber.Update(detail);
                }
            }
        }

        /// <summary>
        /// 取消编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void superGridControl1_BeginEdit(object sender, DevComponents.DotNetBar.SuperGrid.GridEditEventArgs e)
        {
            // 取消编辑
            e.Cancel = true;
        }

        /// <summary>
        /// 选择车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoTruck_Click(object sender, EventArgs e)
        {
            FrmAutotruck_Select frm = new FrmAutotruck_Select(" and IsUse=1 order by CarNumber asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.CurrentAutotruck = frm.Output;
            }
        }

        /// <summary>
        /// 选择供应商
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupplier_Click(object sender, EventArgs e)
        {
            FrmSupplier_Select Frm = new FrmSupplier_Select("where IsStop='0' order by Name asc");
            Frm.ShowDialog();
            if (Frm.DialogResult == DialogResult.OK)
            {
                this.SelectedSupplier_BuyFuel = Frm.Output;
            }
        }

        /// <summary>
        /// 选择矿点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMine_Click(object sender, EventArgs e)
        {
            FrmMine_Select Frm = new FrmMine_Select("where Valid='有效' and (nodecode not like '00%' or nodecode is not null) order by Name asc");
            Frm.ShowDialog();
            if (Frm.DialogResult == DialogResult.OK)
            {
                this.SelectedMine_BuyFuel = Frm.Output;
            }
        }
    }
}