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
using CMCS.CarTransport.Weighter.Frms.Transport.BuyFuelTransport;
using CMCS.CarTransport.Weighter.Frms.Transport.Print;
using CMCS.CarTransport.Weighter.Frms.Transport.GoodsTransport;
using CMCS.Common.Enums;

namespace CMCS.CarTransport.Weighter.Frms
{
    public partial class FrmPrint_List : MetroAppForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmPrint_List";

        #region Vars

        /// <summary>
        /// 每页显示行数
        /// </summary>
        int PageSize = 18;

        /// <summary>
        /// 总页数
        /// </summary>
        int PageCount = 0;

        /// <summary>
        /// 总记录数
        /// </summary>
        int TotalCount = 0;

        /// <summary>
        /// 当前页索引
        /// </summary>
        int CurrentIndex = 0;

        string SqlWhere = string.Empty;

        #endregion

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

                superGridControl1.PrimaryGrid.Columns["clmDelete"].Visible = value;
            }
        }

        public FrmPrint_List()
        {
            InitializeComponent();
        }

        private void FrmBuyFuelTransport_List_Load(object sender, EventArgs e)
        {
            superGridControl1.PrimaryGrid.AutoGenerateColumns = false;
            superGridControl2.PrimaryGrid.AutoGenerateColumns = false;

            btnSearch_Click(null, null);
            GoodsbtnSearch_Click(null, null);
        }
        #region 入厂煤

        public void BindData()
        {
            string tempSqlWhere = this.SqlWhere;
            List<CmcsBuyFuelTransport> list = Dbers.GetInstance().SelfDber.ExecutePager<CmcsBuyFuelTransport>(PageSize, CurrentIndex, tempSqlWhere + " order by SerialNumber desc");
            superGridControl1.PrimaryGrid.DataSource = list;

            GetTotalCount(tempSqlWhere);
            PagerControlStatue();

            lblPagerInfo.Text = string.Format("共 {0} 条记录，每页 {1} 条，共 {2} 页，当前第 {3} 页", TotalCount, PageSize, PageCount, CurrentIndex + 1);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.SqlWhere = " where 1=1";

            if (!string.IsNullOrEmpty(txtCarNumber_Ser.Text)) this.SqlWhere += " and CarNumber like '%" + txtCarNumber_Ser.Text + "%'";

            CurrentIndex = 0;
            BindData();
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            this.SqlWhere = string.Empty;
            txtCarNumber_Ser.Text = string.Empty;

            CurrentIndex = 0;
            BindData();
        }

        private void btnInStore_Click(object sender, EventArgs e)
        {
            FrmBuyFuelTransport_Oper frm = new FrmBuyFuelTransport_Oper();
            frm.ShowDialog();

            BindData();
        }

        #region Pager

        private void btnPagerCommand_Click(object sender, EventArgs e)
        {
            ButtonX btn = sender as ButtonX;
            switch (btn.CommandParameter.ToString())
            {
                case "First":
                    CurrentIndex = 0;
                    break;
                case "Previous":
                    CurrentIndex = CurrentIndex - 1;
                    break;
                case "Next":
                    CurrentIndex = CurrentIndex + 1;
                    break;
                case "Last":
                    CurrentIndex = PageCount - 1;
                    break;
            }

            BindData();
        }

        public void PagerControlStatue()
        {
            if (PageCount <= 1)
            {
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
                btnLast.Enabled = false;
                btnNext.Enabled = false;

                return;
            }

            if (CurrentIndex == 0)
            {
                // 首页
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
                btnLast.Enabled = true;
                btnNext.Enabled = true;
            }

            if (CurrentIndex > 0 && CurrentIndex < PageCount - 1)
            {
                // 上一页/下一页
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
                btnLast.Enabled = true;
                btnNext.Enabled = true;
            }

            if (CurrentIndex == PageCount - 1)
            {
                // 末页
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }
        }

        #endregion

        #region DataGridView

        private void dataGridViewX1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
        }

        private void dataGridViewX1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;

            switch (superGridControl1.PrimaryGrid.Columns[e.ColumnIndex].Name)
            {
                case "clmDelete":
                    break;
            }
        }

        #endregion

        private void superGridControl1_BeginEdit(object sender, DevComponents.DotNetBar.SuperGrid.GridEditEventArgs e)
        {
            // 取消编辑
            e.Cancel = true;
        }

        private void superGridControl1_CellMouseDown(object sender, DevComponents.DotNetBar.SuperGrid.GridCellMouseEventArgs e)
        {
            CmcsBuyFuelTransport entity = Dbers.GetInstance().SelfDber.Get<CmcsBuyFuelTransport>(superGridControl1.PrimaryGrid.GetCell(e.GridCell.GridRow.Index, superGridControl1.PrimaryGrid.Columns["clmId"].ColumnIndex).Value.ToString());
            switch (superGridControl1.PrimaryGrid.Columns[e.GridCell.ColumnIndex].Name)
            {

                case "clmShow":
                    FrmBuyFuelTransport_Oper frmShow = new FrmBuyFuelTransport_Oper(entity.Id, false);
                    if (frmShow.ShowDialog() == DialogResult.OK)
                    {
                        BindData();
                    }
                    break;
                case "clmEdit":
                    FrmBuyFuelTransport_Oper frmEdit = new FrmBuyFuelTransport_Oper(entity.Id, true);
                    if (frmEdit.ShowDialog() == DialogResult.OK)
                    {
                        BindData();
                    }
                    break;
                case "clmPic":

                    if (Dbers.GetInstance().SelfDber.Entities<CmcsTransportPicture>(String.Format(" where TransportId='{0}'", entity.Id)).Count > 0)
                    {
                        FrmTransportPicture frmPic = new FrmTransportPicture(entity.Id, entity.CarNumber);
                        if (frmPic.ShowDialog() == DialogResult.OK)
                        {
                            BindData();
                        }
                    }
                    else
                    {
                        MessageBoxEx.Show("暂无抓拍图片！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            FrmBuyFuelTransport_Oper frmEdit = new FrmBuyFuelTransport_Oper(String.Empty, true);
            if (frmEdit.ShowDialog() == DialogResult.OK)
            {
                BindData();
            }
        }

        private void superGridControl1_DataBindingComplete(object sender, DevComponents.DotNetBar.SuperGrid.GridDataBindingCompleteEventArgs e)
        {

            foreach (GridRow gridRow in e.GridPanel.Rows)
            {
                CmcsBuyFuelTransport entity = gridRow.DataItem as CmcsBuyFuelTransport;
                if (entity == null) return;

                // 填充有效状态
                gridRow.Cells["clmIsUse"].Value = (entity.IsUse == 1 ? "是" : "否");


            }
        }

        private void GetTotalCount(string sqlWhere)
        {
            TotalCount = Dbers.GetInstance().SelfDber.Count<CmcsBuyFuelTransport>(sqlWhere);
            if (TotalCount % PageSize != 0)
                PageCount = TotalCount / PageSize + 1;
            else
                PageCount = TotalCount / PageSize;
        }
        /// <summary>
        /// 打印磅单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiBuyFuelPrint_Click(object sender, EventArgs e)
        {
            GridRow gridRow = superGridControl1.PrimaryGrid.ActiveRow as GridRow;
            if (gridRow == null) return;
            CmcsBuyFuelTransport entity = gridRow.DataItem as CmcsBuyFuelTransport;
            FrmPrintWeb frm = new FrmPrintWeb(entity, null, eCarType.入厂煤);
            frm.ShowDialog();
        }
        #endregion


        #region Vars

        /// <summary>
        /// 每页显示行数
        /// </summary>
        int GoodsPageSize = 18;

        /// <summary>
        /// 总页数
        /// </summary>
        int GoodsPageCount = 0;

        /// <summary>
        /// 总记录数
        /// </summary>
        int GoodsTotalCount = 0;

        /// <summary>
        /// 当前页索引
        /// </summary>
        int GoodsCurrentIndex = 0;

        string GoodsSqlWhere = string.Empty;

        #endregion

        #region 其它物资

        public void GoodsBindData()
        {
            string tempSqlWhere = this.GoodsSqlWhere;
            List<CmcsGoodsTransport> list = Dbers.GetInstance().SelfDber.ExecutePager<CmcsGoodsTransport>(GoodsPageSize, GoodsCurrentIndex, tempSqlWhere + " order by SerialNumber desc");
            superGridControl2.PrimaryGrid.DataSource = list;

            GoodsGetTotalCount(tempSqlWhere);
            GoodsPagerControlStatue();

            lblPagerInfo2.Text = string.Format("共 {0} 条记录，每页 {1} 条，共 {2} 页，当前第 {3} 页", GoodsTotalCount, GoodsPageSize, GoodsPageCount, GoodsCurrentIndex + 1);
        }

        private void GoodsbtnSearch_Click(object sender, EventArgs e)
        {
            this.GoodsSqlWhere = " where 1=1";

            if (!string.IsNullOrEmpty(txtCarNumber_Ser.Text)) this.GoodsSqlWhere += " and CarNumber like '%" + txtCarNumber_Ser.Text + "%'";

            GoodsCurrentIndex = 0;
            GoodsBindData();
        }

        private void GoodsbtnAll_Click(object sender, EventArgs e)
        {
            this.SqlWhere = string.Empty;
            txtCarNumber_Ser.Text = string.Empty;

            GoodsCurrentIndex = 0;
            GoodsBindData();
        }

        private void GoodsbtnInStore_Click(object sender, EventArgs e)
        {
            FrmGoodsTransport_Oper frm = new FrmGoodsTransport_Oper();
            frm.ShowDialog();

            GoodsBindData();
        }

        #region Pager

        private void GoodsbtnPagerCommand_Click(object sender, EventArgs e)
        {
            ButtonX btn = sender as ButtonX;
            switch (btn.CommandParameter.ToString())
            {
                case "First":
                    GoodsCurrentIndex = 0;
                    break;
                case "Previous":
                    GoodsCurrentIndex = GoodsCurrentIndex - 1;
                    break;
                case "Next":
                    GoodsCurrentIndex = GoodsCurrentIndex + 1;
                    break;
                case "Last":
                    GoodsCurrentIndex = GoodsPageCount - 1;
                    break;
            }

            GoodsBindData();
        }

        public void GoodsPagerControlStatue()
        {
            if (GoodsPageCount <= 1)
            {
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
                btnLast.Enabled = false;
                btnNext.Enabled = false;

                return;
            }

            if (GoodsCurrentIndex == 0)
            {
                // 首页
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
                btnLast.Enabled = true;
                btnNext.Enabled = true;
            }

            if (GoodsCurrentIndex > 0 && GoodsCurrentIndex < GoodsPageCount - 1)
            {
                // 上一页/下一页
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
                btnLast.Enabled = true;
                btnNext.Enabled = true;
            }

            if (GoodsCurrentIndex == GoodsPageCount - 1)
            {
                // 末页
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }
        }

        private void GoodsGetTotalCount(string sqlWhere)
        {
            TotalCount = Dbers.GetInstance().SelfDber.Count<CmcsGoodsTransport>(sqlWhere);
            if (TotalCount % PageSize != 0)
                GoodsPageCount = TotalCount / PageSize + 1;
            else
                GoodsPageCount = TotalCount / PageSize;
        }
        #endregion

        #region DataGridView

        private void GoodsdataGridViewX2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;

            CmcsGoodsTransport entity = Dbers.GetInstance().SelfDber.Get<CmcsGoodsTransport>(superGridControl2.PrimaryGrid.GetCell(e.ColumnIndex, superGridControl2.PrimaryGrid.Columns["clmId"].ColumnIndex).Value.ToString());
            if (entity == null)
                return;

            switch (superGridControl2.PrimaryGrid.Columns[e.ColumnIndex].Name)
            {
                case "clmDelete":
                    // 查询正在使用该记录的车数 
                    if (MessageBoxEx.Show("确定要删除该记录？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Dbers.GetInstance().SelfDber.Delete<CmcsGoodsTransport>(entity.Id);

                        BindData();
                    }
                    else
                        MessageBoxEx.Show("该记录正在使用中，禁止删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        private void GoodsdataGridViewX2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
        }

        private void GoodsdataGridViewX2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;

            //CmcsGoodsTransport entity = superGridControl2.PrimaryGrid.Rows[e.RowIndex] as CmcsGoodsTransport;

            switch (superGridControl2.PrimaryGrid.Columns[e.ColumnIndex].Name)
            {
                case "clmDelete":
                    break;
            }
        }

        #endregion

        private void superGridControl2_BeginEdit(object sender, DevComponents.DotNetBar.SuperGrid.GridEditEventArgs e)
        {
            // 取消编辑
            e.Cancel = true;
        }

        private void superGridControl2_CellMouseDown(object sender, DevComponents.DotNetBar.SuperGrid.GridCellMouseEventArgs e)
        {
            CmcsGoodsTransport entity = Dbers.GetInstance().SelfDber.Get<CmcsGoodsTransport>(superGridControl2.PrimaryGrid.GetCell(e.GridCell.GridRow.Index, superGridControl2.PrimaryGrid.Columns["clmId"].ColumnIndex).Value.ToString());
            switch (superGridControl2.PrimaryGrid.Columns[e.GridCell.ColumnIndex].Name)
            {

                case "clmShow":
                    FrmGoodsTransport_Oper frmShow = new FrmGoodsTransport_Oper(entity.Id, false);
                    if (frmShow.ShowDialog() == DialogResult.OK)
                    {
                        GoodsBindData();
                    }
                    break;
                case "clmEdit":
                    FrmGoodsTransport_Oper frmEdit = new FrmGoodsTransport_Oper(entity.Id, true);
                    if (frmEdit.ShowDialog() == DialogResult.OK)
                    {
                        GoodsBindData();
                    }
                    break;
                case "clmDelete":
                    // 查询正在使用该记录的车数 
                    if (MessageBoxEx.Show("确定要删除该记录？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            Dbers.GetInstance().SelfDber.Delete<CmcsGoodsTransport>(entity.Id);
                        }
                        catch (Exception)
                        {
                            MessageBoxEx.Show("该记录正在使用中，禁止删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        GoodsBindData();
                    }
                    break;
                case "clmPic":

                    if (Dbers.GetInstance().SelfDber.Entities<CmcsTransportPicture>(String.Format(" where TransportId='{0}'", entity.Id)).Count > 0)
                    {
                        FrmTransportPicture frmPic = new FrmTransportPicture(entity.Id, entity.CarNumber);
                        if (frmPic.ShowDialog() == DialogResult.OK)
                        {
                            GoodsBindData();
                        }
                    }
                    else
                    {
                        MessageBoxEx.Show("暂无抓拍图片！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
            }
        }

        private void GoodsBtnAdd_Click(object sender, EventArgs e)
        {
            FrmGoodsTransport_Oper frmEdit = new FrmGoodsTransport_Oper(String.Empty, true);
            if (frmEdit.ShowDialog() == DialogResult.OK)
            {
                GoodsBindData();
            }
        }

        private void superGridControl2_DataBindingComplete(object sender, DevComponents.DotNetBar.SuperGrid.GridDataBindingCompleteEventArgs e)
        {

            foreach (GridRow gridRow in e.GridPanel.Rows)
            {
                CmcsGoodsTransport entity = gridRow.DataItem as CmcsGoodsTransport;
                if (entity == null) return;

                // 填充有效状态
                gridRow.Cells["clmIsUse"].Value = (entity.IsUse == 1 ? "是" : "否");


            }
        }
        /// <summary>
        /// 打印磅单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiGoodsPrint_Click(object sender, EventArgs e)
        {
            GridRow gridRow = superGridControl2.PrimaryGrid.ActiveRow as GridRow;
            if (gridRow == null) return;
            CmcsGoodsTransport entity = gridRow.DataItem as CmcsGoodsTransport;
            FrmPrintWeb frm = new FrmPrintWeb(null, entity, eCarType.其他物资);
            frm.ShowDialog();
        }
        #endregion
    }
}
