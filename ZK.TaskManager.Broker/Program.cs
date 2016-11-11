using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using ZK.TaskManager.Core;

namespace ZK.TaskManager.Broker
{
    class Program
    {
        static void Main(string[] args)
        {
            var port= ConfigurationManager.AppSettings["Port"];
            var hosturl = "http://localhost:" + port;
            var config = new HttpSelfHostConfiguration(hosturl);

            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional }
               );
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            var server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();

            Console.WriteLine("Broker 已启动...");

            Console.Read();
        }
    }
}
