namespace CMCS.CarTransport.Weighter.Frms.BaseInfo.FuelKind
{
    partial class FrmFuelKind_List
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.btnSubmit = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.panelEx3 = new DevComponents.DotNetBar.PanelEx();
            this.panelLeft = new DevComponents.DotNetBar.PanelEx();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.BtnDel = new DevComponents.DotNetBar.ButtonX();
            this.buttonX3 = new DevComponents.DotNetBar.ButtonX();
            this.panelRight = new DevComponents.DotNetBar.PanelEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txt_FuelName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.dbi_Sequence = new DevComponents.Editors.IntegerInput();
            this.chb_IsUse = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.txt_ReMark = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.advTree1 = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector2 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle2 = new DevComponents.DotNetBar.ElementStyle();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelEx2.SuspendLayout();
            this.panelEx3.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dbi_Sequence)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelEx2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelEx3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(999, 540);
            this.tableLayoutPanel1.TabIndex = 240;
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx2.Controls.Add(this.btnSubmit);
            this.panelEx2.Controls.Add(this.btnCancel);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx2.Location = new System.Drawing.Point(3, 503);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(993, 34);
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
            this.btnSubmit.Location = new System.Drawing.Point(828, 6);
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
            this.btnCancel.Location = new System.Drawing.Point(909, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取  消";
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
            this.panelEx3.Size = new System.Drawing.Size(993, 497);
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
            this.panelLeft.Controls.Add(this.buttonX1);
            this.panelLeft.Controls.Add(this.BtnDel);
            this.panelLeft.Controls.Add(this.buttonX3);
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
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.buttonX1.Location = new System.Drawing.Point(12, 5);
            this.buttonX1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(64, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 19;
            this.buttonX1.Text = "新 增";
            this.buttonX1.Click += new System.EventHandler(this.BtnAdd_Click);
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
            this.BtnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // buttonX3
            // 
            this.buttonX3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX3.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.buttonX3.Location = new System.Drawing.Point(82, 5);
            this.buttonX3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonX3.Name = "buttonX3";
            this.buttonX3.Size = new System.Drawing.Size(64, 23);
            this.buttonX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX3.TabIndex = 20;
            this.buttonX3.Text = "修 改";
            this.buttonX3.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // panelRight
            // 
            this.panelRight.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelRight.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelRight.Controls.Add(this.labelX1);
            this.panelRight.Controls.Add(this.labelX2);
            this.panelRight.Controls.Add(this.txt_FuelName);
            this.panelRight.Controls.Add(this.dbi_Sequence);
            this.panelRight.Controls.Add(this.chb_IsUse);
            this.panelRight.Controls.Add(this.labelX4);
            this.panelRight.Controls.Add(this.txt_ReMark);
            this.panelRight.Location = new System.Drawing.Point(304, 36);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(632, 232);
            this.panelRight.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelRight.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelRight.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelRight.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelRight.Style.GradientAngle = 90;
            this.panelRight.TabIndex = 237;
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Location = new System.Drawing.Point(39, 32);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(70, 24);
            this.labelX1.TabIndex = 227;
            this.labelX1.Text = "煤种名称";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX2.ForeColor = System.Drawing.Color.Black;
            this.labelX2.Location = new System.Drawing.Point(364, 32);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(54, 24);
            this.labelX2.TabIndex = 228;
            this.labelX2.Text = "顺序号";
            // 
            // txt_FuelName
            // 
            this.txt_FuelName.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txt_FuelName.Border.Class = "TextBoxBorder";
            this.txt_FuelName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txt_FuelName.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_FuelName.ForeColor = System.Drawing.Color.Black;
            this.txt_FuelName.Location = new System.Drawing.Point(115, 29);
            this.txt_FuelName.Name = "txt_FuelName";
            this.txt_FuelName.Size = new System.Drawing.Size(180, 27);
            this.txt_FuelName.TabIndex = 226;
            // 
            // dbi_Sequence
            // 
            this.dbi_Sequence.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.dbi_Sequence.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dbi_Sequence.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dbi_Sequence.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dbi_Sequence.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.dbi_Sequence.ForeColor = System.Drawing.Color.Black;
            this.dbi_Sequence.Location = new System.Drawing.Point(424, 29);
            this.dbi_Sequence.MaxValue = 100000;
            this.dbi_Sequence.MinValue = 0;
            this.dbi_Sequence.Name = "dbi_Sequence";
            this.dbi_Sequence.Size = new System.Drawing.Size(180, 27);
            this.dbi_Sequence.TabIndex = 236;
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
            this.chb_IsUse.ForeColor = System.Drawing.Color.Black;
            this.chb_IsUse.Location = new System.Drawing.Point(115, 62);
            this.chb_IsUse.Name = "chb_IsUse";
            this.chb_IsUse.Size = new System.Drawing.Size(59, 24);
            this.chb_IsUse.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chb_IsUse.TabIndex = 232;
            this.chb_IsUse.Text = "启用";
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX4.ForeColor = System.Drawing.Color.Black;
            this.labelX4.Location = new System.Drawing.Point(70, 97);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(39, 24);
            this.labelX4.TabIndex = 234;
            this.labelX4.Text = "备注";
            // 
            // txt_ReMark
            // 
            this.txt_ReMark.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txt_ReMark.Border.Class = "TextBoxBorder";
            this.txt_ReMark.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txt_ReMark.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_ReMark.ForeColor = System.Drawing.Color.Black;
            this.txt_ReMark.Location = new System.Drawing.Point(115, 95);
            this.txt_ReMark.Multiline = true;
            this.txt_ReMark.Name = "txt_ReMark";
            this.txt_ReMark.Size = new System.Drawing.Size(489, 106);
            this.txt_ReMark.TabIndex = 235;
            // 
            // advTree1
            // 
            this.advTree1.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTree1.AllowDrop = true;
            this.advTree1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.advTree1.BackgroundStyle.Class = "TreeBorderKey";
            this.advTree1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTree1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.advTree1.ForeColor = System.Drawing.Color.Black;
            this.advTree1.Location = new System.Drawing.Point(22, 36);
            this.advTree1.Name = "advTree1";
            this.advTree1.NodesConnector = this.nodeConnector2;
            this.advTree1.NodeStyle = this.elementStyle2;
            this.advTree1.PathSeparator = ";";
            this.advTree1.Size = new System.Drawing.Size(260, 440);
            this.advTree1.Styles.Add(this.elementStyle2);
            this.advTree1.TabIndex = 5;
            this.advTree1.Text = "advTree2";
            this.advTree1.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTree1_NodeClick);
            this.advTree1.NodeDoubleClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTree1_NodeDoubleClick);
            // 
            // nodeConnector2
            // 
            this.nodeConnector2.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            // 
            // elementStyle2
            // 
            this.elementStyle2.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle2.Name = "elementStyle2";
            this.elementStyle2.TextColor = System.Drawing.Color.Black;
            // 
            // FrmFuelKind_List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(999, 540);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1420, 695);
            this.Name = "FrmFuelKind_List";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "煤种";
            this.Shown += new System.EventHandler(this.FrmFuelKind_List_Shown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmFuelKind_List_KeyUp);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelEx2.ResumeLayout(false);
            this.panelEx3.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dbi_Sequence)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.ButtonX btnSubmit;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.PanelEx panelEx3;
        private DevComponents.DotNetBar.PanelEx panelLeft;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.ButtonX BtnDel;
        private DevComponents.DotNetBar.ButtonX buttonX3;
        private DevComponents.DotNetBar.PanelEx panelRight;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_FuelName;
        private DevComponents.Editors.IntegerInput dbi_Sequence;
        private DevComponents.DotNetBar.Controls.CheckBoxX chb_IsUse;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_ReMark;
        private DevComponents.AdvTree.AdvTree advTree1;
        private DevComponents.AdvTree.NodeConnector nodeConnector2;
        private DevComponents.DotNetBar.ElementStyle elementStyle2;


    }
}