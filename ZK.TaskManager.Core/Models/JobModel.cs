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
        public string CreateOn { get; set; }
        public string Cron { get; set; }
        public string CronRemark { get; set; }
        public string TaskId { get; set; }
        public string Param { get; set; }
        public string NodeId { get; set; }
        /// <summary>
        /// none   未开始
        /// start  运行中
        /// stop  已结束
        /// pause  暂停中
        /// </summary>
        public string Status { get; set; }
        public TaskModel Task { get; set; }
        public JobModel()
        {
            Task = new TaskModel();
        }
    }
}
