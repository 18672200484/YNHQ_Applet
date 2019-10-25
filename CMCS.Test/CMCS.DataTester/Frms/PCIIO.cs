using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using IOC.PCI1761;

namespace CMCS.DataTester.Frms
{
    public partial class PCIIO : Form
    {
        PCI1761Iocer iocer = new PCI1761Iocer();
        public PCIIO()
        {
            InitializeComponent();
        }

        private void PCIIO_Load(object sender, EventArgs e)
        {
            iocer.OnReceived += iocer_OnReceived;
            if (iocer.OpenCom())
            {
                MessageBox.Show(this, "打开成功");
            }
            else
            {
                MessageBox.Show(this, "打开失败");
            }
        }

        void iocer_OnReceived(int[] receiveValue)
        {
            for (int i = 1; i < 9; i++)
            {
                Control[] ss = this.Controls.Find("label" + i.ToString(), true);
                if (ss != null)
                    ss[0].Text = receiveValue[i - 1].ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            iocer.Xihe(Convert.ToInt32(textBox1.Text));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            iocer.Shifang(Convert.ToInt32(textBox1.Text));
        }
    }
}
