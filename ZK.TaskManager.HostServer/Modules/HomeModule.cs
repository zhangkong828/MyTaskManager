using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.HostServer.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule() : base("home")
        {
            this.RequiresAuthentication();
            Get["/"] = _ => Index(_);
            Get["/index"] = _ => Index(_);
        }

        public dynamic Index(dynamic _)
        {
            return View["index"];
        }

    }
}
