using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WCS_Login
{
    public static class DbHelper
    {
        private static string _connStr;
        private static bool _initialized = false;

        private static void EnsureConnectionStringInitialized()
        {
            if (_initialized) return;

            try
            {
                var connectionSettings = ConfigurationManager.ConnectionStrings["WCS_Connection"];
                if (connectionSettings == null)
                {
                    throw new InvalidOperationException("在 App.config 中未找到名为 'WCS_Connection' 的连接字符串");
                }
                _connStr = connectionSettings.ConnectionString;
                if (string.IsNullOrWhiteSpace(_connStr))
                {
                    throw new InvalidOperationException("连接字符串 'WCS_Connection' 的值为空");
                }
                _initialized = true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"初始化数据库连接字符串失败：{ex.Message}", ex);
            }
        }

        private static string connStr
        {
            get
            {
                EnsureConnectionStringInitialized();
                return _connStr;
            }
        }

        /// <summary>
        /// 执行查询，返回 DataTable
        /// </summary>
        public static DataTable ExecuteQuery(string sql, SqlParameter[] parameters = null)
        {
            try
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
            catch (Exception ex)
            {
                throw new InvalidOperationException($"执行查询失败：{ex.Message}\nSQL：{sql}", ex);
            }
        }

        /// <summary>
        /// 执行增删改，返回影响行数
        /// </summary>
        public static int ExecuteNonQuery(string sql, SqlParameter[] parameters = null)
        {
            try
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
            catch (Exception ex)
            {
                throw new InvalidOperationException($"执行非查询操作失败：{ex.Message}\nSQL：{sql}", ex);
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