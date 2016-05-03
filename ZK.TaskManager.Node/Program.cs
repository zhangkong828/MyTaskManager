using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.Node
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketClient.Init();

            Console.Read();
        }
    }
}
