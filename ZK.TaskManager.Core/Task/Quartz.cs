using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZK.TaskManager.Core.Models;
using ZK.TaskManager.Utility;

namespace ZK.TaskManager.Core.Task
{
    public class Quartz
    {
        private static object obj = new object();

        /// <summary>
        /// 缓存任务所在程序集信息
        /// </summary>
        private static Dictionary<string, Assembly> AssemblyDict = new Dictionary<string, Assembly>();

        private static IScheduler scheduler = null;

        /// <summary>
        /// 初始化任务调度对象
        /// </summary>
        public static void InitScheduler()
        {
            try
            {
                lock (obj)
                {
                    if (scheduler == null)
                    {
                        NameValueCollection properties = new NameValueCollection();

                        properties["quartz.scheduler.instanceName"] = "QTaskManager";

                        properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";

                        properties["quartz.threadPool.threadCount"] = "10";

                        properties["quartz.threadPool.threadPriority"] = "Normal";

                        properties["quartz.jobStore.misfireThreshold"] = "60000";


                        ISchedulerFactory factory = new StdSchedulerFactory(properties);

                        scheduler = factory.GetScheduler();
                        scheduler.Clear();
                        // LogHelper.WriteLog("任务调度初始化成功！");
                    }
                }
            }
            catch (Exception ex)
            {
                // LogHelper.WriteLog("任务调度初始化失败！", ex);
            }
        }

        /// <summary>
        /// 启用任务调度
        /// 启动调度时会把任务表中状态为“执行中”的任务加入到任务调度队列中
        /// </summary>
        public static void StartScheduler()
        {
            try
            {
                if (!scheduler.IsStarted)
                {
                    //添加全局监听
                    scheduler.ListenerManager.AddTriggerListener(new CustomTriggerListener(), GroupMatcher<TriggerKey>.AnyGroup());
                    scheduler.Start();

                    ///获取所有执行中的任务
                    //List<TaskUtil> listTask = TaskHelper.List();
                    //if (listTask != null && listTask.Count > 0)
                    //{
                    //    foreach (TaskUtil taskUtil in listTask)
                    //    {
                    //        try
                    //        {
                    //            ScheduleJob(taskUtil);
                    //        }
                    //        catch (Exception e)
                    //        {
                    //            //LogHelper.WriteLog(string.Format("任务“{0}”启动失败！", taskUtil.TaskName), e);
                    //        }
                    //    }
                    //}
                    // LogHelper.WriteLog("任务调度启动成功！");
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog("任务调度启动失败！", ex);
            }
        }

        /// <summary>
        /// 删除现有任务
        /// </summary>
        public static void DeleteJob(string TaskID)
        {
            JobKey jk = new JobKey(TaskID);
            if (scheduler.CheckExists(jk))
            {
                //任务已经存在 则删除
                scheduler.DeleteJob(jk);
                // LogHelper.WriteLog(string.Format("任务“{0}”已经删除", JobKey));
            }
        }

        /// <summary>
        /// 启用任务
        /// </summary>
        public static void StartJob(TaskModel taskUtil)
        {
            //验证是否正确的Cron表达式
            if (ValidExpression(taskUtil.CronExpressionString))
            {
                IJobDetail job = new JobDetailImpl(taskUtil.TaskID, GetClassInfo(taskUtil.AssemblyName, taskUtil.NameSpaceAndClass));
                CronTriggerImpl trigger = new CronTriggerImpl();
                trigger.CronExpressionString = taskUtil.CronExpressionString;
                trigger.Name = taskUtil.TaskID;
                trigger.Description = taskUtil.TaskName;
                //添加任务执行参数
                job.JobDataMap.Add("TaskParam", taskUtil.TaskParam);
                scheduler.ScheduleJob(job, trigger);

                if (taskUtil.Status == Models.TaskStatus.STOP)
                {
                    JobKey jk = new JobKey(taskUtil.TaskID);
                    scheduler.PauseJob(jk);
                }
                else
                {
                    //LogHelper.WriteLog(string.Format("任务“{0}”启动成功,未来5次运行时间如下:", taskUtil.TaskName));
                    List<DateTime> list = GetTaskeFireTime(taskUtil.CronExpressionString, 1);
                    foreach (var time in list)
                    {
                        // LogHelper.WriteLog(time.ToString());
                    }
                }
            }
            else
            {
                throw new Exception(taskUtil.CronExpressionString + "不是正确的Cron表达式,无法启动该任务!");
            }
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="JobKey"></param>
        public static void PauseJob(string TaskID)
        {
            JobKey jk = new JobKey(TaskID);
            if (scheduler.CheckExists(jk))
            {
                //任务已经存在则暂停任务
                scheduler.PauseJob(jk);

                //LogHelper.WriteLog(string.Format("任务“{0}”已暂停", JobKey));
            }
        }
        /// <summary>
        /// 恢复暂停的任务
        /// </summary>
        /// <param name="JobKey">任务key</param>
        public static void ResumeJob(string TaskID)
        {
            JobKey jk = new JobKey(TaskID);
            if (scheduler.CheckExists(jk))
            {
                //任务已经存在则恢复运行任务
                scheduler.ResumeJob(jk);
                //LogHelper.WriteLog(string.Format("任务“{0}”已恢复运行", JobKey));
            }
        }

        private static Type GetClassInfo(string assemblyName, string className)
        {
            try
            {
                var assemblyPath = FileHelper.GetAbsolutePath(GlobalConfig.TaskDllDir + assemblyName);
                Assembly assembly = null;
                if (!AssemblyDict.TryGetValue(assemblyPath, out assembly))
                {
                    assembly = Assembly.LoadFrom(assemblyPath);
                    AssemblyDict[assemblyPath] = assembly;
                }
                Type type = assembly.GetType(className, true, true);
                return type;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 校验字符串是否为正确的Cron表达式
        /// </summary>
        /// <param name="cronExpression">带校验表达式</param>
        /// <returns></returns>
        public static bool ValidExpression(string cronExpression)
        {
            return CronExpression.IsValidExpression(cronExpression);
        }

        /// <summary>
        /// 获取任务在未来周期内哪些时间会运行
        /// </summary>
        /// <param name="CronExpressionString">Cron表达式</param>
        /// <param name="numTimes">运行次数</param>
        /// <returns>运行时间段</returns>
        public static List<DateTime> GetTaskeFireTime(string CronExpressionString, int numTimes)
        {
            if (numTimes < 0)
            {
                throw new Exception("参数numTimes值大于等于0");
            }
            //时间表达式
            ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(CronExpressionString).Build();
            IList<DateTimeOffset> dates = TriggerUtils.ComputeFireTimes(trigger as IOperableTrigger, null, numTimes);
            List<DateTime> list = new List<DateTime>();
            foreach (DateTimeOffset dtf in dates)
            {
                list.Add(TimeZoneInfo.ConvertTimeFromUtc(dtf.DateTime, TimeZoneInfo.Local));
            }
            return list;
        }
    }
}
