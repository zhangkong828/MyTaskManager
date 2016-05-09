using Nancy;
using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZK.TaskManager.Core.Models;
using ZK.TaskManager.Utility;

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


            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                var msg = new MessageModel();
                msg.Id = "000001";
                msg.JobId = "qwerasdfzxcv";
                var json = FormatHelper.JsonSerializer(msg);
                SocketServer.Send(json);
            });

            Console.Read();
        }
    }
}
