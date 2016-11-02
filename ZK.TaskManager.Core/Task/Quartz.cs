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

        public static IScheduler scheduler = null;

        /// <summary>
        /// 初始化任务调度对象
        /// </summary>
        public static void InitScheduler()
        {
            lock (obj)
            {
                if (scheduler == null)
                {
                    NameValueCollection properties = new NameValueCollection();

                    properties["quartz.scheduler.instanceName"] = "MyTaskManager";

                    properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";

                    properties["quartz.threadPool.threadCount"] = "50";

                    properties["quartz.threadPool.threadPriority"] = "Normal";

                    properties["quartz.jobStore.misfireThreshold"] = "60000";

                    ISchedulerFactory factory = new StdSchedulerFactory(properties);

                    scheduler = factory.GetScheduler();
                    scheduler.Clear();

                }
            }

        }

        /// <summary>
        /// 启用任务调度
        /// 启动调度时会把任务表中状态为“执行中”的任务加入到任务调度队列中
        /// </summary>
        public static void StartScheduler()
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

            }

        }



        /// <summary>
        /// 启用任务
        /// </summary>
        public static bool StartJob(JobModel jobmodel)
        {
            //验证是否正确的Cron表达式
            if (ValidExpression(jobmodel.Cron))
            {
                IJobDetail job = new JobDetailImpl(jobmodel.Id, GetClassInfo(jobmodel.Id, jobmodel.Task.TaskDirName, jobmodel.Task.Assembly, jobmodel.Task.NameSpaceAndClass));
                CronTriggerImpl trigger = new CronTriggerImpl();
                trigger.CronExpressionString = jobmodel.Cron;
                trigger.Name = jobmodel.Id;
                trigger.Description = jobmodel.Name;
                //添加任务执行参数
                job.JobDataMap.Add("TaskParam", jobmodel.Param);
                scheduler.ScheduleJob(job, trigger);

                //LogHelper.WriteLog(string.Format("任务“{0}”启动成功,未来5次运行时间如下:", taskUtil.TaskName));
                //List<DateTime> list = GetTaskeFireTime(jobmodel.Cron, 1);
                //foreach (var time in list)
                //{
                //    // LogHelper.WriteLog(time.ToString());
                //}
                return true;
            }
            else
            {
                return false;
            }
        }

        private static Type GetClassInfo(string jobid, string dirname, string assemblyName, string className)
        {
            try
            {
                var assemblyPath = System.IO.Path.Combine(GlobalConfig.TaskPluginDir, GlobalConfig.TaskPluginDirSrc,dirname, assemblyName);
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
                Log.NodeLog(GlobalConfig.NodeID, "Job在反射加载任务时出现异常", ex);
                return null;
            }
        }

        /// <summary>
        /// 删除现有任务
        /// </summary>
        public static bool DeleteJob(JobModel jobmodel)
        {
            JobKey jk = new JobKey(jobmodel.Id);
            if (scheduler.CheckExists(jk))
            {
                //任务已经存在 则删除
                scheduler.DeleteJob(jk);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="JobKey"></param>
        public static bool PauseJob(JobModel jobmodel)
        {
            JobKey jk = new JobKey(jobmodel.Id);
            if (scheduler.CheckExists(jk))
            {
                //任务已经存在则暂停任务
                scheduler.PauseJob(jk);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 恢复暂停的任务
        /// </summary>
        /// <param name="JobKey">任务key</param>
        public static bool ResumeJob(JobModel jobmodel)
        {
            JobKey jk = new JobKey(jobmodel.Id);
            if (scheduler.CheckExists(jk))
            {
                //任务已经存在则恢复运行任务
                scheduler.ResumeJob(jk);
                return true;
            }
            return false;
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


        public static int GetCount()
        {
            var jobkeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());
            return jobkeys.Count;
        }

    }
}
