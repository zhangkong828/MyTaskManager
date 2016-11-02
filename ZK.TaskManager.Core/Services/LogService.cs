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

        public static bool SysAdd(string msg, Exception ex = null)
        {
            try
            {
                var sql = "insert into dbo.syslog(id,time,msg,exception) values(@id,@time,@msg,@exception)";
                SqlParameter[] pms = {
                new SqlParameter("@id",Guid.NewGuid().ToString("n")),
                 new SqlParameter("@time",DateTime.Now),
                  new SqlParameter("@msg",msg),
                   new SqlParameter("@exception",ex==null?"":string.Format("Message：{0},StackTrace：{1}",ex.Message,ex.StackTrace))
            };
                return SqlHelper.ExecuteNonQuery(sql, System.Data.CommandType.Text, pms) > 0;

            }
            catch (Exception e)
            {
                Console.WriteLine("日志记录异常！" + e.Message);
                return false;
            }
        }

        public static bool NodeAdd(string nodeid, string msg, Exception ex = null)
        {
            try
            {
                var sql = "insert into dbo.nodelog(id,time,msg,exception,nodeid) values(@id,@time,@msg,@exception,@nodeid)";
                SqlParameter[] pms = {
                new SqlParameter("@id",Guid.NewGuid().ToString("n")),
                 new SqlParameter("@time",DateTime.Now),
                  new SqlParameter("@msg",msg),
                   new SqlParameter("@exception",ex==null?"":string.Format("Message：{0},StackTrace：{1}",ex.Message,ex.StackTrace)),
                    new SqlParameter("@nodeid",nodeid)
            };
                return SqlHelper.ExecuteNonQuery(sql, System.Data.CommandType.Text, pms) > 0;

            }
            catch (Exception e)
            {
                Console.WriteLine("日志记录异常！" + e.Message);
                return false;
            }
        }

        public static List<LogModel> SysQuery(int pageindex, int pagesize, string order, out int totalcount)
        {
            var result = new List<LogModel>();
            totalcount = 0;
            try
            {
                var sql = "select * from(select Id, Time, Msg, Exception, ROW_NUMBER() over(order by time " + order + ") as ranknum from dbo.syslog with(nolock)) as T1 where T1.ranknum between @pagesize*(@pageindex-1)+1 and @pagesize*@pageindex";
               var sql2 = "select count(1) from dbo.sysLog with(nolock)";
                SqlParameter[] pms = {
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
                            model.Exception = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            result.Add(model);
                        }
                    }
                }
                totalcount = (int)SqlHelper.ExecuteScalar(sql2, System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                Log.SysLog("获取日志失败！", ex);
            }
            return result;
        }

        public static List<LogModel> NodeQuery(string nodeid, int pageindex, int pagesize, string order, out int totalcount)
        {
            var result = new List<LogModel>();
            totalcount = 0;
            try
            {
               var  sql = "select * from(select Id, Time, Msg, Exception, NodeId, ROW_NUMBER() over(order by time " + order + ") as ranknum from dbo.log with(nolock) where nodeid=@nodeid) as T1 where T1.ranknum between @pagesize*(@pageindex-1)+1 and @pagesize*@pageindex";
                var sql2 = "select count(1) from dbo.Log with(nolock) where nodeid=@nodeid";
                SqlParameter[] pms = {
                        new SqlParameter("@nodeid",nodeid),
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
                            model.Exception = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            model.NodeId = reader.GetString(4);
                            result.Add(model);
                        }
                    }
                }
                totalcount = (int)SqlHelper.ExecuteScalar(sql2, System.Data.CommandType.Text, new SqlParameter("@nodeid", nodeid));
            }
            catch (Exception ex)
            {
                Log.SysLog("获取日志失败！", ex);
            }
            return result;
        }

    

    }
}
