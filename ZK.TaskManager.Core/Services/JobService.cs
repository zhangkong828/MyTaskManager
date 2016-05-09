using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                var sql = "select * from (select Id, Name, CreateOn, Cron, CronRemark, TaskId, Param, NodeId, Status, ROW_NUMBER() over(order by CreateOn " + order + ") as ranknum from dbo.Job with(nolock)) as T1 where T1.ranknum between @pagesize * (@pageindex - 1) + 1 and @pagesize*@pageindex";
                SqlParameter[] pms = {
                        new SqlParameter("@pageindex",pageindex),
                        new SqlParameter("@pagesize",pagesize)
                    };
                using (SqlDataReader reader = SqlHelper.ExecuteReader(sql, System.Data.CommandType.Text))
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


    }
}
