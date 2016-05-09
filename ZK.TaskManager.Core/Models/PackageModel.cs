using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.Core.Models
{
    public class PackageModel
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 插件版本
        /// </summary>
        public string Verson { get; set; }
        /// <summary>
        /// 插件作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 插件在Plugin下的目录名
        /// </summary>
        public string DirName { get; set; }
        /// <summary>
        /// 插件的程序集名称
        /// </summary>
        public string Assembly { get; set; }
        /// <summary>
        /// 插件的命名空间，类名
        /// </summary>
        public string NameSpace { get; set; }
    }
}
