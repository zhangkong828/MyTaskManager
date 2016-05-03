using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.Core.Models
{
    public class JobModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CteateOn { get; set; }
        public string Cron { get; set; }
        public string CronRemark { get; set; }
        public string TaskId { get; set; }
        public string Param { get; set; }
        public string NodeId { get; set; }
        public string Status { get; set; }
    }
}
