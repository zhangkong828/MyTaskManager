using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZK.TaskManager.Core.Services;

namespace ZK.TaskManager.Core
{
    public class Log
    {
        /// <summary>
        /// 系统日志
        /// </summary>
        public static void SysLog(string msg)
        {
            LogService.SysAdd(msg);
        }
        public static void SysLog(string msg, Exception ex)
        {
            LogService.SysAdd(msg, ex);
        }


        /// <summary>
        /// 节点输出
        /// </summary>
        public static void NodeLog(string nodeid, string msg)
        {
            LogService.NodeAdd(nodeid, msg);
        }
        public static void NodeLog(string nodeid, string msg, Exception ex)
        {
            LogService.NodeAdd(nodeid, msg, ex);
        }
    }
}   
