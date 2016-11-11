using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZK.TaskManager.Core.Services;
using ZK.TaskManager.Utility;

namespace ZK.TaskManager.Web.Controllers
{
    public class LogCenterController : BaseController
    {

        public ActionResult SystemLog()
        {
            return View();
        }
        
        public ActionResult GetSystemLog()
        {
            var order = RequestHelper.GetValue("order");
            order = string.IsNullOrEmpty(order) ? "desc" : order;
            var limit = RequestHelper.GetInt("limit");
            var offset = RequestHelper.GetInt("offset");
            var pageindex = offset + 1;
            var count = 0;
            var list = LogService.SysQuery(pageindex, limit, order, out count);
            var result = new
            {
                rows = list,
                total = count
            };
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public ActionResult NodeLog()
        {
            return View();
        }

        public ActionResult GetNodeLog()
        {
            var id = RequestHelper.GetValue("");
            var order = RequestHelper.GetValue("order");
            order = string.IsNullOrEmpty(order) ? "desc" : order;
            var limit = RequestHelper.GetInt("limit");
            var offset = RequestHelper.GetInt("offset");
            var pageindex = offset + 1;
            var count = 0;
            var list = LogService.NodeQuery(id,pageindex, limit, order, out count);
            var result = new
            {
                rows = list,
                total = count
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}