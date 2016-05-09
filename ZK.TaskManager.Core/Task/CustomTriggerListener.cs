using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.Core.Task
{
    /// <summary>
    /// 自定义触发器监听
    /// </summary>
    public class CustomTriggerListener : ITriggerListener
    {
        public string Name
        {
            get
            {
                return "All_TriggerListener";
            }
        }

        /// <summary>
        /// Trigger命中时
        /// </summary>
        /// <param name="trigger">触发器</param>
        /// <param name="context">上下文</param>
        public void TriggerFired(ITrigger trigger, IJobExecutionContext context)
        {
            //Console.WriteLine("开始");
        }


        /// <summary>
        ///  //Trigger触发后，job执行前调用本方法。true即否决，job后面不执行。
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool VetoJobExecution(ITrigger trigger, IJobExecutionContext context)
        {
            //Console.WriteLine("执行");
            return false;
        }

        /// <summary>
        /// Job完成时调用
        /// </summary>
        /// <param name="trigger">触发器</param>
        /// <param name="context">上下文</param>
        /// <param name="triggerInstructionCode"></param>
        public void TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode)
        {
            //Console.WriteLine("完成");
        }

        /// <summary>
        /// 错过触发时调用
        /// </summary>
        /// <param name="trigger">触发器</param>
        public void TriggerMisfired(ITrigger trigger)
        {
            //Console.WriteLine("错过");
        }
    }
}
