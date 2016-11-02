using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZK.TaskManager.Web.Controllers
{
    public class CommandCenterController : Controller
    {
       
        public ActionResult TaskList()
        {
            return View();
        }

        public ActionResult JobList()
        {
            return View();
        }
    }
}