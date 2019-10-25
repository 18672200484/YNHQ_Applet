using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RW.Hawkvor
{
    public class HawkvorRwer
    {
        System.Timers.Timer timer1 = new System.Timers.Timer();


        private Socket listener = null;
        /// <summary>
        /// 当前连接
        /// </summary>
        public Socket Listener
        {
            get { return listener; }
        }

        private Action<string> output = null;
        /// <summary>
        /// 当前输出方法
        /// </summary>
        public Action<string> OutPut
        {
            get { return output; }
        }

        private string rfId = string.Empty;
        /// <summary>
        /// 当前读到的卡号
        /// </summary>
        public string RfId
        {
            get { return rfId; }
        }

        private bool status = false;
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool Status
        {
            get { return status; }
        }
        /// <summary>
        /// Socket输出对象
        /// </summary>
        Socketoutput socketoutput = new Socketoutput();

        /// <summary>
        /// State对象
        /// </summary>
        StateObject state = new StateObject();
        /// <summary>
        /// 设置连接状态
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(bool status)
        {
            if (this.OnStatusChange != null) this.OnStatusChange(status);
            this.status = status;
        }

        public delegate void StatusChangeHandler(bool status);
        public event StatusChangeHandler OnStatusChange;

        public delegate void ReadSucessHandler(string rfid);
        public event ReadSucessHandler OnReadSucess;


        public HawkvorRwer()
        {

        }
        private ManualResetEvent allDone = new ManualResetEvent(false);
        private class StateObject
        {
            public Socket workSocket = null;
            public const int BufferSize = 1024;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }
        private class Socketoutput
        {
            public StateObject stateobject;
            public Socket socket;
            public Action<string> Output;
        }

        /// <summary>
        /// 创建监听
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="com"></param>
        /// <returns></returns>
        public Socket CreateListening(string ip, int com)
        {

            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, com);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Connect(localEndPoint);
                SetStatus(true);
                return listener;
            }
            catch (Exception)
            {
                SetStatus(false);
                return null;
            }
        }
        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="output"></param>
        public void StartListening(Socket listener, Action<string> output)
        {
            #region 定时器
            //timer1.Interval = 1000;
            //timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
            //timer1.Enabled = true;

            //this.listener = listener;
            //this.output = output;

            //socketoutput.socket = this.Listener;
            //socketoutput.Output = this.OutPut;
            //state.workSocket = listener;
            //socketoutput.stateobject = state; 
            #endregion

            #region 线程
            try
            {
                while (true)
                {
                    allDone.Reset();
                    this.listener = listener;
                    this.output = output;

                    socketoutput.socket = this.Listener;
                    socketoutput.Output = this.OutPut;
                    state.workSocket = listener;
                    socketoutput.stateobject = state;
                    listener.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), socketoutput);
                    allDone.WaitOne();
                }
            }
            catch (Exception ex)
            {
                output("监听线程错误" + ex.Message);
            }
            #endregion
        }

        void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            listener.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), socketoutput);
        }


        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                allDone.Set();
                //timer1.Enabled = false;
                String content = String.Empty;
                //socketoutput = (Socketoutput)ar.AsyncState;
                //state = (StateObject)socketoutput.stateobject;

                Socket handler = state.workSocket;
                int bytesRead = handler.EndReceive(ar);
                if (bytesRead > 0)
                {
                    for (int i = 0; i < bytesRead; i++)
                    {
                        content += string.Format("{0:x2}", state.buffer[i]);
                    }
                    PrintRecvMssg(content, socketoutput.Output);
                    SetStatus(true);
                }
            }
            catch (Exception ex)
            {
                SetStatus(false);
                socketoutput.Output(string.Format("ReadCallback,原因:{0}", ex.ToString()));
            }
            //finally
            //{
            //    timer1.Enabled = true;
            //}
        }

        private void PrintRecvMssg(string str, Action<string> output)
        {
            try
            {
                if (str.Length >= 30)
                {
                    this.rfId = str.Trim().Replace(" ", "").Substring(6, 20).ToUpper();
                    if (!string.IsNullOrEmpty(this.rfId) && this.OnReadSucess != null) OnReadSucess(this.rfId);
                }
            }
            catch (Exception ex)
            {
                SetStatus(false);
                output(string.Format("标签号解析失败,原因:{0}", ex.ToString()));
            }
        }

        public bool Close()
        {
            allDone.Dispose();
            return true;
        }
    }
}
