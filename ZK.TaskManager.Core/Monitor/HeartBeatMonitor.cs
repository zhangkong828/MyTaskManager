using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.Core
{
    /// <summary>
    /// 节点的心跳检测
    /// 用于心跳通知当前节点状态
    /// </summary>
    public class HeartBeatMonitor : BaseMonitor
    {
        public HeartBeatMonitor(): base("节点心跳检测", 1000 * 60 * 1)
        {
        }

        protected override void Run()
        {
            //发送心跳检测
        }
    }
}
