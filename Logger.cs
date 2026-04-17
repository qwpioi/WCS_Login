using System;
using System.IO;

namespace WCS_Login.Utils
{
    /// <summary>
    /// 文本日志工具类（记录系统运行日志）
    /// 日志文件位置：D:\\WCS_Logs\\
    /// 日志文件命名：按日期，如 2026-04-17.log
    /// </summary>
    public static class Logger
    {
        // 日志文件夹路径
        private static string logPath = "D:\\VS\\Data\\WCS_Login_Logger";

        /// <summary>
        /// 写入日志（自动按日期分文件）
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="level">日志级别：INFO/WARN/ERROR</param>
        /// <param name="userName">用户名（可选）</param>
        public static void Write(string message, string level = "INFO", string userName = "")
        {
            try
            {
                // 确保日志目录存在
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                // 按日期生成文件名：2026-04-17.log
                string filename = $"{DateTime.Now:yyyy-MM-dd}.log";
                string filepath = Path.Combine(logPath, filename);

                // 格式化日志内容
                string logTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string log = $"[{logTime}] [{level,-5}]";

                if (!string.IsNullOrEmpty(userName))
                {
                    log += $" [{userName}]";
                }

                log += $" {message}";

                // 追加写入文件
                File.AppendAllText(filepath, log + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // 日志写入失败时，输出到控制台
                Console.WriteLine($"[LOG ERROR] 写入日志失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 快捷方法：信息日志
        /// </summary>
        public static void Info(string message, string userName = "")
        {
            Write(message, "INFO", userName);
        }

        /// <summary>
        /// 快捷方法：警告日志
        /// </summary>
        public static void Warn(string message, string userName = "")
        {
            Write(message, "WARN", userName);
        }

        /// <summary>
        /// 快捷方法：错误日志
        /// </summary>
        public static void Error(string message, string userName = "")
        {
            Write(message, "ERROR", userName);
        }
    }
}