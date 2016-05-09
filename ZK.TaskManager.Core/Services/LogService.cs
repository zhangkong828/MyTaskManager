using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZK.TaskManager.Core.Models;
using ZK.TaskManager.Utility;

namespace ZK.TaskManager.Core.Services
{
    public class LogService
    {
        public static bool SysAdd(string msg)
        {
            return Add(msg, null, "0", "");
        }
        public static bool SysAdd(string msg, Exception ex)
        {
            return Add(msg, ex, "0", "");
        }
        public static bool JobAdd(string jobid, string msg)
        {
            return Add(msg, null, "1", jobid);
        }
        public static bool JobAdd(string jobid, string msg, Exception ex)
        {
            return Add(msg, ex, "1", jobid);
        }

        public static bool Add(string msg, Exception ex, string type, string jobid)
        {
            try
            {
                var sql = "insert into dbo.log(id,time,msg,exmessage,type,jobid) values(@id,@time,@msg,@exmessage,@type,@jobid)";
                SqlParameter[] pms = {
                new SqlParameter("@id",Guid.NewGuid().ToString("n")),
                 new SqlParameter("@time",DateTime.Now),
                  new SqlParameter("@msg",ex==null?msg:msg+ex.Message),
                   new SqlParameter("@exmessage",ex==null?"":string.Format("Message：{0},StackTrace：{1}",ex.Message,ex.StackTrace)),
                    new SqlParameter("@type",type),
                     new SqlParameter("@jobid",jobid)
            };
                return SqlHelper.ExecuteNonQuery(sql, System.Data.CommandType.Text, pms) > 0;

            }
            catch (Exception e)
            {
                Console.WriteLine("严重！！！日志记录异常！" + e.Message);
                return false;
            }
        }

        public static List<LogModel> SysQuery(int pageindex, int pagesize, string order, out int totalcount)
        {
            return Query("0", "", pageindex, pagesize, order, out totalcount);
        }
        public static List<LogModel> JobQuery(string jobid, int pageindex, int pagesize, string order, out int totalcount)
        {
            return Query("1", jobid, pageindex, pagesize, order, out totalcount);
        }

        public static List<LogModel> Query(string type, string jobid, int pageindex, int pagesize, string order, out int totalcount)
        {
            var result = new List<LogModel>();
            totalcount = 0;
            try
            {
                var sql = "";
                var sql2 = "";
                switch (type)
                {
                    case "0":
                        sql = "select * from(select Id, Time, Msg, ExMessage, Type, JobId, ROW_NUMBER() over(order by time " + order + ") as ranknum from dbo.log with(nolock) where type = @type) as T1 where T1.ranknum between @pagesize*(@pageindex-1)+1 and @pagesize*@pageindex";
                        sql2 = "select count(1) from dbo.Log with(nolock) where type=@type";
                        break;
                    case "1":
                        sql = "select * from(select Id, Time, Msg, ExMessage, Type, JobId, ROW_NUMBER() over(order by time " + order + ") as ranknum from dbo.log with(nolock) where type = @type and jobid=@jobid) as T1 where T1.ranknum between @pagesize*(@pageindex-1)+1 and @pagesize*@pageindex";
                        sql2 = "select count(1) from dbo.Log with(nolock) where type=@type and jobid=@jobid";
                        break;
                }
                SqlParameter[] pms = {
                        new SqlParameter("@type",type),
                        new SqlParameter("@jobid",jobid),
                        new SqlParameter("@pageindex",pageindex),
                        new SqlParameter("@pagesize",pagesize)
                    };
                using (SqlDataReader reader = SqlHelper.ExecuteReader(sql, System.Data.CommandType.Text, pms))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var model = new LogModel();
                            model.Id = reader.GetString(0);
                            model.Time = reader.GetDateTime(1).ToString("yyyy-MM-dd HH:mm:ss");
                            model.Msg = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            model.ExMessage = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            model.Type = reader.GetString(4);
                            model.JobId = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            result.Add(model);
                        }
                    }
                }
                totalcount = (int)SqlHelper.ExecuteScalar(sql2, System.Data.CommandType.Text, new SqlParameter[] { new SqlParameter("@type", type), new SqlParameter("@jobid", jobid) });
            }
            catch (Exception ex)
            {
                Log.SysLog("获取日志失败！", ex);
            }
            return result;
        }

    }
}
