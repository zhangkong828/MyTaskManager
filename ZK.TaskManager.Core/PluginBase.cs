using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.Core
{
    public abstract class PluginBase : IJob
    {
        public PluginBase()
        {

        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Run();
            }
            catch (Exception ex)
            {
                Log.SysLog("执行Job异常", ex);
            }
           
        }

        public abstract void Run();
    }
}
