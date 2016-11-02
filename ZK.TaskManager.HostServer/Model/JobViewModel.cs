using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.HostServer.Model
{
    public class JobViewModel
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

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 参数说明
        /// </summary>
        public string TaskTaskParamRemark { get; set; }

        /// <summary>
        /// 任务所在DLL目录名
        /// </summary>
        public string TaskTaskDirName { get; set; }
        /// <summary>
        /// 任务所在DLL下对应的程序集名称
        /// xxx.dll
        /// </summary>
        public string TaskAssembly { get; set; }

        /// <summary>
        /// 任务所在命名空间.类
        /// </summary>
        public string TaskNameSpaceAndClass { get; set; }

        /// <summary>
        /// 任务创建时间
        /// </summary>
        public string TaskCreateOn { get; set; }

        /// <summary>
        /// 任务修改时间
        /// </summary>
        public string TaskModifyOn { get; set; }

        /// <summary>
        /// 任务备注
        /// </summary>
        public string TaskRemark { get; set; }
        /// <summary>
        /// 任务dll版本
        /// </summary>
        public string TaskVerson { get; set; }
    }
}
