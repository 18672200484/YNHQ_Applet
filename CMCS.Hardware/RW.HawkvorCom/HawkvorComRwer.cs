using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.HawkvorCom
{
    public class HawkvorComRwer
    {
        private bool status = false;
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool Status
        {
            get { return status; }
        }

        /// <summary>
        /// 临时数据
        /// </summary>
        private List<byte> ReceiveList = new List<byte>();

        public delegate void StatusChangeHandler(bool status);
        public event StatusChangeHandler OnStatusChange;

        public delegate void ReadSucessHandler(string rfid);
        public event ReadSucessHandler OnReadSucess;

        public delegate void ScanErrorEventHandler(string error);
        public event ScanErrorEventHandler OnScanError;
       
        private SerialPort serialPort = new SerialPort();
        /// <summary>
        /// 打开串口
        /// 成功返回True;失败返回False;
        /// </summary>
        /// <param name="com">串口号</param>
        /// <param name="bandrate">波特率</param>
        /// <param name="dataBits">数据位</param>
        /// <param name="parity">校验</param>
        /// <returns></returns>
        public bool OpenCom(int com, int bandrate)
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.PortName = "COM" + com.ToString();
                    serialPort.BaudRate = bandrate;
                    serialPort.DataBits = 8;
                    serialPort.StopBits = StopBits.One;
                    serialPort.Parity = Parity.None;
                    serialPort.ReceivedBytesThreshold = 1;
                    serialPort.RtsEnable = true;
                    serialPort.Open();
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);

                    SetStatus(true);

                    this.Closing = false;
                }
            }
            catch (Exception ex)
            {
                this.status = false;
                if (this.OnStatusChange != null) this.OnStatusChange(status);
                if (this.OnScanError != null) this.OnScanError(ex.Message);
            }

            return this.status;
        }

        /// <summary>
        /// 关闭串口
        /// 成功返回True;失败返回False;
        /// </summary>
        /// <returns></returns>
        public void CloseCom()
        {
            try
            {
                this.Closing = true;

                serialPort.DataReceived -= new SerialDataReceivedEventHandler(serialPort_DataReceived);
                serialPort.Close();

                SetStatus(false);
            }
            catch { }
        }
        bool Closing = false;

        /// <summary>
        /// 设置连接状态
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(bool status)
        {
            if (this.status != status && this.OnStatusChange != null) this.OnStatusChange(status);
            this.status = status;
        }

        /// <summary>
        /// 串口接收数据
        /// 数据示例：02 00 00 12 3F 07 20 72 4C 78 D5 38 33 0D 0A 03
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (this.Closing) return;

            if (serialPort.IsOpen)
            {
                byte[] buffer = new byte[serialPort.BytesToRead];
                serialPort.Read(buffer, 0, buffer.Length);

                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] == 0x12) ReceiveList.Clear();

                    ReceiveList.Add(buffer[i]);

                    try
                    {
                        if (buffer[i] == 0x0D && ReceiveList.Count == 11)
                        {
                            string temp = string.Empty;
                            for (int j = 0; j < 10; j++)
                            {
                                temp += string.Format("{0:x2}", ReceiveList[j]);
                            }
                            if (!string.IsNullOrEmpty(temp) && this.OnReadSucess != null) OnReadSucess(temp.ToUpper());
                            ReceiveList.Clear();
                        }
                    }
                    catch { }
                }
            }
        }

    }
}
