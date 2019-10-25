using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//
using DevComponents.DotNetBar;
using CMCS.CarTransport.Sample.Core;
using CMCS.Common.DAO;
using CMCS.Common.Utilities;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common;
using CMCS.CarTransport.Sample.Frms.Sys;
using DevComponents.DotNetBar.Controls;
using CMCS.Common.Views;
using DevComponents.DotNetBar.SuperGrid;
using CMCS.Common.Enums;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.Fuel;
using System.Text;
using CMCS.CarTransport.DAO;
using ThoughtWorks.QRCode.Codec;
using System.Drawing.Printing;


namespace CMCS.CarTransport.Sample.Frms
{
    public partial class FrmSampler : DevComponents.DotNetBar.Metro.MetroForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmSampler";

        public FrmSampler()
        {
            InitializeComponent();
        }

        #region Vars
        private string _SampleCode = "无";
        /// <summary>
        /// 当前采样码
        /// </summary>
        public string SampleCode
        {
            get { return _SampleCode; }
            set { _SampleCode = value; }
        }
        #endregion

        /// <summary>
        /// 窗体初始化
        /// </summary>
        private void InitForm()
        {

        }

        private void FrmSampler_Load(object sender, EventArgs e)
        {
            superGridControl1.PrimaryGrid.AutoGenerateColumns = false;
            dtpStartTime.Value = DateTime.Now;
            dtpEndTime.Value = DateTime.Now;
            DataBind();

            printDocument1.DefaultPageSettings.PaperSize = new PaperSize("Custum", 340, 140);
            printDocument1.OriginAtMargins = true;
            printDocument1.DefaultPageSettings.Margins.Left = 10;
            printDocument1.DefaultPageSettings.Margins.Right = 0;
            printDocument1.DefaultPageSettings.Margins.Top = 0;
            printDocument1.DefaultPageSettings.Margins.Bottom = 0;
            printDocument1.PrintController = new StandardPrintController();

        }

        private void FrmSampler_Shown(object sender, EventArgs e)
        {
            InitForm();
        }

        private void FrmQueuer_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        #region 入厂煤业务

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataBind();
        }

        /// <summary>
        /// 加载运输记录
        /// </summary>
        void DataBind()
        {
            string strWhere = "where 1=1 ";
            if (!string.IsNullOrEmpty(txtMineName_BuyFuel.Text)) strWhere += " and MineName ='" + txtMineName_BuyFuel.Text + "'";
            if (dtpStartTime.Value.Year > 2000) strWhere += " and SamplingDate>='" + dtpStartTime.Value.Date + "'";
            if (dtpEndTime.Value.Year > 2000) strWhere += " and SamplingDate<'" + dtpEndTime.Value.AddDays(1).Date + "'";

            IList<View_RCSampling> UnFinishlist = CommonDAO.GetInstance().SelfDber.Entities<View_RCSampling>(strWhere + "order by SamplingDate desc");
            superGridControl1.PrimaryGrid.DataSource = UnFinishlist;
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
                if (frm.Output != null)
                    this.txtMineName_BuyFuel.Text = frm.Output.Name;
            }
        }

        #region DataGridView

        private void superGridControl1_CellMouseDown(object sender, GridCellMouseEventArgs e)
        {
            GridRow gridRow = (sender as SuperGridControl).PrimaryGrid.ActiveRow as GridRow;
            if (gridRow == null) return;
            View_RCSampling entity = (gridRow.DataItem as View_RCSampling);
            if (entity == null) return;
            this.SampleCode = entity.SampleCode;
            switch (superGridControl1.PrimaryGrid.Columns[e.GridCell.ColumnIndex].Name)
            {
                case "clmPrint":
                    printDocument1.Print();
                    break;
            }
        }

        private void superGridControl1_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            try
            {
                foreach (GridRow gridRow in e.GridPanel.Rows)
                {
                    View_RCSampling entity = gridRow.DataItem as View_RCSampling;
                    if (entity == null) return;
                }
            }
            catch (Exception ex)
            {
                Log4Neter.Error("GridView加载完成事件", ex);
            }
        }

        private void superGridControl1_GetRowHeaderText(object sender, GridGetRowHeaderTextEventArgs e)
        {
            e.Text = (e.GridRow.RowIndex + 1).ToString();
        }
        #endregion

        #endregion

        #region 其他函数

        Font ContentFont = new Font("微软雅黑", 24, FontStyle.Bold, GraphicsUnit.Pixel);
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            Bitmap bitmap = CreateImgCode(SampleCode, 4);
            g.DrawImage(bitmap, 140, 20);
            g.DrawString(SampleCode, ContentFont, Brushes.Black, 90, 110);
        }

        /// <summary>  
        /// 生成二维码图片  
        /// </summary>  
        /// <param name="codeNumber">要生成二维码的字符串</param>       
        /// <param name="size">大小尺寸</param>  
        /// <returns>二维码图片</returns>  
        public Bitmap CreateImgCode(string codeNumber, int size)
        {
            //创建二维码生成类  
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //设置编码模式  
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            //设置编码测量度  
            qrCodeEncoder.QRCodeScale = size;
            //设置编码版本  
            qrCodeEncoder.QRCodeVersion = 0;
            //设置编码错误纠正  
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            //生成二维码图片  
            System.Drawing.Bitmap image = qrCodeEncoder.Encode(codeNumber);
            return image;
        }

        /// <summary>
        /// Invoke封装
        /// </summary>
        /// <param name="action"></param>
        public void InvokeEx(Action action)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;

            this.Invoke(action);
        }

        #endregion

    }
}
