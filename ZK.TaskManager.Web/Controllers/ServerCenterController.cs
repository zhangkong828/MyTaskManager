using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZK.TaskManager.Web.Core;

namespace ZK.TaskManager.Web.Controllers
{
    public class ServerCenterController : BaseController
    {

        public ActionResult NodeList()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetNodeList()
        {
            var nodes = NodeHost.Host.GetNodeList();
            return Json(nodes, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult UnRegisterService(string sid)
        {
            if (string.IsNullOrEmpty(sid))
                return Json(false, JsonRequestBehavior.DenyGet);
            return Json(NodeHost.Host.UnRegisterService(sid), JsonRequestBehavior.DenyGet);
        }
    }
}