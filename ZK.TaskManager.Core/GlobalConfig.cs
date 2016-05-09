using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
        public static string NodeID;
        /// <summary>
        /// 节点创建时间
        /// </summary>
        public static DateTime CreateOn;
        /// <summary>
        /// 数据库连接
        /// </summary>
        public static string DataBaseConnectString;
        /// <summary>
        /// web平台地址
        /// </summary>
        public static string ManagerWebUrl;
        /// <summary>
        /// 任务插件根目录
        /// </summary>
        public static string TaskPluginDir;
        /// <summary>
        /// 任务插件 压缩目录
        /// </summary>
        public static string TaskPluginDirZip;
        /// <summary>
        /// 任务插件 目录
        /// </summary>
        public static string TaskPluginDirDll;

        public static void InitConfig()
        {
            CreateOn = DateTime.Now;
            //DataBaseConnectString = ConfigurationManager.ConnectionStrings[""].ConnectionString;
            ManagerWebUrl = ConfigurationManager.AppSettings["WebManager"];
            TaskPluginDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["PluginDir"]);
            //TaskPluginDirZip = Path.Combine(TaskPluginDir, "Zip");
            //TaskPluginDirDll = Path.Combine(TaskPluginDir, "Dll");
        }

    }
}
