using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.HostServer.Modules
{
    public class TaskCenterModule : NancyModule
    {
        public TaskCenterModule() : base("taskcenter")
        {
            this.RequiresAuthentication();
            Get["/tasklist"] = _ => TaskList(_);
            Get["/joblist"] = _ => JobList(_);
        }

        public dynamic TaskList(dynamic _)
        {
            return View["tasklist"];
        }
        public dynamic JobList(dynamic _)
        {
            return View["joblist"];
        }
    }
}
