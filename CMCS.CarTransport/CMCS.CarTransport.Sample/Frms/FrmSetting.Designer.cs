namespace CMCS.CarTransport.Sample.Frms
{
    partial class FrmSetting
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.btnSubmit = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.tabControl1 = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel4 = new DevComponents.DotNetBar.TabControlPanel();
            this.cmbBote = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbRwer1Com = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX28 = new DevComponents.DotNetBar.LabelX();
            this.tabItem4 = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.txtSelfConnStr = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX20 = new DevComponents.DotNetBar.LabelX();
            this.chbStartup = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.txtAppIdentifier = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.tabItem1 = new DevComponents.DotNetBar.TabItem(this.components);
            this.chbisLandscape = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelEx1.SuspendLayout();
            this.panelEx2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabControlPanel4.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(82)))), ((int)(((byte)(89)))));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelEx1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelEx2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(709, 506);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.btnSubmit);
            this.panelEx1.Controls.Add(this.btnCancel);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(3, 469);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(703, 34);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // btnSubmit
            // 
            this.btnSubmit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnSubmit.Location = new System.Drawing.Point(538, 6);
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
            this.btnCancel.Location = new System.Drawing.Point(619, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取  消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx2.Controls.Add(this.tabControl1);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx2.Location = new System.Drawing.Point(3, 3);
            this.panelEx2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(703, 463);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.tabControl1.CanReorderTabs = false;
            this.tabControl1.ColorScheme.TabItemBackgroundColorBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.Empty, 0F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.Empty, 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.Empty, 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.Empty, 1F)});
            this.tabControl1.ColorScheme.TabItemBorder = System.Drawing.Color.Empty;
            this.tabControl1.ColorScheme.TabItemBorderDark = System.Drawing.Color.Empty;
            this.tabControl1.ColorScheme.TabItemHotBackgroundColorBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.Empty, 0F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.Empty, 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.Empty, 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.Empty, 1F)});
            this.tabControl1.ColorScheme.TabItemSelectedBackgroundColorBlend.AddRange(new DevComponents.DotNetBar.BackgroundColorBlend[] {
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.Empty, 0F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.Empty, 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.Empty, 0.45F),
            new DevComponents.DotNetBar.BackgroundColorBlend(System.Drawing.Color.Empty, 1F)});
            this.tabControl1.ColorScheme.TabItemSelectedBorder = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(188)))), ((int)(((byte)(204)))));
            this.tabControl1.ColorScheme.TabItemSelectedBorderDark = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(188)))), ((int)(((byte)(204)))));
            this.tabControl1.ColorScheme.TabItemSelectedBorderLight = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(188)))), ((int)(((byte)(204)))));
            this.tabControl1.ColorScheme.TabPanelBorder = System.Drawing.Color.Empty;
            this.tabControl1.Controls.Add(this.tabControlPanel1);
            this.tabControl1.Controls.Add(this.tabControlPanel4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ForeColor = System.Drawing.Color.White;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedTabFont = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.SelectedTabIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(703, 463);
            this.tabControl1.Style = DevComponents.DotNetBar.eTabStripStyle.Metro;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.MultilineNoNavigationBox;
            this.tabControl1.Tabs.Add(this.tabItem1);
            this.tabControl1.Tabs.Add(this.tabItem4);
            this.tabControl1.TabStop = false;
            // 
            // tabControlPanel4
            // 
            this.tabControlPanel4.Controls.Add(this.cmbBote);
            this.tabControlPanel4.Controls.Add(this.cmbRwer1Com);
            this.tabControlPanel4.Controls.Add(this.labelX1);
            this.tabControlPanel4.Controls.Add(this.labelX28);
            this.tabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel4.Location = new System.Drawing.Point(0, 31);
            this.tabControlPanel4.Name = "tabControlPanel4";
            this.tabControlPanel4.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel4.Size = new System.Drawing.Size(703, 432);
            this.tabControlPanel4.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.tabControlPanel4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel4.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel4.Style.GradientAngle = 90;
            this.tabControlPanel4.TabIndex = 4;
            this.tabControlPanel4.TabItem = this.tabItem4;
            // 
            // cmbBote
            // 
            this.cmbBote.DisplayMember = "Text";
            this.cmbBote.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbBote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBote.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBote.ForeColor = System.Drawing.Color.White;
            this.cmbBote.FormattingEnabled = true;
            this.cmbBote.ItemHeight = 21;
            this.cmbBote.Location = new System.Drawing.Point(488, 29);
            this.cmbBote.Name = "cmbBote";
            this.cmbBote.Size = new System.Drawing.Size(92, 27);
            this.cmbBote.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbBote.TabIndex = 28;
            // 
            // cmbRwer1Com
            // 
            this.cmbRwer1Com.DisplayMember = "Text";
            this.cmbRwer1Com.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbRwer1Com.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRwer1Com.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRwer1Com.ForeColor = System.Drawing.Color.White;
            this.cmbRwer1Com.FormattingEnabled = true;
            this.cmbRwer1Com.ItemHeight = 21;
            this.cmbRwer1Com.Location = new System.Drawing.Point(167, 29);
            this.cmbRwer1Com.Name = "cmbRwer1Com";
            this.cmbRwer1Com.Size = new System.Drawing.Size(92, 27);
            this.cmbRwer1Com.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbRwer1Com.TabIndex = 28;
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
            this.labelX1.ForeColor = System.Drawing.Color.White;
            this.labelX1.Location = new System.Drawing.Point(377, 29);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(105, 24);
            this.labelX1.TabIndex = 27;
            this.labelX1.Text = "读卡器波特率";
            // 
            // labelX28
            // 
            this.labelX28.AutoSize = true;
            this.labelX28.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX28.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX28.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX28.ForeColor = System.Drawing.Color.White;
            this.labelX28.Location = new System.Drawing.Point(67, 31);
            this.labelX28.Name = "labelX28";
            this.labelX28.Size = new System.Drawing.Size(88, 24);
            this.labelX28.TabIndex = 27;
            this.labelX28.Text = "读卡器串口";
            // 
            // tabItem4
            // 
            this.tabItem4.AttachedControl = this.tabControlPanel4;
            this.tabItem4.Name = "tabItem4";
            this.tabItem4.Text = "读卡器";
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.txtSelfConnStr);
            this.tabControlPanel1.Controls.Add(this.labelX20);
            this.tabControlPanel1.Controls.Add(this.chbisLandscape);
            this.tabControlPanel1.Controls.Add(this.chbStartup);
            this.tabControlPanel1.Controls.Add(this.txtAppIdentifier);
            this.tabControlPanel1.Controls.Add(this.labelX4);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 31);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(703, 432);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.GradientAngle = 90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabItem1;
            // 
            // txtSelfConnStr
            // 
            this.txtSelfConnStr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtSelfConnStr.Border.Class = "TextBoxBorder";
            this.txtSelfConnStr.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSelfConnStr.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSelfConnStr.ForeColor = System.Drawing.Color.White;
            this.txtSelfConnStr.Location = new System.Drawing.Point(167, 67);
            this.txtSelfConnStr.Name = "txtSelfConnStr";
            this.txtSelfConnStr.Size = new System.Drawing.Size(446, 27);
            this.txtSelfConnStr.TabIndex = 20;
            // 
            // labelX20
            // 
            this.labelX20.AutoSize = true;
            this.labelX20.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX20.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX20.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX20.ForeColor = System.Drawing.Color.White;
            this.labelX20.Location = new System.Drawing.Point(30, 69);
            this.labelX20.Name = "labelX20";
            this.labelX20.Size = new System.Drawing.Size(137, 24);
            this.labelX20.TabIndex = 19;
            this.labelX20.Text = "数据库连接字符串";
            // 
            // chbStartup
            // 
            this.chbStartup.AutoSize = true;
            this.chbStartup.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chbStartup.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chbStartup.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbStartup.ForeColor = System.Drawing.Color.White;
            this.chbStartup.Location = new System.Drawing.Point(167, 105);
            this.chbStartup.Name = "chbStartup";
            this.chbStartup.Size = new System.Drawing.Size(92, 24);
            this.chbStartup.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chbStartup.TabIndex = 18;
            this.chbStartup.Text = "开机启动";
            // 
            // txtAppIdentifier
            // 
            this.txtAppIdentifier.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtAppIdentifier.Border.Class = "TextBoxBorder";
            this.txtAppIdentifier.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtAppIdentifier.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAppIdentifier.ForeColor = System.Drawing.Color.White;
            this.txtAppIdentifier.Location = new System.Drawing.Point(167, 29);
            this.txtAppIdentifier.Name = "txtAppIdentifier";
            this.txtAppIdentifier.Size = new System.Drawing.Size(180, 27);
            this.txtAppIdentifier.TabIndex = 17;
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
            this.labelX4.ForeColor = System.Drawing.Color.White;
            this.labelX4.Location = new System.Drawing.Point(61, 31);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(105, 24);
            this.labelX4.TabIndex = 16;
            this.labelX4.Text = "程序唯一标识";
            // 
            // tabItem1
            // 
            this.tabItem1.AttachedControl = this.tabControlPanel1;
            this.tabItem1.Name = "tabItem1";
            this.tabItem1.Text = "基础设置";
            // 
            // chbisLandscape
            // 
            this.chbisLandscape.AutoSize = true;
            this.chbisLandscape.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chbisLandscape.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chbisLandscape.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbisLandscape.ForeColor = System.Drawing.Color.White;
            this.chbisLandscape.Location = new System.Drawing.Point(167, 135);
            this.chbisLandscape.Name = "chbisLandscape";
            this.chbisLandscape.Size = new System.Drawing.Size(92, 24);
            this.chbisLandscape.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chbisLandscape.TabIndex = 18;
            this.chbisLandscape.Text = "横向打印";
            // 
            // FrmSetting
            // 
            this.AcceptButton = this.btnSubmit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(709, 506);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.Name = "FrmSetting";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "参数设置";
            this.Load += new System.EventHandler(this.FrmSetting_Load);
            this.Shown += new System.EventHandler(this.FrmSetting_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.panelEx2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabControlPanel4.ResumeLayout(false);
            this.tabControlPanel4.PerformLayout();
            this.tabControlPanel1.ResumeLayout(false);
            this.tabControlPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.ButtonX btnSubmit;
        private DevComponents.DotNetBar.TabControl tabControl1;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tabItem1;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel4;
        private DevComponents.DotNetBar.TabItem tabItem4;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbRwer1Com;
        private DevComponents.DotNetBar.LabelX labelX28;
        private DevComponents.DotNetBar.Controls.CheckBoxX chbStartup;
        private DevComponents.DotNetBar.Controls.TextBoxX txtAppIdentifier;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSelfConnStr;
        private DevComponents.DotNetBar.LabelX labelX20;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbBote;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.CheckBoxX chbisLandscape;
    }
}