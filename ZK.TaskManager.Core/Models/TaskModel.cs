using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.Core.Models
{
    /// <summary>
    /// 任务实体
    /// </summary>
    public class TaskModel
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskID { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 任务执行参数
        /// 格式 json
        /// </summary>
        public string TaskParam { get; set; }

        /// <summary>
        /// 参数说明
        /// </summary>
        public string TaskParamRemark { get; set; }

        /// <summary>
        /// 运行频率参数
        /// </summary>
        public string CronExpressionString { get; set; }

        /// <summary>
        /// 任务频率说明
        /// </summary>
        public string CronRemark { get; set; }

        /// <summary>
        /// 任务所在DLL对应的程序集名称
        /// xxx.dll
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// 任务所在命名空间.类
        /// </summary>
        public string NameSpaceAndClass { get; set; }

        public TaskStatus Status { get; set; }

        /// <summary>
        /// 任务状态中文说明
        /// </summary>
        public string StatusCn
        {
            get
            {
                var cn = "";
                switch (Status)
                {
                    case TaskStatus.RUN:
                        cn = "运行";
                        break;
                    case TaskStatus.STOP:
                        cn = "停止";
                        break;
                    case TaskStatus.Exception:
                        cn = "异常";
                        break;
                }
                return cn;
            }
        }

        /// <summary>
        /// 任务创建时间
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// 任务修改时间
        /// </summary>
        public DateTime? ModifyOn { get; set; }

        /// <summary>
        /// 任务最近运行时间
        /// </summary>
        public DateTime? RecentRunTime { get; set; }

        /// <summary>
        /// 任务下次运行时间
        /// </summary>
        public DateTime? LastRunTime { get; set; }

        /// <summary>
        /// 任务备注
        /// </summary>
        public string Remark { get; set; }

    }

    /// <summary>
    /// 任务状态枚举
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 运行状态
        /// </summary>
        RUN = 0,

        /// <summary>
        /// 停止状态
        /// </summary>
        STOP = 1,

        /// <summary>
        /// 异常
        /// </summary>
        Exception = 2
    }
}
