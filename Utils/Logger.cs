using System;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;

namespace WCS_Login.Utils
{
    /// <summary>
    /// 异步文本日志工具类（无阻塞，不影响业务耗时）
    /// 日志文件位置：D:\\\\VS\\\\Data\\\\WCS_Login_Logger
    /// </summary>
    public static class Logger
    {
        // 日志文件夹路径
        private static string logPath = "D:\\\\VS\\\\Data\\\\WCS_Login_Logger";
        // 异步日志队列
        private static readonly ConcurrentQueue<string> _logQueue = new ConcurrentQueue<string>();
        // 日志写入线程
        private static readonly Thread _writeThread;

        static Logger()
        {
            // 确保日志目录存在
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            // 启动后台写入线程
            _writeThread = new Thread(WriteLogLoop);
            _writeThread.IsBackground = true;
            _writeThread.Start();
        }

        /// <summary>
        /// 日志写入循环（后台线程执行，不阻塞业务）
        /// </summary>
        private static void WriteLogLoop()
        {
            while (true)
            {
                if (_logQueue.TryDequeue(out string log))
                {
                    try
                    {
                        // 按日期生成文件名：2026-04-17.log
                        string filename = $"{DateTime.Now:yyyy-MM-dd}.log";
                        string filepath = Path.Combine(logPath, filename);
                        // 追加写入文件
                        File.AppendAllText(filepath, log + Environment.NewLine);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[LOG ERROR] 写入日志失败：{ex.Message}");
                    }
                }
                else
                {
                    // 队列空时休眠10ms，降低CPU占用
                    Thread.Sleep(10);
                }
            }
        }

        /// <summary>
        /// 写入日志（直接入队，无阻塞）
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="level">日志级别：INFO/WARN/ERROR</param>
        /// <param name="userName">用户名（可选）</param>
        public static void Write(string message, string level = "INFO", string userName = "")
        {
            // 格式化日志内容（在业务线程执行，只占<1ms）
            string logTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string log = $"[{logTime}] [{level,-5}]";

            if (!string.IsNullOrEmpty(userName))
            {
                log += $" [{userName}]";
            }

            log += $" {message}";

            // 直接入队，立刻返回，不阻塞业务
            _logQueue.Enqueue(log);
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