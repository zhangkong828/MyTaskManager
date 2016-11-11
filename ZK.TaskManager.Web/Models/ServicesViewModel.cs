using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZK.TaskManager.Web.Models
{
    public class ServicesViewModel
    {
        public string Node { get; set; }
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceAddress { get; set; }
        public string ServiceIP { get; set; }
        public string ServiceState { get; set; }
    }
}