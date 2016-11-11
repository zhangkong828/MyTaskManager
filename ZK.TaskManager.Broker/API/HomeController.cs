using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ZK.TaskManager.Utility;

namespace ZK.TaskManager.Broker.API
{

    public class HomeController : ApiController
    {
       
        public bool Ping(string node,string parame)
        {
            if (string.IsNullOrEmpty(node)||string.IsNullOrEmpty(parame))
            {
                return false;
            }
            return parame == "1";
        }

     
    }
}
