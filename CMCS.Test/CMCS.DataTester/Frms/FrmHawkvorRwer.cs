using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using RW.Hawkvor;
using RW.HawkvorCom;
using System.Net.Sockets;

namespace CMCS.DataTester.Frms
{
    public partial class FrmHawkvorRwer : Form
    {
        public FrmHawkvorRwer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HawkvorRwer rwer = new HawkvorRwer();
            Socket listener= rwer.CreateListening(textBox1.Text, Convert.ToInt32(textBox2.Text));
            rwer.StartListening(listener,Error);
            rwer.OnReadSucess += new HawkvorRwer.ReadSucessHandler(OnReadSuccess);
        }

        private void OnReadSuccess(string rfid)
        {
            textBox3.Text = rfid;
        }

        private void Error(string error)
        {
            textBox3.Text = error;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HawkvorComRwer rw = new HawkvorComRwer();
            rw.OpenCom(Convert.ToInt32(textBox4.Text),Convert.ToInt32(textBox5.Text));
            rw.OnReadSucess += new HawkvorComRwer.ReadSucessHandler(OnReadSuccess1);
            rw.OnScanError += new HawkvorComRwer.ScanErrorEventHandler(Error1);
        }
        private void OnReadSuccess1(string rfid)
        {
            textBox3.Text = rfid;
        }

        private void Error1(string error)
        {
            textBox3.Text = error;
        }
    }
}
