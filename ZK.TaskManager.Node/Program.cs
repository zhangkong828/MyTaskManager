using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ZK.TaskManager.Core;
using ZK.TaskManager.Core.Task;

namespace ZK.TaskManager.Node
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalConfig.InitConfig();

            JobHelper.Init();

            SocketClient.Init();

            Console.Read();
        }
    }
}
