using System;
using System.Collections.Generic;
using System.IO;
using ZK.TaskManager.Core.Models;
using ZK.TaskManager.Core.Services;

namespace ZK.TaskManager.Core.Task
{
    public class JobHelper
    {
        public static void Init()
        {
            try
            {
                Quartz.InitScheduler();
                Log.SysLog("节点：" + GlobalConfig.NodeID + "初始化任务调度成功！");
            }
            catch (Exception ex)
            {
                Log.SysLog("节点：" + GlobalConfig.NodeID + " 初始化任务调度失败！", ex);
            }
            try
            {
                Quartz.StartScheduler();
                Log.SysLog("节点：" + GlobalConfig.NodeID + " 启动成功！");
            }
            catch (Exception ex)
            {
                Log.SysLog("节点：" + GlobalConfig.NodeID + " 启动失败！", ex);
            }
        }


        /// <summary>
        /// 获取当前job数量
        /// </summary>
        public static int GetJobCount()
        {
            try
            {
                return Quartz.GetCount();
            }
            catch (Exception ex)
            {
                Log.SysLog("节点：" + GlobalConfig.NodeID + " 获取当前job数量失败！", ex);
                return 999;
            }

        }

        /// <summary>
        /// 启动Job
        /// </summary>
        public static bool StartJob(JobModel job)
        {
            try
            {
                if (job.Status.ToLower() != "none")
                {
                    Log.NodeLog(GlobalConfig.NodeID, ConsoleMsg(job, "不是【未开始】的任务，无法启动!"));
                    return false;
                }
                var taskdir = job.Task.TaskDirName;
                if (Directory.Exists(taskdir))
                {
                    if (Quartz.StartJob(job))
                    {
                        if (JobService.UpdateState(job.Id, "start"))
                        {
                            Log.NodeLog(GlobalConfig.NodeID, ConsoleMsg(job, "已启动!"));
                            return true;
                        }
                    }
                    else
                    {
                        Log.NodeLog(GlobalConfig.NodeID, ConsoleMsg(job, "不是正确的Cron表达式，无法启动!"));
                    }
                }
                else
                {
                    Log.NodeLog(GlobalConfig.NodeID, ConsoleMsg(job, "在插件目录下，无法找到该插件！"));
                }
                return false;

            }
            catch (Exception ex)
            {
                Log.NodeLog(GlobalConfig.NodeID, string.Format("任务“{0}”[{1}]启动异常！", job.Name, job.Id), ex);
                return false;
            }
        }

        /// <summary>
        /// 删除Job
        /// </summary>
        public static bool DeleteJob(JobModel job)
        {
            try
            {
                if (Quartz.DeleteJob(job))
                {
                    if (JobService.Remove(job.Id))
                    {
                        Log.NodeLog(job.Id, string.Format("任务“{0}”已删除！", job.Name));
                        return true;
                    }
                }
                else
                {
                    Log.NodeLog(job.Id, string.Format("任务“{0}”删除失败！", job.Name));
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.NodeLog(GlobalConfig.NodeID, "删除Job出现异常！", ex);
                return false;
            }

        }

        /// <summary>
        /// 暂停Job
        /// </summary>
        public static bool PauseJob(JobModel job)
        {
            try
            {
                if (job.Status.ToLower() != "start")
                {

                    Log.NodeLog(job.Id, string.Format("任务“{0}”[{1}]不是【运行中】的任务，无法暂停!", job.Name, job.Id));
                    return false;
                }
                if (Quartz.PauseJob(job))
                {
                    if (JobService.UpdateState(job.Id, "pause"))
                    {
                        Log.NodeLog(job.Id, string.Format("任务“{0}”已暂停！", job.Name));
                        return true;
                    }
                }
                else
                {
                    Log.NodeLog(job.Id, string.Format("任务“{0}”暂停失败！", job.Name));
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.NodeLog(GlobalConfig.NodeID, "暂停Job出现异常！", ex);
                return false;
            }

        }

        /// <summary>
        /// 恢复Job
        /// </summary>
        public static bool ResumeJob(JobModel job)
        {
            try
            {
                if (job.Status.ToLower() != "pause")
                {

                    Log.NodeLog(job.Id, string.Format("任务“{0}”[{1}]不是【暂停中】的任务，无法恢复!", job.Name, job.Id));
                    return false;
                }
                if (Quartz.ResumeJob(job))
                {
                    if (JobService.UpdateState(job.Id, "start"))
                    {
                        Log.NodeLog(job.Id, string.Format("任务“{0}”已恢复！", job.Name));
                        return true;
                    }
                }
                else
                {
                    Log.NodeLog(job.Id, string.Format("任务“{0}”恢复失败！", job.Name));
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.NodeLog(GlobalConfig.NodeID, "恢复Job出现异常！", ex);
                return false;
            }

        }

        public static string ConsoleMsg(JobModel job, string msg)
        {
            return string.Format("Id:[{0}],Name:[{1}],Output: {2}", job.Id, job.Name, msg);
        }

        /// <summary>
        /// 更新任务运行状态
        /// </summary>
        /// <param name="TaskID">任务id</param>
        /// <param name="Status">任务状态</param>
        //public static void UpdateTaskStatus(string TaskID, Models.TaskStatus Status)
        //{
        //    if (Status == Models.TaskStatus.RUN)
        //    {
        //        Quartz.ResumeJob(TaskID);
        //    }
        //    else if (Status == Models.TaskStatus.RUN)
        //    {
        //        Quartz.PauseJob(TaskID);
        //    }
        //    // SQLHelper.ExecuteNonQuery("UPDATE p_Task SET Status=@Status WHERE TaskID=@TaskID", new { TaskID = TaskID, Status = Status });
        //}

        /// <summary>
        /// 更新任务下次运行时间
        /// </summary>
        /// <param name="TaskID">任务id</param>
        /// <param name="LastRunTime">下次运行时间</param>
        public static void UpdateLastRunTime(string TaskID, DateTime LastRunTime)
        {
            //SQLHelper.ExecuteNonQuery("UPDATE p_Task SET LastRunTime=@LastRunTime WHERE TaskID=@TaskID", new { TaskID = TaskID, LastRunTime = LastRunTime });
        }

        /// <summary>
        /// 更新任务最近运行时间
        /// </summary>
        /// <param name="TaskID">任务id</param>
        public static void UpdateRecentRunTime(string TaskID, DateTime LastRunTime)
        {
            //SQLHelper.ExecuteNonQuery("UPDATE p_Task SET RecentRunTime=GETDATE(),LastRunTime=@LastRunTime WHERE TaskID=@TaskID", new { TaskID = TaskID, LastRunTime = LastRunTime });
        }

        /// <summary>
        /// 获取所有启用的任务
        /// </summary>
        /// <returns>所有启用的任务</returns>
        public static List<TaskModel> List()
        {
            var list = new List<TaskModel>();

            var task = new TaskModel();
            //task.Id = "11111";
            //task.Name = "1111";
            //task.CronExpressionString = "0/5 * * * * ?";
            //task.TaskParam = "每5秒执行一次~~~";
            //task.Assembly = "SayHelloPlug.dll";
            //task.NameSpaceAndClass = "SayHelloPlug.SayHello";

            list.Add(task);
            return list;
        }

        /// <summary>
        /// 根据条件查询任务
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>符合条件的任务</returns>
        //public static JsonBaseModel<List<TaskUtil>> Query(QueryCondition condition)
        //{
        //    JsonBaseModel<List<TaskUtil>> result = new JsonBaseModel<List<TaskUtil>>();
        //    if (string.IsNullOrEmpty(condition.SortField))
        //    {
        //        condition.SortField = "CreatedOn";
        //        condition.SortOrder = "DESC";
        //    }
        //    Hashtable ht = Pagination.QueryBase<TaskUtil>("SELECT * FROM p_Task", condition);
        //    result.Result = ht["data"] as List<TaskUtil>;
        //    result.TotalCount = Convert.ToInt32(ht["total"]);
        //    result.TotalPage = result.CalculateTotalPage(condition.PageSize, result.TotalCount.Value, condition.IsPagination);
        //    return result;
        //}

        /// <summary>
        /// 保存任务
        /// </summary>
        /// <param name="value">任务</param>
        /// <returns>保存结果</returns>
        //public static JsonBaseModel<string> SaveTask(TaskUtil value)
        //{
        //    JsonBaseModel<string> result = new JsonBaseModel<string>();
        //    result.HasError = true;
        //    if (value == null)
        //    {
        //        result.Message = "参数空异常";
        //        return result;
        //    }

        //    #region "校验"
        //    if (string.IsNullOrEmpty(value.TaskName))
        //    {
        //        result.Message = "任务名称不能为空";
        //        return result;
        //    }
        //    if (string.IsNullOrEmpty(value.Assembly))
        //    {
        //        result.Message = "程序集名称不能为空";
        //        return result;
        //    }
        //    if (string.IsNullOrEmpty(value.CronExpressionString))
        //    {
        //        result.Message = "Cron表达式不能为空";
        //        return result;
        //    }
        //    if (!QuartzHelper.ValidExpression(value.CronExpressionString))
        //    {
        //        result.Message = "Cron表达式格式不正确";
        //        return result;
        //    }
        //    if (string.IsNullOrEmpty(value.CronRemark))
        //    {
        //        result.Message = "表达式说明不能为空";
        //        return result;
        //    }
        //    if (string.IsNullOrEmpty(value.Class))
        //    {
        //        result.Message = "类名不能为空";
        //        return result;
        //    }
        //    #endregion

        //    JsonBaseModel<DateTime> cronResult = null;
        //    try
        //    {
        //        //新增
        //        if (value.TaskID == Guid.Empty)
        //        {
        //            value.TaskID = Guid.NewGuid();
        //            //任务状态处理

        //            cronResult = GetTaskeLastRunTime(value.CronExpressionString);
        //            if (cronResult.HasError)
        //            {
        //                result.Message = cronResult.Message;
        //                return result;
        //            }
        //            else
        //            {
        //                value.LastRunTime = cronResult.Result;
        //            }
        //            //添加新任务
        //            QuartzHelper.ScheduleJob(value);

        //            SQLHelper.ExecuteNonQuery(InsertSQL, value);
        //        }
        //        else
        //        {
        //            value.ModifyOn = DateTime.Now;
        //            TaskUtil srcTask = GetById(value.TaskID.ToString());

        //            //表达式改变了重新计算下次运行时间
        //            if (!value.CronExpressionString.Equals(srcTask.CronExpressionString, StringComparison.OrdinalIgnoreCase))
        //            {
        //                cronResult = GetTaskeLastRunTime(value.CronExpressionString);
        //                if (cronResult.HasError)
        //                {
        //                    result.Message = cronResult.Message;
        //                    return result;
        //                }
        //                else
        //                {
        //                    value.LastRunTime = cronResult.Result;
        //                }

        //                //更新任务
        //                QuartzHelper.ScheduleJob(value, true);
        //            }
        //            else
        //            {
        //                value.LastRunTime = srcTask.LastRunTime;
        //            }

        //            SQLHelper.ExecuteNonQuery(UpdateSQL, value);
        //        }

        //        result.HasError = false;
        //        result.Result = value.TaskID.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        result.HasError = true;
        //        result.Message = ex.Message;
        //    }
        //    return result;
        //}

        /// <summary>
        /// 计算任务下次运行时间
        /// </summary>
        /// <param name="CronExpressionString"></param>
        /// <returns>下次运行时间</returns>
        //private static JsonBaseModel<DateTime> GetTaskeLastRunTime(string CronExpressionString)
        //{
        //    JsonBaseModel<DateTime> result = new JsonBaseModel<DateTime>();
        //    try
        //    {
        //        //计算下次任务运行时间
        //        result.Result = QuartzHelper.GetTaskeFireTime(CronExpressionString, 1)[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        result.HasError = true;
        //        result.Message = "任务Cron表达式设置错误";
        //        LogHelper.WriteLog("任务Cron表达式设置错误", ex);
        //    }
        //    return result;
        //}
    }
}
