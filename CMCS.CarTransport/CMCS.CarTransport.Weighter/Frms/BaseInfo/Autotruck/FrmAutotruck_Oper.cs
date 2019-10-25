using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using CMCS.Common;
using CMCS.Common.Entities.CarTransport;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;
using CMCS.CarTransport.Weighter.Utilities;
using CMCS.CarTransport.DAO;
using CMCS.Common.Enums;

namespace CMCS.CarTransport.Weighter.Frms.BaseInfo.Autotruck
{
    public partial class FrmAutotruck_Oper : DevComponents.DotNetBar.Metro.MetroForm
    {
        String id = String.Empty;
        bool edit = false;
        CmcsAutotruck cmcsAutotruck;
        public FrmAutotruck_Oper()
        {
            InitializeComponent();
            edit = true;
        }
        public FrmAutotruck_Oper(String pId, bool pEdit)
        {
            InitializeComponent();
            id = pId;
            edit = pEdit;
        }


        private void FrmAutotruck_Oper_Load(object sender, EventArgs e)
        {
            chb_IsUse.Checked = true;
            if (!String.IsNullOrEmpty(id))
            {
                this.cmcsAutotruck = Dbers.GetInstance().SelfDber.Get<CmcsAutotruck>(this.id);
                txt_CarNumber.Text = cmcsAutotruck.CarNumber;
                txt_Driver.Text = cmcsAutotruck.Driver;
                txt_CellPhoneNumber.Text = cmcsAutotruck.CellPhoneNumber;
                chb_IsUse.Checked = (cmcsAutotruck.IsUse == 1);
                dbi_LeftObstacle1.Value = cmcsAutotruck.LeftObstacle1;
                dbi_LeftObstacle2.Value = cmcsAutotruck.LeftObstacle2;
                dbi_LeftObstacle3.Value = cmcsAutotruck.LeftObstacle3;
                dbi_LeftObstacle4.Value = cmcsAutotruck.LeftObstacle4;
                dbi_LeftObstacle5.Value = cmcsAutotruck.LeftObstacle5;
                dbi_LeftObstacle6.Value = cmcsAutotruck.LeftObstacle6;
                dbi_RightObstacle1.Value = cmcsAutotruck.RightObstacle1;
                dbi_RightObstacle2.Value = cmcsAutotruck.RightObstacle2;
                dbi_RightObstacle3.Value = cmcsAutotruck.RightObstacle3;
                dbi_RightObstacle4.Value = cmcsAutotruck.RightObstacle4;
                dbi_RightObstacle5.Value = cmcsAutotruck.RightObstacle5;
                dbi_RightObstacle6.Value = cmcsAutotruck.RightObstacle6;
                dbi_CarriageLength.Value = cmcsAutotruck.CarriageLength;
                dbi_CarriageWidth.Value = cmcsAutotruck.CarriageWidth;
                dbi_CarriageHeight.Value = cmcsAutotruck.CarriageHeight;
                dbi_CarriageBottomToFloor.Value = cmcsAutotruck.CarriageBottomToFloor;
                txt_ReMark.Text = cmcsAutotruck.ReMark;
            }
            if (!edit)
            {
                btnSubmit.Enabled = false;
                CMCS.CarTransport.Weighter.Utilities.Helper.ControlReadOnly(panelEx2);
            }
            label_warn.ForeColor = Color.Red;
        }


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            txt_CarNumber.Text = txt_CarNumber.Text.ToUpper();
            if (txt_CarNumber.Text.Length == 0)
            {
                MessageBoxEx.Show("该车牌号不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ((cmcsAutotruck == null || cmcsAutotruck.CarNumber != txt_CarNumber.Text) && Dbers.GetInstance().SelfDber.Entities<CmcsAutotruck>(" where CarNumber=:CarNumber", new { CarNumber = txt_CarNumber.Text }).Count > 0)
            {
                MessageBoxEx.Show("该车牌号不可重复！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (String.IsNullOrEmpty(id)) { id = Guid.NewGuid().ToString(); }

            //if (dbi_CarriageLength.Value <= 0)
            //{
            //    MessageBoxEx.Show("该长不能为0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //if (dbi_CarriageWidth.Value <= 0)
            //{
            //    MessageBoxEx.Show("该宽不能为0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //if (dbi_CarriageHeight.Value <= 0)
            //{
            //    MessageBoxEx.Show("该高不能为0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //if (dbi_CarriageBottomToFloor.Value <= 0)
            //{
            //    MessageBoxEx.Show("该车厢底部到地面高不能为0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //if ((dbi_LeftObstacle6.Value > 0 || dbi_RightObstacle6.Value > 0) && (dbi_LeftObstacle5.Value <= 0 && dbi_RightObstacle5.Value <= 0))
            //{
            //    MessageBoxEx.Show("已有拉筋六信息必须有拉筋五信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //if ((dbi_LeftObstacle5.Value > 0 || dbi_RightObstacle5.Value > 0) && (dbi_LeftObstacle4.Value <= 0 && dbi_RightObstacle4.Value <= 0))
            //{
            //    MessageBoxEx.Show("已有拉筋五信息必须有拉筋四信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //if ((dbi_LeftObstacle4.Value > 0 || dbi_RightObstacle4.Value > 0) && (dbi_LeftObstacle3.Value <= 0 && dbi_RightObstacle3.Value <= 0))
            //{
            //    MessageBoxEx.Show("已有拉筋四信息必须有拉筋三信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //if ((dbi_LeftObstacle3.Value > 0 || dbi_RightObstacle3.Value > 0) && (dbi_LeftObstacle2.Value <= 0 && dbi_RightObstacle2.Value <= 0))
            //{
            //    MessageBoxEx.Show("已有拉筋三信息必须有拉筋二信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //if ((dbi_LeftObstacle2.Value > 0 || dbi_RightObstacle2.Value > 0) && (dbi_LeftObstacle1.Value <= 0 && dbi_RightObstacle1.Value <= 0))
            //{
            //    MessageBoxEx.Show("已有拉筋二信息必须有拉筋一信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            if (cmcsAutotruck != null)
            {
                if (cmcsAutotruck.CarNumber != txt_CarNumber.Text)
                {
                    Dbers.GetInstance().SelfDber.Execute(string.Format("update cmcstbbuyfueltransport set carnumber='{0}' where carnumber='{1}'", txt_CarNumber.Text, cmcsAutotruck.CarNumber));
                    Dbers.GetInstance().SelfDber.Execute(string.Format("update cmcstbadvance set carnumber='{0}' where carnumber='{1}'", txt_CarNumber.Text, cmcsAutotruck.CarNumber));
                }
                cmcsAutotruck.CarNumber = txt_CarNumber.Text;
                cmcsAutotruck.Driver = txt_Driver.Text;
                cmcsAutotruck.CellPhoneNumber = txt_CellPhoneNumber.Text;
                cmcsAutotruck.IsUse = (chb_IsUse.Checked ? 1 : 0);
                cmcsAutotruck.LeftObstacle1 = (int)dbi_LeftObstacle1.Value;
                cmcsAutotruck.LeftObstacle2 = (int)dbi_LeftObstacle2.Value;
                cmcsAutotruck.LeftObstacle3 = (int)dbi_LeftObstacle3.Value;
                cmcsAutotruck.LeftObstacle4 = (int)dbi_LeftObstacle4.Value;
                cmcsAutotruck.LeftObstacle5 = (int)dbi_LeftObstacle5.Value;
                cmcsAutotruck.LeftObstacle6 = (int)dbi_LeftObstacle6.Value;
                //cmcsAutotruck.RightObstacle1 = (int)dbi_RightObstacle1.Value;
                //cmcsAutotruck.RightObstacle2 = (int)dbi_RightObstacle2.Value;
                //cmcsAutotruck.RightObstacle3 = (int)dbi_RightObstacle3.Value;
                //cmcsAutotruck.RightObstacle4 = (int)dbi_RightObstacle4.Value;
                //cmcsAutotruck.RightObstacle5 = (int)dbi_RightObstacle5.Value;
                //cmcsAutotruck.RightObstacle6 = (int)dbi_RightObstacle6.Value;
                //右拉筋默认等于左拉筋
                cmcsAutotruck.RightObstacle1 = cmcsAutotruck.LeftObstacle1;
                cmcsAutotruck.RightObstacle2 = cmcsAutotruck.LeftObstacle2;
                cmcsAutotruck.RightObstacle3 = cmcsAutotruck.LeftObstacle3;
                cmcsAutotruck.RightObstacle4 = cmcsAutotruck.LeftObstacle4;
                cmcsAutotruck.RightObstacle5 = cmcsAutotruck.LeftObstacle5;
                cmcsAutotruck.RightObstacle6 = cmcsAutotruck.LeftObstacle6;

                cmcsAutotruck.CarriageLength = (int)dbi_CarriageLength.Value;
                cmcsAutotruck.CarriageWidth = (int)dbi_CarriageWidth.Value;
                cmcsAutotruck.CarriageHeight = (int)dbi_CarriageHeight.Value;
                cmcsAutotruck.CarriageBottomToFloor = (int)dbi_CarriageBottomToFloor.Value;
                cmcsAutotruck.ReMark = txt_ReMark.Text;
                cmcsAutotruck.CarType = eCarType.入厂煤.ToString();
                Dbers.GetInstance().SelfDber.Update(cmcsAutotruck);
                CarTransportDAO.GetInstance().BossienInsertDTtb_vehicle_tare(cmcsAutotruck);
            }
            else
            {
                cmcsAutotruck = new CmcsAutotruck();
                cmcsAutotruck.CarNumber = txt_CarNumber.Text;
                cmcsAutotruck.Driver = txt_Driver.Text;
                cmcsAutotruck.CellPhoneNumber = txt_CellPhoneNumber.Text;
                cmcsAutotruck.IsUse = (chb_IsUse.Checked ? 1 : 0);
                cmcsAutotruck.LeftObstacle1 = (int)dbi_LeftObstacle1.Value;
                cmcsAutotruck.LeftObstacle2 = (int)dbi_LeftObstacle2.Value;
                cmcsAutotruck.LeftObstacle3 = (int)dbi_LeftObstacle3.Value;
                cmcsAutotruck.LeftObstacle4 = (int)dbi_LeftObstacle4.Value;
                cmcsAutotruck.LeftObstacle5 = (int)dbi_LeftObstacle5.Value;
                cmcsAutotruck.LeftObstacle6 = (int)dbi_LeftObstacle6.Value;
                //cmcsAutotruck.RightObstacle1 = (int)dbi_RightObstacle1.Value;
                //cmcsAutotruck.RightObstacle2 = (int)dbi_RightObstacle2.Value;
                //cmcsAutotruck.RightObstacle3 = (int)dbi_RightObstacle3.Value;
                //cmcsAutotruck.RightObstacle4 = (int)dbi_RightObstacle4.Value;
                //cmcsAutotruck.RightObstacle5 = (int)dbi_RightObstacle5.Value;
                //cmcsAutotruck.RightObstacle6 = (int)dbi_RightObstacle6.Value;
                //右拉筋默认等于左拉筋
                cmcsAutotruck.RightObstacle1 = cmcsAutotruck.LeftObstacle1;
                cmcsAutotruck.RightObstacle2 = cmcsAutotruck.LeftObstacle2;
                cmcsAutotruck.RightObstacle3 = cmcsAutotruck.LeftObstacle3;
                cmcsAutotruck.RightObstacle4 = cmcsAutotruck.LeftObstacle4;
                cmcsAutotruck.RightObstacle5 = cmcsAutotruck.LeftObstacle5;
                cmcsAutotruck.RightObstacle6 = cmcsAutotruck.LeftObstacle6;

                cmcsAutotruck.CarriageLength = (int)dbi_CarriageLength.Value;
                cmcsAutotruck.CarriageWidth = (int)dbi_CarriageWidth.Value;
                cmcsAutotruck.CarriageHeight = (int)dbi_CarriageHeight.Value;
                cmcsAutotruck.CarriageBottomToFloor = (int)dbi_CarriageBottomToFloor.Value;
                cmcsAutotruck.ReMark = txt_ReMark.Text;
                cmcsAutotruck.CarType = eCarType.入厂煤.ToString();
                Dbers.GetInstance().SelfDber.Insert(cmcsAutotruck);
                CarTransportDAO.GetInstance().BossienInsertDTtb_vehicle_tare(cmcsAutotruck);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelectedCarModel_Click(object sender, EventArgs e)
        {
            FrmCarModel_Select Frm = new FrmCarModel_Select();
            Frm.ShowDialog();
            if (Frm.DialogResult == DialogResult.OK)
            {
                CmcsCarModel item = Frm.Output;
                dbi_LeftObstacle1.Value = item.LeftObstacle1;
                dbi_LeftObstacle2.Value = item.LeftObstacle2;
                dbi_LeftObstacle3.Value = item.LeftObstacle3;
                dbi_LeftObstacle4.Value = item.LeftObstacle4;
                dbi_LeftObstacle5.Value = item.LeftObstacle5;
                dbi_LeftObstacle6.Value = item.LeftObstacle6;
                dbi_RightObstacle1.Value = item.RightObstacle1;
                dbi_RightObstacle2.Value = item.RightObstacle2;
                dbi_RightObstacle3.Value = item.RightObstacle3;
                dbi_RightObstacle4.Value = item.RightObstacle4;
                dbi_RightObstacle5.Value = item.RightObstacle5;
                dbi_RightObstacle6.Value = item.RightObstacle6;
                dbi_CarriageLength.Value = item.CarriageLength;
                dbi_CarriageWidth.Value = item.CarriageWidth;
                dbi_CarriageHeight.Value = item.CarriageHeight;
                dbi_CarriageBottomToFloor.Value = item.CarriageBottomToFloor;
            }
        }

        private void txt_CarNumber_TextChanged(object sender, EventArgs e)
        {
        }

        private void txt_CarNumber_Leave(object sender, EventArgs e)
        {

            ((TextBoxX)sender).Text = ((TextBoxX)sender).Text.ToUpper();
        }

        /// <summary>
        /// 创建省份简称按钮
        /// </summary>
        private void CreateProvinceAbbreviationButton()
        {
            flpanProvinceAbbreviation.Controls.Clear();

            foreach (CmcsProvinceAbbreviation provinceAbbreviation in CarTransportDAO.GetInstance().GetProvinceAbbreviationsInOrder())
            {
                ButtonX btnProvinceAbbreviation = new ButtonX();
                btnProvinceAbbreviation.Text = provinceAbbreviation.PaName;
                btnProvinceAbbreviation.Style = eDotNetBarStyle.Metro;
                btnProvinceAbbreviation.Font = new Font("微软雅黑", 10.8f, FontStyle.Bold);
                btnProvinceAbbreviation.Size = new Size(26, 26);
                btnProvinceAbbreviation.Margin = new System.Windows.Forms.Padding(3);
                btnProvinceAbbreviation.Click += BtnProvinceAbbreviation_Click;

                flpanProvinceAbbreviation.Controls.Add(btnProvinceAbbreviation);
            }
        }

        /// <summary>
        /// 点击省份简称按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnProvinceAbbreviation_Click(object sender, EventArgs e)
        {
            ButtonX btnProvinceAbbreviation = sender as ButtonX;
            if (btnProvinceAbbreviation != null) CarTransportDAO.GetInstance().AddProvinceAbbreviationUseCount(btnProvinceAbbreviation.Text);

            txt_CarNumber.Text = txt_CarNumber.Text.Insert(0, btnProvinceAbbreviation.Text);
            txt_CarNumber.CloseDropDown();

            txt_CarNumber.Focus();
            txt_CarNumber.Select(txt_CarNumber.Text.Length, 0);
        }

        private void txt_CarNumber_ButtonDropDownClick(object sender, CancelEventArgs e)
        {
            CreateProvinceAbbreviationButton();
        }
    }
}