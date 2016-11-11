using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZK.TaskManager.Web.Controllers
{
    public class LogCenterController : BaseController
    {
        
        public ActionResult SystemLog()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetSystemLog()
        {
            return View();
        }

        public ActionResult NodeLog()
        {
            return View();
        }
    }
}