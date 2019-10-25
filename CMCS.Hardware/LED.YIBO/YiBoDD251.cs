using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LED.YIBO
{
    public class YiBoDD251
    {
        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int com { get; set; }

        /// <summary>
        /// 当前监听
        /// </summary>
        public Socket client = null;

        private bool status = false;
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool Status
        {
            get { return status; }
        }

        private string returnmess = string.Empty;
        /// <summary>
        /// 返回信息
        /// </summary>
        public string ReturnMess
        {
            get { return returnmess; }
        }

        /// <summary>
        /// 设置连接状态
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(bool status)
        {
            if ( this.OnStatusChange != null) this.OnStatusChange(status);
            this.status = status;
        }

        private IPEndPoint localEndPoint = null;

        public delegate void StatusChangeHandler(bool status);
        public event StatusChangeHandler OnStatusChange;

        /// <summary>
        /// 创建监听
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="com"></param>
        /// <returns></returns>
        public bool CreateListening(string ip, int com)
        {
            try
            {
                this.Ip = ip;
                this.com = com;
                IPAddress ipAddress = IPAddress.Parse(ip);
                localEndPoint = new IPEndPoint(ipAddress, com);
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(localEndPoint);
                SetStatus(true);
            }
            catch (Exception)
            {
                SetStatus(false);
                return false;
            }
            return true;
        }

        private static byte[] strToHexByte(string hexStr)
        {
            hexStr = hexStr.Replace(" ", "");
            if ((hexStr.Length % 2) != 0)
                hexStr += " ";
            byte[] returnbytes = new byte[hexStr.Length / 2];
            for (int i = 0; i < returnbytes.Length; i++)
            {
                returnbytes[i] = Convert.ToByte(hexStr.Substring(i * 2, 2), 16);
            }
            return returnbytes;
        }

        private static string toChar(string str)
        {
            string result = string.Empty;
            char[] values = str.ToCharArray();
            foreach (char item in values)
            {
                int value = Convert.ToInt32(item);
                result += string.Format("{0:X}", value) + " ";
            }
            return BitConverter.ToString(ASCIIEncoding.Default.GetBytes(result)).Replace("-", "");
            //return result;
        }
        /// <summary>
        /// 发送车号与重量
        /// </summary>
        /// <param name="CarNumber">车号</param>
        /// <param name="Weight">重量</param>
        public void Send(string CarNumber, string Weight)
        {
            string carnumbertype = "$0&X0000&Y0000&Z00$1$R";//ASC码格式 清除屏幕 坐标 行间距 汉字格式 颜色
            string weighttype = "&N16";//换行

            string ss = string.Empty;

            byte[] byteclear = Encoding.ASCII.GetBytes(carnumbertype);//符号转byte
            byte[] byteclear2 = Encoding.ASCII.GetBytes(weighttype);//符号转byte
            byte[] byteContent = Encoding.Default.GetBytes(CarNumber);//内容转byte
            byte[] byteContent2 = Encoding.Default.GetBytes(Weight);//内容转byte
            for (int i = 0; i < byteclear.Length; i++)
            {
                ss += string.Format("{0:X}", byteclear[i]) + " ";
            }
            for (int i = 0; i < byteContent.Length; i++)
            {
                ss += string.Format("{0:X}", byteContent[i]) + " ";
            }
            for (int i = 0; i < byteclear2.Length; i++)
            {
                ss += string.Format("{0:X}", byteclear2[i]) + " ";
            }
            for (int i = 0; i < byteContent2.Length; i++)
            {
                ss += string.Format("{0:X}", byteContent2[i]) + " ";
            }
            //命令前缀+内容
            string content = string.Format("f5 c2 ff 01 0b 08 ff ff {0}1a", ss);
            client.Send(strToHexByte(content));
            //关闭接收数据
            //Socketoutput socketoutput = new Socketoutput();
            //socketoutput.socket = client;
            //socketoutput.Output = null;
            //StateObject state = new StateObject();
            //state.workSocket = client;
            //socketoutput.stateobject = state;
            //client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), socketoutput);
        }

        /// <summary>
        /// 发送文字
        /// </summary>
        /// <param name="CarNumber">第一行文字</param>
        /// <param name="Weight">第二行文字</param>
        public void Send2(string value1, string value2)
        {
            string carnumbertype = "$0&X0000&Y0000&Z00$1$R";//ASC码格式 清除屏幕 坐标 行间距 汉字格式 颜色
            string weighttype = "&N16";//换行

            string ss = string.Empty;

            byte[] byteclear = Encoding.ASCII.GetBytes(carnumbertype);//符号转byte
            byte[] byteclear2 = Encoding.ASCII.GetBytes(weighttype);//符号转byte
            byte[] byteContent = Encoding.Default.GetBytes(value1);//内容转byte
            byte[] byteContent2 = Encoding.Default.GetBytes(value2);//内容转byte
            for (int i = 0; i < byteclear.Length; i++)
            {
                ss += string.Format("{0:X}", byteclear[i]) + " ";
            }
            for (int i = 0; i < byteContent.Length; i++)
            {
                ss += string.Format("{0:X}", byteContent[i]) + " ";
            }
            for (int i = 0; i < byteclear2.Length; i++)
            {
                ss += string.Format("{0:X}", byteclear2[i]) + " ";
            }
            for (int i = 0; i < byteContent2.Length; i++)
            {
                ss += string.Format("{0:X}", byteContent2[i]) + " ";
            }
            //命令前缀+内容
            string content = string.Format("f5 c2 ff 01 0b 08 ff ff {0}1a", ss);
            client.Send(strToHexByte(content));
            //关闭接收数据
            //Socketoutput socketoutput = new Socketoutput();
            //socketoutput.socket = client;
            //socketoutput.Output = null;
            //StateObject state = new StateObject();
            //state.workSocket = client;
            //socketoutput.stateobject = state;
            //client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), socketoutput);
        }
        /// <summary>
        /// 关闭监听
        /// </summary>
        public void Close()
        {
            if (this.client != null)
                this.client.Close();
        }
        /// <summary>
        /// 接受返回数据
        /// </summary>
        /// <param name="ar"></param>
        private void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;
            Socketoutput socketoutput = (Socketoutput)ar.AsyncState;
            StateObject state = (StateObject)socketoutput.stateobject;
            try
            {
                Socket handler = state.workSocket;
                int bytesRead = handler.EndReceive(ar);
                if (bytesRead > 0)
                {
                    for (int i = 0; i < bytesRead; i++)
                    {
                        content += string.Format("{0:x2}", state.buffer[i]);
                    }
                }
                this.returnmess = content;
            }
            catch (Exception ex)
            {
                socketoutput.Output(string.Format("ReadCallback,原因:{0}", ex.ToString()));
            }
        }

        private class Socketoutput
        {
            public StateObject stateobject;
            public Socket socket;
            public Action<string> Output;
        }
        private class StateObject
        {
            public Socket workSocket = null;
            public const int BufferSize = 1024;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }
    }
}
