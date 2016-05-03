using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZK.TaskManager.Core.Models;

namespace ZK.TaskManager.Core.Task
{
    public class TaskHelper
    {
        private static string InsertSQL = @"INSERT INTO dbo.p_Task(TaskID,TaskName,TaskParam,CronExpressionString,Assembly,Class,Status,CronRemark,Remark,LastRunTime)
                            VALUES(@TaskID,@TaskName,@TaskParam,@CronExpressionString,@Assembly,@Class,@Status,@CronRemark,@Remark,@LastRunTime)";

        private static string UpdateSQL = @"UPDATE dbo.p_Task SET TaskName=@TaskName,TaskParam=@TaskParam,CronExpressionString=@CronExpressionString,Assembly=@Assembly,
                                Class=@Class,CronRemark=@CronRemark,Remark=@Remark,LastRunTime=@LastRunTime WHERE TaskID=@TaskID";
        /// <summary>
        /// 获取指定id任务
        /// </summary>
        /// <param name="TaskID">任务id</param>
        /// <returns>任务数据</returns>
        public static TaskModel GetById(string TaskID)
        {
            return new TaskModel();
        }


        /// <summary>
        /// 添加新任务
        /// </summary>
        public static void AddTask(TaskModel taskUtil)
        {
            Quartz.StartJob(taskUtil);
            //SQLHelper.ExecuteNonQuery("DELETE FROM p_Task WHERE TaskID=@TaskID", new { TaskID = TaskID });
        }

        /// <summary>
        /// 删除指定id任务
        /// </summary>
        /// <param name="TaskID">任务id</param>
        public static void DeleteById(string TaskID)
        {
            Quartz.DeleteJob(TaskID);
            //SQLHelper.ExecuteNonQuery("DELETE FROM p_Task WHERE TaskID=@TaskID", new { TaskID = TaskID });
        }

        /// <summary>
        /// 更新任务运行状态
        /// </summary>
        /// <param name="TaskID">任务id</param>
        /// <param name="Status">任务状态</param>
        public static void UpdateTaskStatus(string TaskID, Models.TaskStatus Status)
        {
            if (Status == Models.TaskStatus.RUN)
            {
                Quartz.ResumeJob(TaskID);
            }
            else if(Status == Models.TaskStatus.RUN)
            {
                Quartz.PauseJob(TaskID);
            }
            // SQLHelper.ExecuteNonQuery("UPDATE p_Task SET Status=@Status WHERE TaskID=@TaskID", new { TaskID = TaskID, Status = Status });
        }

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
            task.Id = "11111";
            task.Name = "1111";
            task.CronExpressionString = "0/5 * * * * ?";
            task.TaskParam = "每5秒执行一次~~~";
            task.AssemblyName = "SayHelloPlug.dll";
            task.NameSpaceAndClass = "SayHelloPlug.SayHello";

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
