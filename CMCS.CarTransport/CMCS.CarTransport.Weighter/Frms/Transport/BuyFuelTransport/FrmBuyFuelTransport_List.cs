using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using CMCS.Common;
using CMCS.Common.Entities.CarTransport;
using DevComponents.DotNetBar.SuperGrid;
using CMCS.Common.Entities;
using CMCS.CarTransport.Weighter.Frms.Transport.TransportPicture;
using CMCS.Common.Entities.Fuel;
using CMCS.CarTransport.DAO;
using CMCS.Common.DAO;
using CMCS.CarTransport.Weighter.Core;
using CMCS.Common.Utilities;
using CMCS.Common.Enums;
using System.IO;
using NPOI.HSSF.UserModel;
using CMCS.CarTransport.Weighter.Frms.Transport.Print;

namespace CMCS.CarTransport.Weighter.Frms.Transport.BuyFuelTransport
{
    public partial class FrmBuyFuelTransport_List : MetroAppForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmBuyFuelTransport_List";

        List<CmcsBuyFuelTransport> listCount = new List<CmcsBuyFuelTransport>();

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
        CarTransportDAO carTransportDAO = CarTransportDAO.GetInstance();

        public FrmBuyFuelTransport_List()
        {
            InitializeComponent();
        }

        private void FrmBuyFuelTransport_List_Load(object sender, EventArgs e)
        {
            superGridControl1.PrimaryGrid.AutoGenerateColumns = false;
            HasManagePower = CommonDAO.GetInstance().HasResourcePowerByResCode(SelfVars.LoginUser.UserAccount, eUserRoleCodes.汽车智能化信息维护.ToString());

            dtpStartTime.Value = DateTime.Now;
            dtpEndTime.Value = DateTime.Now;

            btnSearch_Click(null, null);
        }

        public void BindData()
        {
            string tempSqlWhere = this.SqlWhere;
            List<CmcsBuyFuelTransport> list = Dbers.GetInstance().SelfDber.ExecutePager<CmcsBuyFuelTransport>(PageSize, CurrentIndex, tempSqlWhere + " order by SerialNumber desc");
            listCount = Dbers.GetInstance().SelfDber.Entities<CmcsBuyFuelTransport>(tempSqlWhere + " order by SerialNumber desc");
            superGridControl1.PrimaryGrid.DataSource = list;

            GetTotalCount(tempSqlWhere);
            PagerControlStatue();

            lblPagerInfo.Text = string.Format("共 {0} 条记录，每页 {1} 条，共 {2} 页，当前第 {3} 页", TotalCount, PageSize, PageCount, CurrentIndex + 1);
            labNumber_BuyFuel.Text = string.Format("已登记：{0}  已称重：{1}  已回皮：{2}  未回皮：{3}", listCount.Count, listCount.Where(a => a.GrossWeight > 0).Count(), listCount.Where(a => a.TareWeight > 0).Count(), listCount.Where(a => a.SuttleWeight == 0).Count());
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.SqlWhere = " where IsHidden=0";
            if (dtpStartTime.Value != DateTime.MinValue) this.SqlWhere += string.Format(" and case when trunc(taretime)>'2000-01-01' then trunc(taretime) else trunc(InFactorytime) end >= '{0}'", dtpStartTime.Value.Date.ToString("yyyy-MM-dd"));
            if (dtpEndTime.Value != DateTime.MinValue) this.SqlWhere += string.Format(" and case when trunc(taretime)>'2000-01-01' then trunc(taretime) else trunc(InFactorytime) end < '{0}'", dtpEndTime.Value.AddDays(1).ToString("yyyy-MM-dd"));
            if (!string.IsNullOrEmpty(txtMineName_BuyFuel.Text)) this.SqlWhere += " and MineName = '" + txtMineName_BuyFuel.Text + "'";
            if (!string.IsNullOrEmpty(txtCarNumber_Ser.Text)) this.SqlWhere += " and CarNumber like '%" + txtCarNumber_Ser.Text.Trim().ToUpper() + "%'";

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

        private void GetTotalCount(string sqlWhere)
        {
            TotalCount = Dbers.GetInstance().SelfDber.Count<CmcsBuyFuelTransport>(sqlWhere);
            if (TotalCount % PageSize != 0)
                PageCount = TotalCount / PageSize + 1;
            else
                PageCount = TotalCount / PageSize;
        }
        #endregion

        #region DataGridView

        private void dataGridViewX1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;

            CmcsBuyFuelTransport entity = Dbers.GetInstance().SelfDber.Get<CmcsBuyFuelTransport>(superGridControl1.PrimaryGrid.GetCell(e.ColumnIndex, superGridControl1.PrimaryGrid.Columns["clmId"].ColumnIndex).Value.ToString());
            if (entity == null)
                return;

            switch (superGridControl1.PrimaryGrid.Columns[e.ColumnIndex].Name)
            {
                case "clmDelete":
                    // 查询正在使用该记录的车数 
                    if (MessageBoxEx.Show("确定要删除该记录？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Dbers.GetInstance().SelfDber.Delete<CmcsBuyFuelTransport>(entity.Id);

                        BindData();
                    }
                    else
                        MessageBoxEx.Show("该记录正在使用中，禁止删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        private void dataGridViewX1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
        }

        private void dataGridViewX1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;

            //CmcsBuyFuelTransport entity = superGridControl1.PrimaryGrid.Rows[e.RowIndex] as CmcsBuyFuelTransport;

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
                case "clmDelete":
                    // 查询正在使用该记录的车数 
                    if (MessageBoxEx.Show("确定要删除该记录？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            Dbers.GetInstance().SelfDber.Delete<CmcsBuyFuelTransport>(entity.Id);
                            carTransportDAO.DelDttb_record_preset_weigh(entity.EpcCard);
                            carTransportDAO.DelAdvance(entity.Id);
                            carTransportDAO.DelUnFinishTransport(entity.Id);
                            Log4Neter.Info(string.Format("{0}删除运输记录，车号:{1} 矿点:{2}", SelfVars.LoginUser.UserName, entity.CarNumber, entity.MineName));
                        }
                        catch (Exception)
                        {
                            MessageBoxEx.Show("该记录正在使用中，禁止删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

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
                case "clmPrint":
                    FrmPrintWeb frm = new FrmPrintWeb(entity, null, eCarType.入厂煤);
                    frm.ShowDialog();
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
                //if (entity.GrossWeight > 0 || entity.TareWeight > 0)
                //    gridRow.Cells["clmDelete"].Visible = false;
                // 填充有效状态
                gridRow.Cells["clmIsUse"].Value = (entity.IsUse == 1 ? "是" : "否");
                CmcsInFactoryBatch cmcsinfactorybatch = Dbers.GetInstance().SelfDber.Get<CmcsInFactoryBatch>(entity.InFactoryBatchId);
                if (cmcsinfactorybatch != null)
                {
                    gridRow.Cells["clmInFactoryBatchNumber"].Value = cmcsinfactorybatch.Batch;
                }

                List<CmcsTransportPicture> cmcstrainwatchs = Dbers.GetInstance().SelfDber.Entities<CmcsTransportPicture>(String.Format(" where TransportId='{0}'", gridRow.Cells["clmId"].Value));
                if (cmcstrainwatchs.Count == 0)
                {
                    gridRow.Cells["clmPic"].Value = "";
                }
                gridRow.Cells["clmKzWeight"].Value = entity.KsWeight + entity.KgWeight;
                gridRow.Cells["clmKsWeight"].Value = entity.AutoKsWeight + entity.KsWeight;
            }
        }

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
        #region 导出Excel

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                FileStream file = new FileStream("鹤淇电厂入厂煤运输记录.xls", FileMode.Open, FileAccess.Read);
                HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                HSSFSheet sheetl = (HSSFSheet)hssfworkbook.GetSheet("sheet1");

                if (this.listCount.Count == 0)
                {
                    MessageBox.Show("请先查询数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
                    return;
                Mysheet1(sheetl, 1, 11, "日期：" + DateTime.Now.ToString("yyyy-MM-dd"));
                for (int i = 0; i < listCount.Count; i++)
                {
                    CmcsBuyFuelTransport entity = listCount[i];

                    Mysheet1(sheetl, i + 3, 0, entity.CarNumber);
                    Mysheet1(sheetl, i + 3, 1, entity.MineName);
                    Mysheet1(sheetl, i + 3, 2, entity.FuelKindName);
                    Mysheet1(sheetl, i + 3, 3, entity.GrossTime.Year > 2000 ? entity.GrossTime.ToString("yyyy/MM/dd HH:mm:ss") : "");
                    Mysheet1(sheetl, i + 3, 4, entity.TareTime.Year > 2000 ? entity.TareTime.ToString("yyyy/MM/dd HH:mm:ss") : "");
                    Mysheet1(sheetl, i + 3, 5, entity.TicketWeight.ToString("f2"));
                    Mysheet1(sheetl, i + 3, 6, entity.GrossWeight.ToString("f2"));
                    Mysheet1(sheetl, i + 3, 7, entity.TareWeight.ToString("f2"));
                    Mysheet1(sheetl, i + 3, 8, entity.SuttleWeight.ToString("f2"));
                    Mysheet1(sheetl, i + 3, 9, (entity.KsWeight + entity.KgWeight).ToString("f2"));
                    Mysheet1(sheetl, i + 3, 10, entity.KgWeight.ToString("f2"));
                    Mysheet1(sheetl, i + 3, 11, entity.KsWeight.ToString("f2"));
                    Mysheet1(sheetl, i + 3, 12, entity.CheckWeight.ToString("f2"));
                    sheetl.GetRow(i + 3).Height = sheetl.GetRow(3).Height;
                    int cellStyle = 3;//合计第2行，非合计第3行
                    if (entity.MineName == "合计")
                    {
                        cellStyle = 2;
                    }
                    sheetl.GetRow(i + 3).GetCell(0).CellStyle = sheetl.GetRow(cellStyle).GetCell(0).CellStyle;
                    sheetl.GetRow(i + 3).GetCell(1).CellStyle = sheetl.GetRow(cellStyle).GetCell(1).CellStyle;
                    sheetl.GetRow(i + 3).GetCell(2).CellStyle = sheetl.GetRow(cellStyle).GetCell(2).CellStyle;
                    sheetl.GetRow(i + 3).GetCell(3).CellStyle = sheetl.GetRow(cellStyle).GetCell(3).CellStyle;
                    sheetl.GetRow(i + 3).GetCell(4).CellStyle = sheetl.GetRow(cellStyle).GetCell(4).CellStyle;
                    sheetl.GetRow(i + 3).GetCell(5).CellStyle = sheetl.GetRow(cellStyle).GetCell(5).CellStyle;
                    sheetl.GetRow(i + 3).GetCell(6).CellStyle = sheetl.GetRow(cellStyle).GetCell(6).CellStyle;
                    sheetl.GetRow(i + 3).GetCell(7).CellStyle = sheetl.GetRow(cellStyle).GetCell(7).CellStyle;
                    sheetl.GetRow(i + 3).GetCell(8).CellStyle = sheetl.GetRow(cellStyle).GetCell(8).CellStyle;
                    sheetl.GetRow(i + 3).GetCell(9).CellStyle = sheetl.GetRow(cellStyle).GetCell(9).CellStyle;
                    sheetl.GetRow(i + 3).GetCell(10).CellStyle = sheetl.GetRow(cellStyle).GetCell(10).CellStyle;
                    sheetl.GetRow(i + 3).GetCell(11).CellStyle = sheetl.GetRow(cellStyle).GetCell(11).CellStyle;
                    sheetl.GetRow(i + 3).GetCell(12).CellStyle = sheetl.GetRow(cellStyle).GetCell(12).CellStyle;
                }
                //Mysheet1(sheetl, listCount.Count + 3, 0, "客户：");
                //Mysheet1(sheetl, listCount.Count + 3, 11, "交易中心：");
                //sheetl.GetRow(listCount.Count + 3).Height = sheetl.GetRow(3).Height;
                #region 合计
                //Mysheet1(sheetl, _CurrExportData.Count + 1, 0, "合计");
                //Mysheet1(sheetl, _CurrExportData.Count + 1, 2, _CurrExportData.Count + "车");
                //Mysheet1(sheetl, _CurrExportData.Count + 1, 9, Math.Round(_CurrExportData.Sum(a => a.TZ), 2).ToString());
                //Mysheet1(sheetl, _CurrExportData.Count + 1, 10, Math.Round(_CurrExportData.Sum(a => a.JZ), 2).ToString());
                //Mysheet1(sheetl, _CurrExportData.Count + 1, 11, Math.Round(_CurrExportData.Sum(a => a.KZ), 2).ToString());
                //Mysheet1(sheetl, _CurrExportData.Count + 1, 13, Math.Round(_CurrExportData.Sum(a => a.MZ), 2).ToString());
                //Mysheet1(sheetl, _CurrExportData.Count + 1, 19, Math.Round(_CurrExportData.Sum(a => a.PZ), 2).ToString());
                #endregion

                sheetl.ForceFormulaRecalculation = true;
                string fileName = "鹤淇电厂入厂煤运输记录_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls";
                GC.Collect();

                FileStream fs = File.OpenWrite(folderBrowserDialog1.SelectedPath + "\\" + fileName);
                hssfworkbook.Write(fs);   //向打开的这个xls文件中写入表并保存。  
                fs.Close();
                MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Mysheet1(HSSFSheet sheet1, int x, int y, String Value)
        {
            if (sheet1.GetRow(x) == null)
            {
                sheet1.CreateRow(x);
            }
            if (sheet1.GetRow(x).GetCell(y) == null)
            {
                sheet1.GetRow(x).CreateCell(y);
            }
            sheet1.GetRow(x).GetCell(y).SetCellValue(Value);

        }

        #endregion
    }
}
