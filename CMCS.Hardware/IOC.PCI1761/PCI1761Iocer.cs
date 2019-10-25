using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using Automation.BDaq;

namespace IOC.PCI1761
{
    /// <summary>
    /// PCI1761控制卡 吸合之后要释放 不然道闸不会下降
    /// </summary>
    public class PCI1761Iocer
    {
        /// <summary>
        /// 写IO卡操作对象
        /// </summary>
        private InstantDoCtrl InsDoCtrl;

        /// <summary>
        /// 读IO卡操作对象
        /// </summary>
        private InstantDiCtrl InsDiCtrl;

        /// <summary>
        /// 控制卡串口
        /// </summary>
        bool[] BoolMark = new bool[8];

        /// <summary>
        /// 控制卡读写状态
        /// </summary>
        ErrorCode err = ErrorCode.Success;

        /// <summary>
        /// 吸合 释放 命令
        /// </summary>
        double portNum = 0;

        public delegate void ReceivedEventHandler(int[] receiveValue);
        public event ReceivedEventHandler OnReceived;
        public delegate void StatusChangeHandler(bool status);
        public event StatusChangeHandler OnStatusChange;

        System.Timers.Timer timer1 = new System.Timers.Timer();
        /// <summary>
        /// 接收到的数据
        /// </summary>
        public int[] receiveport = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };

        private bool status = false;
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool Status
        {
            get { return status; }
        }

        private string statusmessage = string.Empty;
        /// <summary>
        /// 连接说明
        /// </summary>
        public string StatusMessage
        {
            get { return statusmessage; }
            set { value = statusmessage; }
        }

        /// <summary>
        /// 设置连接状态
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(bool status)
        {
            if (this.status != status && this.OnStatusChange != null) this.OnStatusChange(status);
            this.status = status;
        }

        public PCI1761Iocer()
        {
            timer1.Interval = 100;
            timer1.Elapsed += timer1_Elapsed;
        }

        /// <summary>
        /// 接收信号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            for (int l = 0; l < 8; ++l)
            {
                byte portData = 0;
                ErrorCode err = ErrorCode.Success;
                err = InsDiCtrl.Read(0, out portData);
                receiveport[l] = ((portData >> l) & 0x1);
            }
            if (this.OnReceived != null) OnReceived(receiveport);
        }

        public bool OpenCom()
        {
            try
            {
                InsDoCtrl = new InstantDoCtrl();
                InsDoCtrl.SelectedDevice = new DeviceInformation(0);
                InsDiCtrl = new InstantDiCtrl();
                InsDiCtrl.SelectedDevice = new DeviceInformation(0);

                if (InsDoCtrl.Initialized)
                {
                    this.StatusMessage = "IO卡打开成功";
                }
                else
                {
                    this.StatusMessage = "IO卡打开失败或者没有检测到IO卡";
                }
                if (InsDiCtrl.Initialized)
                {
                    this.StatusMessage = "IO卡读对象打开成功";
                    SetStatus(true);
                    timer1.Enabled = true;
                }
                else
                {
                    this.StatusMessage = "IO卡读对象打开失败";
                }
            }
            catch (Exception ee)
            {
                this.status = false;
                if (this.OnStatusChange != null) this.OnStatusChange(status);
                this.StatusMessage = ee.Message.ToString();
            }

            return this.status;
        }

        public bool CloseCom()
        {
            timer1.Enabled = false;
            return true;
        }

        private void GetXihePortNum(int port)
        {
            portNum += System.Math.Pow(2, port);
        }

        private void GetShifangPortNum(int port)
        {
            portNum -= System.Math.Pow(2, port);
        }
        /// <summary>
        /// 吸合
        /// </summary>
        /// <param name="port"></param>
        public void Xihe(int port)
        {
            if (BoolMark[port] == false)
            {
                GetXihePortNum(port);
                int state = Convert.ToInt32(portNum);
                err = InsDoCtrl.Write(0, (byte)state);

                if (err == ErrorCode.Success)
                {
                    BoolMark[port] = true;
                }
            }
        }
        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="port"></param>
        public void Shifang(int port)
        {
            if (BoolMark[port])
            {
                GetShifangPortNum(port);
                int state = int.Parse(portNum.ToString());
                if (InsDoCtrl.Write(0, (byte)state) == ErrorCode.Success)
                {
                    //System.Console.WriteLine(portNum.ToString() + ":" + state.ToString());
                    BoolMark[port] = false;
                }
            }
        }
    }
}
