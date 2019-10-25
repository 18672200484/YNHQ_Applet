using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using HikVisionSDK.Core;

namespace CMCS.DataTester.Frms
{
    public partial class FrmVideo : Form
    {
        public FrmVideo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPCer iPCer1 = new IPCer();
            IPCer.InitSDK();
            iPCer1.Login(textBox1.Text,Convert.ToInt32(textBox2.Text),textBox3.Text,textBox4.Text);
            iPCer1.StartPreview(panel1.Handle,Convert.ToInt32(textBox5.Text));
        }
    }
}
