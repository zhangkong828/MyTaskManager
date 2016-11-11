using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ZK.TaskManager.Node.API
{
   public class NodeController: ApiController
    {
        [HttpGet]
        public bool HeartBeat()
        {
            return true;
        }
    }
}
