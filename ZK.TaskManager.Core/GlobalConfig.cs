using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZK.TaskManager.Utility;

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
        /// 当前节点端口
        /// </summary>
        public static string Port;
        /// <summary>
        /// 节点创建时间
        /// </summary>
        public static DateTime CreateOn;
        /// <summary>
        /// 数据库连接
        /// </summary>
        public static string DataBaseConnectString;
        /// <summary>
        /// 插件根目录
        /// </summary>
        public static string TaskPluginDir;
        /// <summary>
        /// 插件 压缩目录
        /// </summary>
        public static string TaskPluginDirZip;
        /// <summary>
        /// 插件 解压缩目录
        /// </summary>
        public static string TaskPluginDirSrc;

        public static void InitConfig()
        {
            Port = ConfigurationManager.AppSettings["Port"];
            NodeID = CommonHelper.GetIP() + ":" + Port;
            CreateOn = DateTime.Now;
            //DataBaseConnectString = ConfigurationManager.ConnectionStrings[""].ConnectionString;
            TaskPluginDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["PluginDir"]);
            TaskPluginDirZip = Path.Combine(TaskPluginDir, "Zip");
            TaskPluginDirSrc = Path.Combine(TaskPluginDir, "Src");
        }

    }
}
