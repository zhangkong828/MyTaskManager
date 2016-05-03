using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.Core
{
    /// <summary>
    /// 任务检测
    /// 用于获取最新任务
    /// </summary>
    public class TaskMonitor : BaseMonitor
    {
        public TaskMonitor() : base("任务检测", 1000 * 60 * 1)
        {
        }

        protected override void Run()
        {
            //获取任务

            //任务dll下载

            //加入
        }
    }
}
