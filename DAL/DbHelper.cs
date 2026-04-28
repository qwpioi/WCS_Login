using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WCS_Login
{
    public static class DbHelper
    {
        // 获取连接字符串
        private static string connStr = ConfigurationManager.ConnectionStrings["WCS_Connection"].ConnectionString;

        /// <summary>
        /// 执行查询，返回 DataTable
        /// </summary>
        public static DataTable ExecuteQuery(string sql, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>
        /// 执行增删改，返回影响行数
        /// </summary>
        public static int ExecuteNonQuery(string sql, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 记录系统日志到数据库
        /// </summary>
        /// <param name="userName">操作用户</param>
        /// <param name="operation">操作类型（登录、删除、修改等）</param>
        /// <param name="module">所属模块</param>
        /// <param name="content">详细内容</param>
        /// <param name="logLevel">日志级别：INFO/WARN/ERROR（默认 INFO）</param>
        public static void LogToDatabase(string userName, string operation, string module, string content, string logLevel = "INFO")
        {
            try
            {
                string sql = @"INSERT INTO T_SystemLog 
                               (UserName, Operation, Module, Content, LogLevel) 
                               VALUES 
                               (@UserName, @Operation, @Module, @Content, @LogLevel)";

                SqlParameter[] parameters = {
                    new SqlParameter("@UserName", string.IsNullOrEmpty(userName) ? "System" : userName),
                    new SqlParameter("@Operation", operation),
                    new SqlParameter("@Module", module),
                    new SqlParameter("@Content", content),
                    new SqlParameter("@LogLevel", logLevel)
                };

                ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                // 日志记录失败不影响主流程，输出到控制台
                Console.WriteLine($"[数据库日志失败] {ex.Message}");
            }
        }
    }
}