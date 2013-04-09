using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSDN.Client.SDK
{
    internal class SocketHelper
    {
        private Socket client = null;
        private IPAddress _ip = null;
        private int _port = 80;
        private Uri _uri = null;
        private int timeOut = 1000;
        private int sendTimeout = 2000;
        private int receiveTimeout = 3000;
        private List<KeyValuePair<string, string>> header = new List<KeyValuePair<string, string>>();

        private bool isConnected = false;
        public bool isEnd = false;

        private EventWaitHandle allDone = new EventWaitHandle(false, EventResetMode.ManualReset);

        public SocketHelper(IPAddress ip, int port)
        {
            this._ip = ip;
            this._port = port;
        }
        public SocketHelper(Uri uri)
        {
            this._uri = uri;
            var ipEntry = Dns.GetHostEntry(uri.Host);

            this._ip = ipEntry.AddressList[0];
            this._port = uri.Port;
        }
        /// <summary>
        /// 设置通信超时限制（毫秒）
        /// </summary>
        public int Timeout
        {
            set { timeOut = value; }
        }
        public int SendTimeout
        {
            set { sendTimeout = value; }
        }
        public int ReceiveTimeout
        {
            set { receiveTimeout = value; }
        }
        public void AddHeader(string name, string value)
        {
            header.Add(new KeyValuePair<string, string>(name, value));
        }
        /// <summary>
        /// 获取连接后的socket对象（也可能连接失败）
        /// </summary>
        public Socket SocketClient
        {
            get
            {
                if (!isConnected) Connect();
                return client;
            }
        }

        /// <summary>
        /// 对uri发起一个get请求
        /// </summary>
        /// <param name="receive">是否接收返回的数据</param>
        /// <returns></returns>
        public string Get(bool receive)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kv in header)
            {
                sb.AppendFormat("{0}: {1}\r\n", kv.Key, kv.Value);
            }
            string socketStr = string.Format("GET {0} HTTP/1.1\r\nHost: {1}\r\n{2}Content-Length: 0\r\n\r\n"
                  , _uri.PathAndQuery, _uri.Host, sb.ToString());

            return Send(socketStr, receive);
        }
        /// <summary>
        /// 对uri发起一个post请求
        /// </summary>
        /// <param name="data">发送的urlencode后数据</param>
        /// <param name="receive">是否接收返回的数据</param>
        /// <returns></returns>
        public string Post(string data, bool receive)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kv in header)
            {
                sb.AppendFormat("{0}: {1}\r\n", kv.Key, kv.Value);
            }
            string socketStr = string.Format(@"POST {0} HTTP/1.1
Host: {1}
{2}Content-Type: application/x-www-form-urlencoded
Content-Length: {3}

{4}"
                , _uri.AbsolutePath, _uri.Host, sb.ToString()
                , Encoding.UTF8.GetBytes(data).Length, data);

            return Send(socketStr, receive);
        }
        /// <summary>
        /// 向目标机器发起一个请求
        /// </summary>
        /// <param name="socketStr">要发送的socket字符常常</param>
        /// <param name="receive">是否接收返回的数据</param>
        /// <returns></returns>
        public string Send(string socketStr, bool receive)
        {
            if (!isConnected) Connect();
            if (!client.Connected) return null;

            string result = null;
            byte[] socketData = Encoding.UTF8.GetBytes(socketStr);
            try
            {
                client.Send(socketData);

                if (receive)
                {
                    byte[] receive_data = new byte[4096];
                    client.Receive(receive_data, SocketFlags.None);

                    result = Encoding.UTF8.GetString(receive_data);
                    result = result.Substring(result.IndexOf("\r\n\r\n")).TrimStart().TrimEnd('\0');
                }
            }
            finally
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            return result;
        }
        private void Connect()
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.SendTimeout = sendTimeout;
            client.ReceiveTimeout = receiveTimeout;

            allDone.Reset();

            client.BeginConnect(_ip, _port, RequestCallback, client);
            if (!allDone.WaitOne(timeOut, false))
            {
                client.Close();
            }
        }
        private void RequestCallback(IAsyncResult ar)
        {
            allDone.Set();

            client = (Socket)ar.AsyncState;

            isConnected = true;
            isEnd = true;
        }
    }
}
