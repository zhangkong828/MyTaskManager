using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.HostServer.Modules
{
    public class ServerCenterModule: NancyModule
    {
        public ServerCenterModule() : base("servercenter")
        {
            this.RequiresAuthentication();
            Get["/nodelist"] = _ => NodeList(_);
        }

        public dynamic NodeList(dynamic _)
        {
            return View["nodelist"];
        }
    }
}
