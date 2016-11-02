using System;
using System.Collections.Generic;
using ZK.TaskManager.Core.Models;
using System.Data.SqlClient;
using ZK.TaskManager.Utility;

namespace ZK.TaskManager.Core.Services
{
    public class JobService
    {
        public static List<JobModel> List(int pageindex, int pagesize, string order, out int count)
        {
            var list = new List<JobModel>();
            count = 0;
            try
            {
                var sql = "select * from (select jobid = T1.Id, jobname = T1.Name, jobcreateon = T1.CreateOn, T1.Cron, T1.CronRemark, T1.TaskId, T1.Param, T1.NodeId, T1.Status, ROW_NUMBER() over(order by T1.CreateOn " + order + ") as ranknum, T2.Id, T2.Name, T2.ParamRemark, T2.TaskDirName, T2.TaskAssembly, T2.TaskNameSpaceAndClass, T2.CreateOn, T2.ModifyOn, T2.Remark, T2.Verson from dbo.Job as T1 with(nolock)inner join dbo.Task as T2 with(nolock) on T1.TaskId = T2.Id) as T3 where T3.ranknum between @pagesize * (@pageindex - 1) + 1 and @pagesize*@pageindex";
                SqlParameter[] pms = {
                        new SqlParameter("@pageindex",pageindex),
                        new SqlParameter("@pagesize",pagesize)
                    };
                using (SqlDataReader reader = SqlHelper.ExecuteReader(sql, System.Data.CommandType.Text,pms))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var model = new JobModel();
                            model.Id = reader.GetString(0);
                            model.Name = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            model.CreateOn = reader.IsDBNull(2) ? "" : reader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss");
                            model.Cron = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            model.CronRemark = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            model.TaskId = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            model.Param = reader.IsDBNull(6) ? "" : reader.GetString(6);
                            model.NodeId = reader.IsDBNull(7) ? "" : reader.GetString(7);
                            model.Status = reader.IsDBNull(8) ? "" : reader.GetString(8);

                            model.Task.Id = reader.GetString(10);
                            model.Task.Name = reader.IsDBNull(11) ? "" : reader.GetString(11);
                            model.Task.TaskParamRemark = reader.IsDBNull(12) ? "" : reader.GetString(12);
                            model.Task.TaskDirName = reader.IsDBNull(13) ? "" : reader.GetString(13);
                            model.Task.Assembly = reader.IsDBNull(14) ? "" : reader.GetString(14);
                            model.Task.NameSpaceAndClass = reader.IsDBNull(15) ? "" : reader.GetString(15);
                            model.Task.CreateOn = reader.GetDateTime(16).ToString("yyyy-MM-dd HH:mm:ss");
                            model.Task.ModifyOn = reader.GetDateTime(17).ToString("yyyy-MM-dd HH:mm:ss");
                            model.Task.Remark = reader.IsDBNull(18) ? "" : reader.GetString(18);
                            model.Task.Verson = reader.IsDBNull(19) ? "" : reader.GetString(19);
                            list.Add(model);
                        }
                    }
                }
                var sql2 = "select count(1) from dbo.Job with(nolock)";
                count = (int)SqlHelper.ExecuteScalar(sql2, System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                Log.SysLog("获取Job集合出现异常", ex);
            }
            return list;
        }


        public static JobModel Query(string jobid)
        {
            try
            {
                var sql = "select Id, Name, CreateOn, Cron, CronRemark, TaskId, Param, NodeId, Status from dbo.Job with(nolock) where Id=@Id";
                using (SqlDataReader reader = SqlHelper.ExecuteReader(sql, System.Data.CommandType.Text, new SqlParameter("@Id", jobid)))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        var model = new JobModel();
                        model.Id = reader.GetString(0);
                        model.Name = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        model.CreateOn = reader.IsDBNull(2) ? "" : reader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss");
                        model.Cron = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        model.CronRemark = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        model.TaskId = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        model.Param = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        model.NodeId = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        model.Status = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        return model;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.SysLog("获取Job出现异常", ex);
                return null;
            }
        }

        public static bool Add(JobModel job)
        {
            var result = false;
            try
            {
                var sql = "insert into dbo.job(Id, Name, CreateOn, Cron, CronRemark, TaskId, Param, NodeId, Status) values(@Id, @Name, @CreateOn, @Cron, @CronRemark, @TaskId, @Param, @NodeId, @Status) ";
                SqlParameter[] pms = {
                    new SqlParameter("@Id",job.Id),
                    new SqlParameter("@Name",job.Name),
                    new SqlParameter("@CreateOn",job.CreateOn),
                    new SqlParameter("@Cron",job.Cron),
                    new SqlParameter("@CronRemark",job.CronRemark),
                    new SqlParameter("@TaskId",job.TaskId),
                    new SqlParameter("@Param",job.Param),
                    new SqlParameter("@NodeId",job.NodeId),
                    new SqlParameter("@Status",job.Status)
                };
                result = SqlHelper.ExecuteNonQuery(sql, System.Data.CommandType.Text, pms) > 0;
            }
            catch (Exception ex)
            {
                Log.SysLog("添加Job出现异常", ex);
            }
            return result;
        }
        public static bool Update(JobModel job)
        {
            var result = false;
            try
            {
                var sql = "update dbo.job set Name=@Name, CreateOn=@CreateOn, Cron=@Cron, CronRemark=@CronRemark, TaskId=@TaskId, Param=@Param, NodeId=@NodeId, Status=@Status where Id=@Id";
                SqlParameter[] pms = {
                    new SqlParameter("@Id",job.Id),
                    new SqlParameter("@Name",job.Name),
                    new SqlParameter("@CreateOn",job.CreateOn),
                    new SqlParameter("@Cron",job.Cron),
                    new SqlParameter("@CronRemark",job.CronRemark),
                    new SqlParameter("@TaskId",job.TaskId),
                    new SqlParameter("@Param",job.Param),
                    new SqlParameter("@NodeId",job.NodeId),
                    new SqlParameter("@Status",job.Status)
                };
                result = SqlHelper.ExecuteNonQuery(sql, System.Data.CommandType.Text, pms) > 0;
            }
            catch (Exception ex)
            {
                Log.SysLog("更新Job出现异常", ex);
            }
            return result;
        }

        public static bool Remove(string jobid)
        {
            var result = false;
            try
            {
                var sql = "delete from dbo.job where Id=@Id";
                SqlParameter pms = new SqlParameter("@Id", jobid);
                result = SqlHelper.ExecuteNonQuery(sql, System.Data.CommandType.Text, pms) > 0;
            }
            catch (Exception ex)
            {
                Log.SysLog("删除job出现异常", ex);
            }
            return result;
        }

        public static bool UpdateState(string jobid,string state)
        {
            var result = false;
            try
            {
                var sql = "update dbo.job set Status=@Status where Id=@Id";
                SqlParameter[] pms = {
                    new SqlParameter("@Id",jobid),
                    new SqlParameter("@Status",state)
                };
                result = SqlHelper.ExecuteNonQuery(sql, System.Data.CommandType.Text, pms) > 0;
            }
            catch (Exception ex)
            {
                Log.SysLog("更新Job状态出现异常", ex);
            }
            return result;
        }
    }
}
