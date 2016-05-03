using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.Core
{
    public class Log
    {
        /// <summary>
        /// 系统错误
        /// </summary>
        public static void SysError(string msg, Exception ex)
        {
            Console.WriteLine(string.Format("{0}:{1} > {2}，Exception.Message:{3}，Exception.StackTrace:{4}", DateTime.Now.ToString(), GlobalConfig.NodeID, msg, ex.Message, ex.StackTrace));
        }

        /// <summary>
        /// 日志输出
        /// </summary>
        public static void TaskLog(string taskid,string msg)
        {
            Console.WriteLine(string.Format("{0}:{1} > {2}", DateTime.Now.ToString(), GlobalConfig.NodeID, msg));
        }

    }
}
