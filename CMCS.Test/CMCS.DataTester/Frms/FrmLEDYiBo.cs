using LED.YIBO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CMCS.DataTester.Frms
{
    public partial class FrmLEDYiBo : Form
    {
        public FrmLEDYiBo()
        {
            InitializeComponent();
        }
        YiBoDD251 led = new YiBoDD251();
        private void button1_Click(object sender, EventArgs e)
        {
            led.Send(this.textBox1.Text, this.textBox4.Text);
        }

        private void FrmLEDYiBo_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = "456";
            this.textBox2.Text = "192.168.8.18";
            this.textBox3.Text = "2002";
            this.textBox4.Text = "20KG";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            led.CreateListening(this.textBox2.Text, Convert.ToInt32(this.textBox3.Text));
        }
    }
}
