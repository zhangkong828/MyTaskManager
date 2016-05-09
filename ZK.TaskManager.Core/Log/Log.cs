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
        /// 日志输出
        /// </summary>
        public static void JobLog(string jobid, string msg)
        {
            LogService.JobAdd(jobid, msg);
        }
        public static void JobLog(string jobid, string msg, Exception ex)
        {
            LogService.JobAdd(jobid, msg, ex);
        }
    }
}
