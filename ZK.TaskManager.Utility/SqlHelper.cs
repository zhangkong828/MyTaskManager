using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK.TaskManager.Utility
{
    public static class SqlHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        static string connstr = ConfigurationManager.ConnectionStrings["mssqlserver"].ConnectionString;
        /// <summary>
        /// 执行非查询语句，对增删改返回受影响行数，否则返回-1
        /// </summary>
        /// <param name="sql">执行的sql语句或存储过程名字</param>
        /// <param name="cmdtype">执行的类型，是sql语句还是存储过程</param>
        /// <param name="pm">参数化的绑定参数</param>
        /// <returns>受影响行数</returns>
        public static int ExecuteNonQuery(string sql, CommandType cmdtype, params SqlParameter[] pm)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = cmdtype;
                    if (pm != null)
                    {
                        cmd.Parameters.AddRange(pm);
                    }
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回一行一列的object数据
        /// </summary>
        /// <param name="sql">执行的sql语句或存储过程名字</param>
        /// <param name="cmdtype">执行的类型，是sql语句还是存储过程</param>
        /// <param name="pm">参数化的绑定参数</param>
        /// <returns>一行一列的结果</returns>
        public static object ExecuteScalar(string sql, CommandType cmdtype, params SqlParameter[] pm)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = cmdtype;
                    if (pm != null)
                    {
                        cmd.Parameters.AddRange(pm);
                    }
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }
        /// <summary>
        /// 执行读取数据语句，返回SqlDataReader类型结果
        /// </summary>
        /// <param name="sql">执行的sql语句或存储过程名字</param>
        /// <param name="cmdtype">执行的类型，是sql语句还是存储过程</param>
        /// <param name="pm">参数化的绑定参数</param>
        /// <returns>SqlDataReader类型结果</returns>
        public static SqlDataReader ExecuteReader(string sql, CommandType cmdtype, params SqlParameter[] pm)
        {
            SqlConnection conn = new SqlConnection(connstr);
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = cmdtype;
                cmd.CommandTimeout = 180;
                if (pm != null)
                {
                    cmd.Parameters.AddRange(pm);
                }
                try
                {
                    conn.Open();
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    throw;
                }
            }

        }

        /// <summary>
        /// 返回一个DataTable
        /// </summary>
        /// <param name="sql">执行的sql语句或存储过程名字</param>
        /// <param name="cmdtype">执行的类型，是sql语句还是存储过程</param>
        /// <param name="pm">参数化的绑定参数</param>
        /// <returns>DataTable</returns>
        public static DataTable DataAdapter(string sql, CommandType cmdtype, params SqlParameter[] pm)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter da = new SqlDataAdapter(sql, connstr))
            {
                da.SelectCommand.CommandType = cmdtype;
                if (pm != null)
                {
                    da.SelectCommand.Parameters.AddRange(pm);
                }
                da.Fill(dt);
            }
            return dt;

        }
    }
}
