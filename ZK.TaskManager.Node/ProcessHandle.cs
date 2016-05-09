using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZK.TaskManager.Core;
using ZK.TaskManager.Core.Models;
using ZK.TaskManager.Core.Task;
using ZK.TaskManager.Utility;

namespace ZK.TaskManager.Node
{
    public class ProcessHandle
    {
        /// <summary>
        /// 处理接收到的命令
        /// </summary>
        public static void Execute(string json)
        {
            var msg = FormatHelper.JsonDeserializer<MessageModel>(json);
            if (msg == null)
            {
                Log.SysLog("节点：" + GlobalConfig.NodeID + "消息接收失败");
                return;
            }
            JobHelper.Execute(msg);
        }




    }
}
