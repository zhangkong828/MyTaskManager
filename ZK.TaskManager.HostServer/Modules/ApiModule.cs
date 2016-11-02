using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZK.TaskManager.Core.Models;
using ZK.TaskManager.Core.Services;
using ZK.TaskManager.HostServer.Model;
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

            Post["/task/add"] = _ =>
            {
                TaskModel model = new TaskModel();
                var form = Request.Form;
                model.Id = Guid.NewGuid().ToString("n");
                model.Name = form.name.HasValue ? form.name : "";
                model.TaskParamRemark = form.taskParamRemark.HasValue ? form.taskParamRemark : "";
                model.TaskDirName = form.taskDirName.HasValue ? form.taskDirName : "";
                model.Assembly = form.assembly.HasValue ? form.assembly : "";
                model.NameSpaceAndClass = form.nameSpaceAndClass.HasValue ? form.nameSpaceAndClass : "";
                model.CreateOn = DateTime.Now.ToString();
                model.ModifyOn = DateTime.Now.ToString();
                model.Remark = form.remark.HasValue ? form.remark : "";
                model.Verson = form.verson.HasValue ? form.verson : "";
                var r = TaskService.Add(model);
                return Response.AsJson(r);
            };

            Post["/task/update"] = _ =>
            {
                TaskModel model = new TaskModel();
                var form = Request.Form;
                model.Id = form.id;
                model.Name = form.name.HasValue ? form.name : "";
                model.TaskParamRemark = form.taskParamRemark.HasValue ? form.taskParamRemark : "";
                model.TaskDirName = form.taskDirName.HasValue ? form.taskDirName : "";
                model.Assembly = form.assembly.HasValue ? form.assembly : "";
                model.NameSpaceAndClass = form.nameSpaceAndClass.HasValue ? form.nameSpaceAndClass : "";
                model.CreateOn = form.createOn;
                model.ModifyOn = DateTime.Now.ToString();
                model.Remark = form.remark.HasValue ? form.remark : "";
                model.Verson = form.verson.HasValue ? form.verson : "";
                var r = TaskService.Update(model);
                return Response.AsJson(r);
            };
            //调度列表
            Get["/joblist"] = _ =>
            {
                var form = Request.Query;
                var order = form.order.HasValue ? form.order : "desc";
                var limit = form.limit;
                var offset = form.offset;
                var pageindex = (int)offset + 1;
                var count = 0;
                var joblist = JobService.List(pageindex, limit, order, out count);
                var list = AutoMapper.Mapper.Map<List<JobModel>, List<JobViewModel>>(joblist);
                var result = new
                {
                    rows = list,
                    total = count
                };
                return Response.AsJson(result);
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
                var list = LogService.NodeQuery(jobid, pageindex, limit, order, out count);
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
