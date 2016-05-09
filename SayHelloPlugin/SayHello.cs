using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZK.TaskManager.Core;

namespace SayHelloPlugin
{
    public class SayHello : PluginBase
    {
        public override void Run()
        {
            Console.WriteLine("Say Hello " + DateTime.Now.Ticks + " | " + DateTime.Now.ToString());
        }
    }
}
