using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.HostServer.Modules
{
    public class IndexModule : NancyModule
    {
        public IndexModule() : base("/")
        {
            this.RequiresAuthentication();
            Get["/"] = _ =>
            {
                return View["Index"];
            };

        }
    }
}
