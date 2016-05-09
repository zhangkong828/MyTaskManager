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
        public string Id { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数说明
        /// </summary>
        public string TaskParamRemark { get; set; }
        
        /// <summary>
        /// 任务所在DLL目录名
        /// </summary>
        public string TaskDirName { get; set; }
        /// <summary>
        /// 任务所在DLL下对应的程序集名称
        /// xxx.dll
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// 任务所在命名空间.类
        /// </summary>
        public string NameSpaceAndClass { get; set; }

        /// <summary>
        /// 任务创建时间
        /// </summary>
        public string CreateOn { get; set; }

        /// <summary>
        /// 任务修改时间
        /// </summary>
        public string ModifyOn { get; set; }

        /// <summary>
        /// 任务备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 任务dll版本
        /// </summary>
        public string Verson { get; set; }
    }

}
