using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZK.TaskManager.Core.Services;
using ZK.TaskManager.Utility;

namespace ZK.TaskManager.HostServer.Modules
{
    public class ApiModule : NancyModule
    {
        public ApiModule() : base("api")
        {
            this.RequiresAuthentication();

            //节点列表
            Get["/nodelist"] = _ =>
            {
                var dic = SocketServer.dic.Keys.ToArray();
                var result = new List<object>();
                for (int i = 0; i < dic.Length; i++)
                {
                    result.Add(new { index = i + 1, node = dic[i] });
                }
                return Response.AsJson(result);
            };

            //任务列表
            Get["/tasklist"] = _ =>
            {
                var list = TaskService.List();
                return Response.AsJson(list);
            };

            //调度列表
            Get["/joblist"] = _ =>
            {
                var list = TaskService.List();
                return Response.AsJson(list);
            };

            //系统日志列表
            //分页
            Get["/syslog"] = _ =>
            {
                var form = Request.Query;
                var order = form.order.HasValue ? form.order : "desc";
                var limit = form.limit;
                var offset = form.offset;
                var pageindex = (int)offset + 1;
                var count = 0;
                var list = LogService.SysQuery(pageindex, limit, order, out count);
                var result = new
                {
                    rows = list,
                    total = count
                };
                return Response.AsJson(result);
            };

            //任务日志列表
            //分页
            Get["/joblog"] = _ =>
            {
                var form = Request.Query;
                var order = form.order.HasValue ? form.order : "desc";
                var limit = form.limit;
                var offset = form.offset;
                var pageindex = (int)offset + 1;
                var jobid = form.jobid;
                var count = 0;
                var list = LogService.JobQuery(jobid, pageindex, limit, order, out count);
                var result = new
                {
                    rows = list,
                    total = count
                };
                return Response.AsJson(result);
            };
        }
    }
}
