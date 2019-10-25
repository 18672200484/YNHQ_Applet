using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.IO.Ports;
using System.Threading;

namespace Wagon_Meter_TOLEDO_IND231
{
    /// <summary>
    /// 地磅 For 托利多
    /// 型号：IND231(60KG)
    /// 
    /// </summary>
    public class Wagon_Meter
    {
        private SerialPort port = new SerialPort();
        private System.Timers.Timer timer1 = new System.Timers.Timer(1000);

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool State = false;

        /// <summary>
        /// 数据接收次数
        /// </summary>
        private int ReceiveCount = 0;

        /// <summary>
        /// 重量
        /// </summary>
        public double Value = 0;

        /// <summary>
        /// 上一次重量
        /// </summary>
        private double LastValue = 0;

        /// <summary>
        /// 稳定状态
        /// </summary>
        public bool SteadyState = false;

        /// <summary>
        /// 稳定时长（单位：秒）
        /// </summary>
        private int SteadySecond = 2;

        /// <summary>
        /// 当前稳定时长（单位：秒）
        /// </summary>
        private int CurrSteadySecond = 0;

        /// <summary>
        /// 设置稳定时长，默认是2秒（单位：秒）
        /// </summary>
        public void SetSteadySecond(int second)
        {
            SteadySecond = second;
        }

        /// <summary>
        /// 临时数据集
        /// </summary>
        List<byte> ReceiveList = new List<byte>();

        /// <summary>
        /// 打开串口
        /// 成功返回True;失败返回False;
        /// </summary>
        /// <param name="com">串口号</param>
        /// <param name="bandrate">波特率</param>
        /// <returns></returns>
        public bool OpenCom(int com, int bandrate)
        {
            port.PortName = "COM" + com.ToString();
            port.BaudRate = bandrate;
            port.DataBits = 8;
            port.StopBits = StopBits.One;
            port.Parity = Parity.None;
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            port.ReceivedBytesThreshold = 17;
            port.RtsEnable = true;
            try
            {
                port.Open();
                timer1.AutoReset = true;
                timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
                timer1.Enabled = true;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 关闭串口
        /// 成功返回True;失败返回False;
        /// </summary>
        /// <returns></returns>
        public bool CloseCom()
        {
            try
            {
                timer1.Enabled = false;
                timer1.Elapsed -= new System.Timers.ElapsedEventHandler(timer1_Elapsed);

                Thread.Sleep(20);

                port.Close();
                port.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);

                Value = 0;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 串口接收数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            ReceiveCount++;

            if (port.IsOpen)
            {
                int bytesToRead = port.BytesToRead;
                byte[] buffer = new byte[bytesToRead];
                port.Read(buffer, 0, bytesToRead);

                for (int i = 0; i < bytesToRead; i++)
                {
                    if (buffer[i] == 0x02)
                        ReceiveList.Clear();

                    ReceiveList.Add(buffer[i]);

                    if (buffer[i] == 0x0D && ReceiveList.Count == 17)
                    {
                        try
                        {
                            string temp = string.Empty;
                            for (int j = 4; j < 14; j++)
                            {
                                temp += Convert.ToChar(ReceiveList[j].ToString("X").Substring(1, 1));
                            }

                            if (ReceiveList[1] == 0x3D)
                            {//15Kg Kg g 
                                if (ReceiveList[2] == 0x32)
                                    Value = Math.Round(Convert.ToDouble(temp) / -10000d / 10d, 2);
                                else
                                    Value = Math.Round(Convert.ToDouble(temp) / 10000d / 10d, 2);
                            }
                            else if (ReceiveList[1] == 0x3A)
                            {//15Kg 5g 
                                if (ReceiveList[2] == 0x22)
                                    Value = Math.Round(Convert.ToDouble(temp) / -10000d, 2);
                                else
                                    Value = Math.Round(Convert.ToDouble(temp) / 10000d, 2);
                            }
                            else if (ReceiveList[1] == 0x3B)
                            {//15Kg 0.5g 
                                if (ReceiveList[2] == 0x22)
                                    Value = Math.Round(Convert.ToDouble(temp) / -10000d / 10d, 2);
                                else
                                    Value = Math.Round(Convert.ToDouble(temp) / 10000d / 10d, 2);
                            }
                            else
                            {//60Kg Kg ReceiveList[1]== 34
                                if (ReceiveList[2] == 0x32)
                                    Value = Math.Round(Convert.ToDouble(temp) / -10000d / 100d, 2);
                                else
                                    Value = Math.Round(Convert.ToDouble(temp) / 10000d / 100d, 2);
                            }
                        }
                        catch (Exception)
                        {

                        }

                        ReceiveList.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// 间隔事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            #region 判断稳定状态

            if (Value > 0)
            {
                if (Value == LastValue)
                    CurrSteadySecond++;
                else
                    CurrSteadySecond = 0;

                LastValue = Value;
            }
            else
            {
                CurrSteadySecond = 0;
                SteadyState = false;
            }

            if (CurrSteadySecond >= SteadySecond)
                SteadyState = true;
            else
                SteadyState = false;
            #endregion

            #region 判断连接状态
            if (ReceiveCount > 0)
                State = true;
            else
                State = false;

            ReceiveCount = 0;
            #endregion
        }
    }
}
