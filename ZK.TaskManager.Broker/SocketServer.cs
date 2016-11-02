using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ZK.TaskManager.Utility;

namespace ZK.TaskManager.Broker
{
    public class SocketServer
    {
        public static Dictionary<string, Socket> dic;
        public static string address;
        public static string port;

        public static void Inti()
        {
            dic = new Dictionary<string, Socket>();
            address = ConfigurationManager.AppSettings["Address"];
            port = ConfigurationManager.AppSettings["Port"];

            var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定本机ip
            s.Bind(new IPEndPoint(IPAddress.Parse(address), Convert.ToInt32(port)));
            //监听
            s.Listen(10);

            Console.WriteLine("服务已启动!");

            Task t1 = new Task(() =>
            {
                while (true)
                {
                    //接收客户端连接
                    var accept = s.Accept();
                    var current = accept.RemoteEndPoint.ToString();
                    Console.WriteLine("节点" + current + "连接成功");
                    dic.Add(current, accept);
                    Task t2 = new Task(() =>
                    {
                        while (true)
                        {
                            //接收缓冲区
                            byte[] buffer = new byte[1024 * 1024 * 1];
                            //n为实际接收字节数
                            int n = accept.Receive(buffer);
                            if (n == 0)
                            {
                                Console.WriteLine("节点" + current + "失去连接");
                                dic.Remove(current);
                                break;
                            }
                            var msg = System.Text.Encoding.UTF8.GetString(buffer, 0, n);
                            Console.WriteLine(current + ">>>" + msg);
                        }
                    });
                    t2.Start();
                }
            });
            t1.Start();
        }

        public static void Send(string msg)
        {
            foreach (var item in dic)
            {
                var b = FormatHelper.StringToBytes(msg);
                item.Value.Send(b);
            }
        }


    }
}
