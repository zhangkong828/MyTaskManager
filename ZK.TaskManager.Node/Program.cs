using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.SelfHost;
using ZK.TaskManager.Core;
using ZK.TaskManager.Core.Task;

namespace ZK.TaskManager.Node
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalConfig.InitConfig();
            JobHelper.Init();


            var hosturl = "http://localhost:" + GlobalConfig.Port;
            var config = new HttpSelfHostConfiguration(hosturl);

            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional }
               );
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            var server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();


            Console.WriteLine(hosturl + "已启动...");
            Console.WriteLine("Task Node正在运行...");
        }
    }
}
