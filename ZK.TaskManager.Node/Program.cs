using Consul;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using ZK.TaskManager.Core;
using ZK.TaskManager.Core.Task;
using ZK.TaskManager.Utility;

namespace ZK.TaskManager.Node
{
    class Program
    {
        public delegate bool ControlCtrlDelegate(int CtrlType);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);
        private static ControlCtrlDelegate cancelHandler = new ControlCtrlDelegate(HandlerRoutine);
        public static bool HandlerRoutine(int CtrlType)
        {
            new ConsulClient().Agent.ServiceDeregister(GlobalConfig.NodeID);
            return false;
        }

        static void Main(string[] args)
        {
            if (!SetConsoleCtrlHandler(cancelHandler, true))
            {
                throw new Exception("注册事件失败！");
            }
            //配置信息
            GlobalConfig.InitConfig();
            //服务注册
            RegisterService();
            //调度初始化
            //JobHelper.Init();
            //self host 启动
            var hosturl = GlobalConfig.NodeAddress;
            var config = new HttpSelfHostConfiguration(hosturl);
            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional }
               );
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            var server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();
            Console.WriteLine($"{hosturl} 正在运行...");


            Console.Read();
        }

        static async void RegisterService()
        {
            var client = new ConsulClient();
            var servicesId = GlobalConfig.NodeID;
            //var services = await client.Agent.Services();
            //if (services.Response.ContainsKey(servicesId))
            //    throw new Exception("该服务节点已存在！");
            var registration = new AgentServiceRegistration()
            {
                ID= servicesId,
                Name = "taskservice",
                Tags = new[] { "taskclient", "node" },
                Address = GlobalConfig.NodeIP,
                Port = GlobalConfig.NodePort,
                Check = new AgentServiceCheck
                {
                    HTTP = $"{GlobalConfig.NodeAddress}/api/node/heartbeat",
                    Interval = TimeSpan.FromSeconds(10)
                }
            };
            var result = await client.Agent.ServiceRegister(registration);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("服务注册失败！");
        }

    }
}
