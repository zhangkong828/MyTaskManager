using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.Core
{
    /// <summary>
    /// 全局配置
    /// </summary>
    public class GlobalConfig
    {
        /// <summary>
        /// 当前节点标识
        /// </summary>
        public static string NodeID = Guid.NewGuid().ToString("n");
        /// <summary>
        /// 节点创建时间
        /// </summary>
        public static DateTime CreateOn = DateTime.Now;
        /// <summary>
        /// 数据库连接
        /// </summary>
        public static string DataBaseConnectString = ConfigurationManager.ConnectionStrings[""].ConnectionString;
        /// <summary>
        /// 任务调度平台web地址
        /// </summary>
        public static string ManagerWebUrl = ConfigurationManager.AppSettings[""];
        /// <summary>
        /// 任务插件dll根目录
        /// </summary>
        public static string TaskDllDir = "";



    }
}
