namespace CMCS.CarTransport.Queue.Frms
{
    partial class QueueMessageBox
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
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.btnSubmit = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.txtUnLoadArea = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtFuelKind = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtUnLoadType = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtQch = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtSupplierName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtMineName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtCarNumber = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtTicketWeight = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX12 = new DevComponents.DotNetBar.LabelX();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelEx1.SuspendLayout();
            this.panelEx2.SuspendLayout();
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(586, 225);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.btnSubmit);
            this.panelEx1.Controls.Add(this.btnCancel);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(3, 188);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(580, 34);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 1;
            // 
            // btnSubmit
            // 
            this.btnSubmit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnSubmit.Location = new System.Drawing.Point(388, 7);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSubmit.TabIndex = 3;
            this.btnSubmit.Text = "确  认";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnCancel.Location = new System.Drawing.Point(469, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取  消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx2.Controls.Add(this.txtUnLoadArea);
            this.panelEx2.Controls.Add(this.txtFuelKind);
            this.panelEx2.Controls.Add(this.txtUnLoadType);
            this.panelEx2.Controls.Add(this.txtQch);
            this.panelEx2.Controls.Add(this.txtSupplierName);
            this.panelEx2.Controls.Add(this.txtMineName);
            this.panelEx2.Controls.Add(this.txtCarNumber);
            this.panelEx2.Controls.Add(this.txtTicketWeight);
            this.panelEx2.Controls.Add(this.labelX7);
            this.panelEx2.Controls.Add(this.labelX6);
            this.panelEx2.Controls.Add(this.labelX5);
            this.panelEx2.Controls.Add(this.labelX4);
            this.panelEx2.Controls.Add(this.labelX3);
            this.panelEx2.Controls.Add(this.labelX2);
            this.panelEx2.Controls.Add(this.labelX1);
            this.panelEx2.Controls.Add(this.labelX12);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx2.Location = new System.Drawing.Point(3, 3);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(580, 179);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 2;
            // 
            // txtUnLoadArea
            // 
            this.txtUnLoadArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtUnLoadArea.Border.Class = "TextBoxBorder";
            this.txtUnLoadArea.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtUnLoadArea.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUnLoadArea.ForeColor = System.Drawing.Color.White;
            this.txtUnLoadArea.Location = new System.Drawing.Point(98, 131);
            this.txtUnLoadArea.Name = "txtUnLoadArea";
            this.txtUnLoadArea.ReadOnly = true;
            this.txtUnLoadArea.Size = new System.Drawing.Size(180, 27);
            this.txtUnLoadArea.TabIndex = 252;
            // 
            // txtFuelKind
            // 
            this.txtFuelKind.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtFuelKind.Border.Class = "TextBoxBorder";
            this.txtFuelKind.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtFuelKind.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFuelKind.ForeColor = System.Drawing.Color.White;
            this.txtFuelKind.Location = new System.Drawing.Point(98, 96);
            this.txtFuelKind.Name = "txtFuelKind";
            this.txtFuelKind.ReadOnly = true;
            this.txtFuelKind.Size = new System.Drawing.Size(180, 27);
            this.txtFuelKind.TabIndex = 255;
            // 
            // txtUnLoadType
            // 
            this.txtUnLoadType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtUnLoadType.Border.Class = "TextBoxBorder";
            this.txtUnLoadType.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtUnLoadType.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUnLoadType.ForeColor = System.Drawing.Color.White;
            this.txtUnLoadType.Location = new System.Drawing.Point(378, 131);
            this.txtUnLoadType.Name = "txtUnLoadType";
            this.txtUnLoadType.ReadOnly = true;
            this.txtUnLoadType.Size = new System.Drawing.Size(180, 27);
            this.txtUnLoadType.TabIndex = 254;
            // 
            // txtQch
            // 
            this.txtQch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtQch.Border.Class = "TextBoxBorder";
            this.txtQch.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtQch.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQch.ForeColor = System.Drawing.Color.White;
            this.txtQch.Location = new System.Drawing.Point(378, 96);
            this.txtQch.Name = "txtQch";
            this.txtQch.ReadOnly = true;
            this.txtQch.Size = new System.Drawing.Size(180, 27);
            this.txtQch.TabIndex = 253;
            // 
            // txtSupplierName
            // 
            this.txtSupplierName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtSupplierName.Border.Class = "TextBoxBorder";
            this.txtSupplierName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSupplierName.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSupplierName.ForeColor = System.Drawing.Color.White;
            this.txtSupplierName.Location = new System.Drawing.Point(378, 57);
            this.txtSupplierName.Name = "txtSupplierName";
            this.txtSupplierName.ReadOnly = true;
            this.txtSupplierName.Size = new System.Drawing.Size(180, 27);
            this.txtSupplierName.TabIndex = 256;
            // 
            // txtMineName
            // 
            this.txtMineName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtMineName.Border.Class = "TextBoxBorder";
            this.txtMineName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtMineName.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMineName.ForeColor = System.Drawing.Color.White;
            this.txtMineName.Location = new System.Drawing.Point(98, 61);
            this.txtMineName.Name = "txtMineName";
            this.txtMineName.ReadOnly = true;
            this.txtMineName.Size = new System.Drawing.Size(180, 27);
            this.txtMineName.TabIndex = 259;
            // 
            // txtCarNumber
            // 
            this.txtCarNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtCarNumber.Border.Class = "TextBoxBorder";
            this.txtCarNumber.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCarNumber.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCarNumber.ForeColor = System.Drawing.Color.White;
            this.txtCarNumber.Location = new System.Drawing.Point(98, 22);
            this.txtCarNumber.Name = "txtCarNumber";
            this.txtCarNumber.ReadOnly = true;
            this.txtCarNumber.Size = new System.Drawing.Size(180, 27);
            this.txtCarNumber.TabIndex = 258;
            // 
            // txtTicketWeight
            // 
            this.txtTicketWeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            // 
            // 
            // 
            this.txtTicketWeight.Border.Class = "TextBoxBorder";
            this.txtTicketWeight.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtTicketWeight.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTicketWeight.ForeColor = System.Drawing.Color.White;
            this.txtTicketWeight.Location = new System.Drawing.Point(378, 22);
            this.txtTicketWeight.Name = "txtTicketWeight";
            this.txtTicketWeight.ReadOnly = true;
            this.txtTicketWeight.Size = new System.Drawing.Size(180, 27);
            this.txtTicketWeight.TabIndex = 257;
            // 
            // labelX7
            // 
            this.labelX7.AutoSize = true;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX7.ForeColor = System.Drawing.Color.White;
            this.labelX7.Location = new System.Drawing.Point(22, 134);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(70, 24);
            this.labelX7.TabIndex = 246;
            this.labelX7.Text = "卸煤区域";
            // 
            // labelX6
            // 
            this.labelX6.AutoSize = true;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX6.ForeColor = System.Drawing.Color.White;
            this.labelX6.Location = new System.Drawing.Point(302, 130);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(70, 24);
            this.labelX6.TabIndex = 247;
            this.labelX6.Text = "卸煤方式";
            // 
            // labelX5
            // 
            this.labelX5.AutoSize = true;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX5.ForeColor = System.Drawing.Color.White;
            this.labelX5.Location = new System.Drawing.Point(53, 99);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(39, 24);
            this.labelX5.TabIndex = 244;
            this.labelX5.Text = "煤种";
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX4.ForeColor = System.Drawing.Color.White;
            this.labelX4.Location = new System.Drawing.Point(333, 95);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(39, 24);
            this.labelX4.TabIndex = 245;
            this.labelX4.Text = "地磅";
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX3.ForeColor = System.Drawing.Color.White;
            this.labelX3.Location = new System.Drawing.Point(302, 57);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(70, 24);
            this.labelX3.TabIndex = 248;
            this.labelX3.Text = "供煤单位";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX2.ForeColor = System.Drawing.Color.White;
            this.labelX2.Location = new System.Drawing.Point(53, 64);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(39, 24);
            this.labelX2.TabIndex = 251;
            this.labelX2.Text = "矿点";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX1.ForeColor = System.Drawing.Color.White;
            this.labelX1.Location = new System.Drawing.Point(38, 25);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(54, 24);
            this.labelX1.TabIndex = 250;
            this.labelX1.Text = "车牌号";
            // 
            // labelX12
            // 
            this.labelX12.AutoSize = true;
            // 
            // 
            // 
            this.labelX12.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX12.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX12.ForeColor = System.Drawing.Color.White;
            this.labelX12.Location = new System.Drawing.Point(333, 21);
            this.labelX12.Name = "labelX12";
            this.labelX12.Size = new System.Drawing.Size(39, 24);
            this.labelX12.TabIndex = 249;
            this.labelX12.Text = "票重";
            // 
            // QueueMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 225);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QueueMessageBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "信息确认";
            this.Shown += new System.EventHandler(this.QueueMessageBox_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.panelEx2.ResumeLayout(false);
            this.panelEx2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ButtonX btnSubmit;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUnLoadArea;
        private DevComponents.DotNetBar.Controls.TextBoxX txtFuelKind;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUnLoadType;
        private DevComponents.DotNetBar.Controls.TextBoxX txtQch;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSupplierName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMineName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCarNumber;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTicketWeight;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX12;
    }
}