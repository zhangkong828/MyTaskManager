using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.HostServer.Modules
{
    public class LogCenterModule : NancyModule
    {
        public LogCenterModule() : base("logcenter")
        {
            this.RequiresAuthentication();
            Get["/systemlog"] = _ => SystemLog(_);
            Get["/tasklog"] = _ => TaskLog(_);
        }

        public dynamic SystemLog(dynamic _)
        {
            return View["systemlog"];
        }
        public dynamic TaskLog(dynamic _)
        {
            return View["tasklog"];
        }
    }
}
