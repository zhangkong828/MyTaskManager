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
        /// start  启动
        /// delete  删除
        /// pause  暂停
        /// resume  恢复
        /// </summary>
        public string Status { get; set; }
        public TaskModel Task { get; set; }
        public JobModel()
        {
            Task = new TaskModel();
        }
    }
}
