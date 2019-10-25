namespace CMCS.CarTransport.Weighter.Frms.BaseInfo.Mine
{
    partial class FrmMine_List
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.advTree1 = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.BtnAdd = new DevComponents.DotNetBar.ButtonX();
            this.BtnEdit = new DevComponents.DotNetBar.ButtonX();
            this.BtnDel = new DevComponents.DotNetBar.ButtonX();
            this.chb_IsUse = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.txt_Name = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txt_ReMark = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.dbi_Sequence = new DevComponents.Editors.IntegerInput();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.btnSubmit = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.panelEx3 = new DevComponents.DotNetBar.PanelEx();
            this.panelLeft = new DevComponents.DotNetBar.PanelEx();
            this.panelRight = new DevComponents.DotNetBar.PanelEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.cmbUnLoader_BuyFuel = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbi_Sequence)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelEx2.SuspendLayout();
            this.panelEx3.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // advTree1
            // 
            this.advTree1.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTree1.AllowDrop = true;
            this.advTree1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.advTree1.BackgroundStyle.Class = "TreeBorderKey";
            this.advTree1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTree1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.advTree1.ForeColor = System.Drawing.Color.White;
            this.advTree1.Location = new System.Drawing.Point(22, 36);
            this.advTree1.Name = "advTree1";
            this.advTree1.NodesConnector = this.nodeConnector1;
            this.advTree1.NodeStyle = this.elementStyle1;
            this.advTree1.PathSeparator = ";";
            this.advTree1.Size = new System.Drawing.Size(355, 510);
            this.advTree1.Styles.Add(this.elementStyle1);
            this.advTree1.TabIndex = 5;
            this.advTree1.Text = "advTree1";
            this.advTree1.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTree1_NodeClick);
            this.advTree1.NodeDoubleClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTree1_NodeDoubleClick);
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(188)))), ((int)(((byte)(204)))));
            // 
            // elementStyle1
            // 
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.Color.White;
            // 
            // BtnAdd
            // 
            this.BtnAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnAdd.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.BtnAdd.Location = new System.Drawing.Point(12, 5);
            this.BtnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(64, 23);
            this.BtnAdd.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnAdd.TabIndex = 19;
            this.BtnAdd.Text = "新 增";
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnEdit
            // 
            this.BtnEdit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnEdit.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.BtnEdit.Location = new System.Drawing.Point(82, 5);
            this.BtnEdit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnEdit.Name = "BtnEdit";
            this.BtnEdit.Size = new System.Drawing.Size(64, 23);
            this.BtnEdit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnEdit.TabIndex = 20;
            this.BtnEdit.Text = "修 改";
            this.BtnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // BtnDel
            // 
            this.BtnDel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnDel.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.BtnDel.Location = new System.Drawing.Point(152, 5);
            this.BtnDel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnDel.Name = "BtnDel";
            this.BtnDel.Size = new System.Drawing.Size(64, 23);
            this.BtnDel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnDel.TabIndex = 21;
            this.BtnDel.Text = "删 除";
            this.BtnDel.Click += new System.EventHandler(this.BtnDel_Click);
            // 
            // chb_IsUse
            // 
            this.chb_IsUse.AutoSize = true;
            this.chb_IsUse.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chb_IsUse.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chb_IsUse.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chb_IsUse.ForeColor = System.Drawing.Color.White;
            this.chb_IsUse.Location = new System.Drawing.Point(424, 71);
            this.chb_IsUse.Name = "chb_IsUse";
            this.chb_IsUse.Size = new System.Drawing.Size(60, 24);
            this.chb_IsUse.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chb_IsUse.TabIndex = 232;
            this.chb_IsUse.Text = "启用";
            // 
            // labelX6
            // 
            this.labelX6.AutoSize = true;
            this.labelX6.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX6.ForeColor = System.Drawing.Color.White;
            this.labelX6.Location = new System.Drawing.Point(364, 32);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(56, 24);
            this.labelX6.TabIndex = 228;
            this.labelX6.Text = "顺序号";
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX3.ForeColor = System.Drawing.Color.White;
            this.labelX3.Location = new System.Drawing.Point(39, 32);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(72, 24);
            this.labelX3.TabIndex = 227;
            this.labelX3.Text = "矿点名称";
            // 
            // txt_Name
            // 
            this.txt_Name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txt_Name.Border.Class = "TextBoxBorder";
            this.txt_Name.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txt_Name.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Name.ForeColor = System.Drawing.Color.White;
            this.txt_Name.Location = new System.Drawing.Point(115, 29);
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Size = new System.Drawing.Size(180, 27);
            this.txt_Name.TabIndex = 226;
            // 
            // txt_ReMark
            // 
            this.txt_ReMark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txt_ReMark.Border.Class = "TextBoxBorder";
            this.txt_ReMark.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txt_ReMark.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_ReMark.ForeColor = System.Drawing.Color.White;
            this.txt_ReMark.Location = new System.Drawing.Point(115, 108);
            this.txt_ReMark.Multiline = true;
            this.txt_ReMark.Name = "txt_ReMark";
            this.txt_ReMark.Size = new System.Drawing.Size(489, 106);
            this.txt_ReMark.TabIndex = 235;
            // 
            // labelX8
            // 
            this.labelX8.AutoSize = true;
            this.labelX8.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX8.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX8.ForeColor = System.Drawing.Color.White;
            this.labelX8.Location = new System.Drawing.Point(70, 110);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(40, 24);
            this.labelX8.TabIndex = 234;
            this.labelX8.Text = "备注";
            // 
            // dbi_Sequence
            // 
            this.dbi_Sequence.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.dbi_Sequence.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dbi_Sequence.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dbi_Sequence.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dbi_Sequence.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.dbi_Sequence.ForeColor = System.Drawing.Color.White;
            this.dbi_Sequence.Location = new System.Drawing.Point(424, 29);
            this.dbi_Sequence.MaxValue = 100000;
            this.dbi_Sequence.MinValue = 0;
            this.dbi_Sequence.Name = "dbi_Sequence";
            this.dbi_Sequence.Size = new System.Drawing.Size(180, 27);
            this.dbi_Sequence.TabIndex = 236;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(82)))), ((int)(((byte)(89)))));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelEx2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelEx3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1321, 630);
            this.tableLayoutPanel1.TabIndex = 238;
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx2.Controls.Add(this.btnSubmit);
            this.panelEx2.Controls.Add(this.btnCancel);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx2.Location = new System.Drawing.Point(3, 593);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(1315, 34);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 0;
            // 
            // btnSubmit
            // 
            this.btnSubmit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnSubmit.Location = new System.Drawing.Point(1150, 6);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "保  存";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnCancel.Location = new System.Drawing.Point(1231, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取  消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panelEx3
            // 
            this.panelEx3.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx3.Controls.Add(this.panelLeft);
            this.panelEx3.Controls.Add(this.panelRight);
            this.panelEx3.Controls.Add(this.advTree1);
            this.panelEx3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx3.Location = new System.Drawing.Point(3, 3);
            this.panelEx3.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.panelEx3.Name = "panelEx3";
            this.panelEx3.Size = new System.Drawing.Size(1315, 587);
            this.panelEx3.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx3.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx3.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx3.Style.GradientAngle = 90;
            this.panelEx3.TabIndex = 1;
            // 
            // panelLeft
            // 
            this.panelLeft.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelLeft.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelLeft.Controls.Add(this.BtnAdd);
            this.panelLeft.Controls.Add(this.BtnDel);
            this.panelLeft.Controls.Add(this.BtnEdit);
            this.panelLeft.Location = new System.Drawing.Point(22, 3);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(260, 32);
            this.panelLeft.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelLeft.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelLeft.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelLeft.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelLeft.Style.GradientAngle = 90;
            this.panelLeft.TabIndex = 238;
            // 
            // panelRight
            // 
            this.panelRight.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelRight.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelRight.Controls.Add(this.labelX4);
            this.panelRight.Controls.Add(this.cmbUnLoader_BuyFuel);
            this.panelRight.Controls.Add(this.labelX3);
            this.panelRight.Controls.Add(this.labelX6);
            this.panelRight.Controls.Add(this.txt_Name);
            this.panelRight.Controls.Add(this.dbi_Sequence);
            this.panelRight.Controls.Add(this.chb_IsUse);
            this.panelRight.Controls.Add(this.labelX8);
            this.panelRight.Controls.Add(this.txt_ReMark);
            this.panelRight.Location = new System.Drawing.Point(383, 36);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(632, 232);
            this.panelRight.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelRight.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelRight.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelRight.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelRight.Style.GradientAngle = 90;
            this.panelRight.TabIndex = 237;
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.labelX4.ForeColor = System.Drawing.Color.White;
            this.labelX4.Location = new System.Drawing.Point(32, 68);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(77, 26);
            this.labelX4.TabIndex = 249;
            this.labelX4.Text = "卸煤区域";
            // 
            // cmbUnLoader_BuyFuel
            // 
            this.cmbUnLoader_BuyFuel.DisplayMember = "Text";
            this.cmbUnLoader_BuyFuel.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbUnLoader_BuyFuel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnLoader_BuyFuel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cmbUnLoader_BuyFuel.ForeColor = System.Drawing.Color.White;
            this.cmbUnLoader_BuyFuel.FormattingEnabled = true;
            this.cmbUnLoader_BuyFuel.ItemHeight = 23;
            this.cmbUnLoader_BuyFuel.Location = new System.Drawing.Point(115, 66);
            this.cmbUnLoader_BuyFuel.Name = "cmbUnLoader_BuyFuel";
            this.cmbUnLoader_BuyFuel.Size = new System.Drawing.Size(180, 29);
            this.cmbUnLoader_BuyFuel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbUnLoader_BuyFuel.TabIndex = 248;
            // 
            // FrmMine_List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1321, 630);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1420, 695);
            this.Name = "FrmMine_List";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "矿点";
            this.Shown += new System.EventHandler(this.FrmMine_List_Shown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmMine_List_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbi_Sequence)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelEx2.ResumeLayout(false);
            this.panelEx3.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.AdvTree.AdvTree advTree1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.ButtonX BtnAdd;
        private DevComponents.DotNetBar.ButtonX BtnEdit;
        private DevComponents.DotNetBar.ButtonX BtnDel;
        private DevComponents.DotNetBar.Controls.CheckBoxX chb_IsUse;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_Name;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_ReMark;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.Editors.IntegerInput dbi_Sequence;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.ButtonX btnSubmit;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.PanelEx panelEx3;
        private DevComponents.DotNetBar.PanelEx panelRight;
        private DevComponents.DotNetBar.PanelEx panelLeft;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbUnLoader_BuyFuel;


    }
}