using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CMCS.CarTransport.Queue.Frms
{
    public partial class QueueMessageBox : DevComponents.DotNetBar.Metro.MetroForm
    {
        public QueueMessageBox()
        {
            InitializeComponent();
        }

        private void QueueMessageBox_Shown(object sender, EventArgs e)
        {
        }
        public QueueMessageBox(string carnumber, string ticketweith, string minename, string suppliername, string fuelkind, string qch, string unloadarea, string unloadtype)
        {
            InitializeComponent();
            this.txtCarNumber.Text = carnumber;
            this.txtTicketWeight.Text = ticketweith;
            this.txtMineName.Text = minename;
            this.txtSupplierName.Text = suppliername;
            this.txtFuelKind.Text = fuelkind;
            this.txtQch.Text = qch;
            this.txtUnLoadArea.Text = unloadarea;
            this.txtUnLoadType.Text = unloadtype;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
