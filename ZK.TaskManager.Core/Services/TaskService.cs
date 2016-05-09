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
    public class TaskService
    {
        public static List<TaskModel> List()
        {
            var list = new List<TaskModel>();
            try
            {
                var sql = "select Id, Name, ParamRemark, TaskDirName, TaskAssembly, TaskNameSpaceAndClass, CreateOn, ModifyOn, Remark, Verson from dbo.Task order by CreateOn asc";
                using (SqlDataReader reader = SqlHelper.ExecuteReader(sql, System.Data.CommandType.Text))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var model = new TaskModel();
                            model.Id = reader.GetString(0);
                            model.Name = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            model.TaskParamRemark = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            model.TaskDirName = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            model.Assembly = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            model.NameSpaceAndClass = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            model.CreateOn = reader.GetDateTime(6).ToString("yyyy-MM-dd HH:mm:ss");
                            model.ModifyOn = reader.GetDateTime(7).ToString("yyyy-MM-dd HH:mm:ss");
                            model.Remark = reader.IsDBNull(8) ? "" : reader.GetString(8);
                            model.Verson = reader.IsDBNull(9) ? "" : reader.GetString(9);
                            list.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.SysLog("获取Task集合出现异常", ex);
            }
            return list;
        }

        public static TaskModel Query(string taskid)
        {
            try
            {
                var sql = "select Id, Name, ParamRemark, TaskDirName, TaskAssembly, TaskNameSpaceAndClass, CreateOn, ModifyOn, Remark, Verson from dbo.Task where Id=@Id";
                using (SqlDataReader reader = SqlHelper.ExecuteReader(sql, System.Data.CommandType.Text, new SqlParameter("@Id", taskid)))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        var model = new TaskModel();
                        model.Id = reader.GetString(0);
                        model.Name = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        model.TaskParamRemark = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        model.TaskDirName = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        model.Assembly = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        model.NameSpaceAndClass = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        model.CreateOn = reader.GetDateTime(6).ToString("yyyy-MM-dd HH:mm:ss");
                        model.ModifyOn = reader.GetDateTime(7).ToString("yyyy-MM-dd HH:mm:ss");
                        model.Remark = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        model.Verson = reader.IsDBNull(9) ? "" : reader.GetString(9);
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
                Log.SysLog("获取Task出现异常", ex);
                return null;
            }
        }


        public static bool Add(TaskModel task)
        {
            var result = false;
            try
            {
                var sql = "insert into dbo.task(Id, Name, ParamRemark, TaskDirName, TaskAssembly, TaskNameSpaceAndClass, CreateOn, ModifyOn, Remark, Verson) values(@Id, @Name, @ParamRemark, @TaskDirName, @TaskAssembly, @TaskNameSpaceAndClass, @CreateOn, @ModifyOn, @Remark, @Verson) ";
                SqlParameter[] pms = {
                    new SqlParameter("@Id",task.Id),
                    new SqlParameter("@Name",task.Name),
                    new SqlParameter("@ParamRemark",task.TaskParamRemark),
                    new SqlParameter("@TaskDirName",task.TaskDirName),
                    new SqlParameter("@TaskAssembly",task.Assembly),
                    new SqlParameter("@TaskNameSpaceAndClass",task.NameSpaceAndClass),
                    new SqlParameter("@CreateOn",task.CreateOn),
                    new SqlParameter("@ModifyOn",task.ModifyOn),
                    new SqlParameter("@Remark",task.Remark),
                    new SqlParameter("@Verson",task.Verson)
                };
                result = SqlHelper.ExecuteNonQuery(sql, System.Data.CommandType.Text, pms) > 0;
            }
            catch (Exception ex)
            {
                Log.SysLog("添加Task出现异常", ex);
            }
            return result;
        }

        public static bool Remove(string taskid)
        {
            var result = false;
            try
            {
                var sql = "delete from dbo.task where Id=@Id";
                SqlParameter pms = new SqlParameter("@Id", taskid);
                result = SqlHelper.ExecuteNonQuery(sql, System.Data.CommandType.Text, pms) > 0;
            }
            catch (Exception ex)
            {
                Log.SysLog("删除Task出现异常", ex);
            }
            return result;
        }

        public static bool Update(TaskModel task)
        {
            var result = false;
            try
            {
                var sql = "update dbo.task set Name=@Name, ParamRemark=@ParamRemark, TaskDirName=@TaskDirName, TaskAssembly=@TaskAssembly, TaskNameSpaceAndClass=@TaskNameSpaceAndClass, CreateOn=@CreateOn, ModifyOn=@ModifyOn, Remark=@Remark, Verson=@Verson where Id=@Id";
                SqlParameter[] pms = {
                    new SqlParameter("@Id",task.Id),
                    new SqlParameter("@Name",task.Name),
                    new SqlParameter("@ParamRemark",task.TaskParamRemark),
                    new SqlParameter("@TaskDirName",task.TaskDirName),
                    new SqlParameter("@TaskAssembly",task.Assembly),
                    new SqlParameter("@TaskNameSpaceAndClass",task.NameSpaceAndClass),
                    new SqlParameter("@CreateOn",task.CreateOn),
                    new SqlParameter("@ModifyOn",task.ModifyOn),
                    new SqlParameter("@Remark",task.Remark),
                    new SqlParameter("@Verson",task.Verson)
                };
                result = SqlHelper.ExecuteNonQuery(sql, System.Data.CommandType.Text, pms) > 0;
            }
            catch (Exception ex)
            {
                Log.SysLog("更新Task出现异常", ex);
            }
            return result;
        }

    }
}
