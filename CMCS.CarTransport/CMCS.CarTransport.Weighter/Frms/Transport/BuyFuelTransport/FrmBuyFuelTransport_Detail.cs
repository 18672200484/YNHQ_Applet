using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using CMCS.Common;
using CMCS.Common.Entities.CarTransport;
using DevComponents.DotNetBar.SuperGrid;
using CMCS.Common.Entities;
using CMCS.CarTransport.Weighter.Frms.Transport.TransportPicture;
using CMCS.Common.Entities.Fuel;
using CMCS.CarTransport.Weighter.Frms.Transport.Print;
using CMCS.Common.Enums;
using System.Linq;
using CMCS.CarTransport.DAO;

namespace CMCS.CarTransport.Weighter.Frms.Transport.BuyFuelTransport
{
    public partial class FrmBuyFuelTransport_Detail : MetroAppForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmBuyFuelTransport_Detail";

        WagonPrinterDetail wagonPrinter = null;
        List<CmcsBuyFuelTransport> listCount = new List<CmcsBuyFuelTransport>();

        string SqlWhere = string.Empty;

        public FrmBuyFuelTransport_Detail()
        {
            InitializeComponent();
        }

        private void FrmBuyFuelTransport_List_Load(object sender, EventArgs e)
        {
            superGridControl1.PrimaryGrid.AutoGenerateColumns = false;

            dtpStartTime.Value = DateTime.Now;
            dtpEndTime.Value = DateTime.Now;

            this.wagonPrinter = new WagonPrinterDetail(printDocument1);

            btnSearch_Click(null, null);
        }

        public void BindData()
        {
            listCount.Clear();
            string tempSqlWhere = this.SqlWhere;
            listCount = Dbers.GetInstance().SelfDber.Entities<CmcsBuyFuelTransport>(tempSqlWhere + " order by SerialNumber desc");

            labNumber_BuyFuel.Text = string.Format("已登记：{0}  已称重：{1}  已回皮：{2}  未回皮：{3}", listCount.Count, listCount.Where(a => a.GrossWeight > 0).Count(), listCount.Where(a => a.TareWeight > 0).Count(), listCount.Where(a => a.SuttleWeight == 0).Count());
            listCount.OrderBy(a => a.MineName);
            CmcsBuyFuelTransport listTotal1 = new CmcsBuyFuelTransport();
            listTotal1.CarNumber = "合计";
            listTotal1.TicketWeight = listCount.Sum(a => a.TicketWeight);
            listTotal1.GrossWeight = listCount.Sum(a => a.GrossWeight);
            listTotal1.TareWeight = listCount.Sum(a => a.TareWeight);
            listTotal1.SuttleWeight = listCount.Sum(a => a.SuttleWeight);
            listTotal1.CheckWeight = listCount.Sum(a => a.CheckWeight);
            listTotal1.ProfitAndLossWeight = listCount.Sum(a => a.ProfitAndLossWeight);
            listTotal1.DeductWeight = listCount.Sum(a => a.DeductWeight);
            listTotal1.KgWeight = listCount.Sum(a => a.KgWeight);
            listTotal1.KsWeight = listCount.Sum(a => a.KsWeight);
            listTotal1.MineName = listCount.Count.ToString() + "车";//车数
            listCount.Add(listTotal1);

            superGridControl1.PrimaryGrid.DataSource = listCount;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.SqlWhere = " where 1=1";
            if (dtpStartTime.Value != DateTime.MinValue) this.SqlWhere += " and trunc(InFactoryTime) >= '" + dtpStartTime.Value.ToString("yyyy-MM-dd") + "'";
            if (dtpEndTime.Value != DateTime.MinValue) this.SqlWhere += " and trunc(InFactoryTime) < '" + dtpEndTime.Value.AddDays(1).ToString("yyyy-MM-dd") + "'";
            if (!string.IsNullOrEmpty(txtCarNumber.Text)) this.SqlWhere += " and CarNumber like '%" + txtCarNumber.Text + "%'";
            if (!string.IsNullOrEmpty(txtMineName_BuyFuel.Text)) this.SqlWhere += " and MineName = '" + txtMineName_BuyFuel.Text + "'";
            BindData();
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            this.SqlWhere = string.Empty;
            txtCarNumber.Text = string.Empty;

            BindData();
        }

        private void btnInStore_Click(object sender, EventArgs e)
        {
            FrmBuyFuelTransport_Oper frm = new FrmBuyFuelTransport_Oper();
            frm.ShowDialog();

            BindData();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            FrmBuyFuelTransport_Oper frmEdit = new FrmBuyFuelTransport_Oper(String.Empty, true);
            if (frmEdit.ShowDialog() == DialogResult.OK)
            {
                BindData();
            }
        }

        /// <summary>
        /// 打印磅单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiPrint_Click(object sender, EventArgs e)
        {
            this.wagonPrinter.Print(this.listCount, null, dtpStartTime.Value, dtpEndTime.Value);
        }
        /// <summary>
        /// 选择矿点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectMine_BuyFuel_Click(object sender, EventArgs e)
        {
            FrmMine_Select frm = new FrmMine_Select("where Valid='有效' and (nodecode not like '00%' or nodecode is not null) order by Name asc");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.Output != null) this.txtMineName_BuyFuel.Text = frm.Output.Name;
            }
        }

        /// <summary>
        /// 设置行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void superGridControl1_GetRowHeaderText(object sender, GridGetRowHeaderTextEventArgs e)
        {
            e.Text = (e.GridRow.RowIndex + 1).ToString();
        }

        private void superGridControl1_BeginEdit(object sender, DevComponents.DotNetBar.SuperGrid.GridEditEventArgs e)
        {
            if (e.GridCell.GridColumn.DataPropertyName != "IsUse")
            {
                // 取消进入编辑
                //e.Cancel = true;
            }
        }

        private void superGridControl1_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            foreach (GridRow gridRow in e.GridPanel.Rows)
            {
                CmcsBuyFuelTransport entity = gridRow.DataItem as CmcsBuyFuelTransport;
                if (entity == null) return;

                // 填充有效状态
                gridRow.Cells["ChangeIsHidden"].Value = Convert.ToBoolean(entity.IsHidden);
            }
        }

        private void superGridControl1_CellClick(object sender, GridCellClickEventArgs e)
        {
            CmcsBuyFuelTransport entity = e.GridCell.GridRow.DataItem as CmcsBuyFuelTransport;
            if (entity == null) return;
            if (e.GridCell.GridColumn.Name == "ChangeIsHidden" && entity.SuttleWeight <= 0)
            {
                MessageBoxEx.Show("无净重,禁止操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.GridCell.Value = !Convert.ToBoolean(e.GridCell.Value);
                return;
            }

            QueuerDAO.GetInstance().ChangeBuyFuelTransportHidden(entity.Id, Convert.ToBoolean(e.GridCell.Value));
        }
    }
}
