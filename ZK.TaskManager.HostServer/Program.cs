using Nancy;
using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.HostServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //启动 后台管理站点
            var webmanager = ConfigurationManager.AppSettings["WebManager"];
            var Host = new NancyHost(new Uri(webmanager));
            
            Host.Start();
            Console.WriteLine("管理系统已启动..." + webmanager);
            SocketServer.Inti();

            Console.Read();
        }
    }
}
