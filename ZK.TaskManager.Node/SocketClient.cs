using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZK.TaskManager.Core;
using ZK.TaskManager.Utility;

namespace ZK.TaskManager.Node
{
    public class SocketClient
    {
        public static Socket client;
        public static string address;
        public static string port;
        public static bool iskeep;
        public static void Init()
        {
            address = ConfigurationManager.AppSettings["Address"];
            port = ConfigurationManager.AppSettings["Port"];
            iskeep = false;

            Connect();
            Receive();
            //相当于心跳 自动重连
            Task t = new Task(() =>
            {
                while (true)
                {
                    if (!iskeep)
                    {
                        Connect();
                        Receive();
                    }
                    Thread.Sleep(1000);
                }
            });
            t.Start();
        }

        private static void Connect()
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        SocketConnect:
            try
            {
                //连接
                client.Connect(new IPEndPoint(IPAddress.Parse(address), Convert.ToInt32(port)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("连接异常，尝试重新连接HostServer...");
                Thread.Sleep(2000);
                goto SocketConnect;
            }
            iskeep = true;
            Console.WriteLine("已成功连接到服务端！");
            GlobalConfig.NodeID = client.LocalEndPoint.ToString();
        }

        private static void Receive()
        {
            Task t = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        //设置缓冲区
                        byte[] buffer = new byte[1024 * 1024 * 1];
                        int num = client.Receive(buffer);
                        //把字节转换为字符串
                        string msg = System.Text.Encoding.UTF8.GetString(buffer, 0, num);
                        ProcessHandle.Execute(msg);

                    }
                    catch (Exception ex)
                    {
                        iskeep = false;
                        Console.WriteLine(ex.Message);
                        break;
                    }
                }
            });
            t.Start();
        }


        public static void Send(string msg)
        {
            var b = FormatHelper.StringToBytes(msg);
        SocketSend:
            try
            {
                client.Send(b);
            }
            catch (Exception)
            {
                Connect();
                Receive();
                goto SocketSend;
            }

        }

    }
}
