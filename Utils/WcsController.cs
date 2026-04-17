using System;
using System.Data;
using System.Threading.Tasks;
using WCS_Login.Utils;  // ← 添加这一行（第 5 行）

namespace WCS_Login.Utils
{
    /// <summary>
    /// WCS 核心业务控制器
    /// 协调读码器数据接收、数据库查询、PLC 控制
    /// </summary>
    public class WcsController
    {
        private TcpScannerListener _scannerListener;
        private S7PlcHelper _plcHelper;
        private bool _isRunning = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WcsController()
        {
            _scannerListener = new TcpScannerListener(8899);
            _scannerListener.DataReceived += OnScannerDataReceived;
        }


        /// <summary>
        /// 启动 WCS 系统
        /// </summary>
        /// <returns>PLC 是否连接成功</returns>
        public async Task<bool> StartAsync()
        {
            if (_isRunning) return false;

            try
            {
                _isRunning = true;

                // 【新增】记录启动日志
                Console.WriteLine("WCS 系统启动中...");
                Logger.Info("WCS 系统启动中...");
                DbHelper.LogToDatabase(
                    Program.CurrentUserName,
                    "启动",
                    "WCS",
                    "WCS 系统启动中...",
                    "INFO"
                );

                // 1. 连接 PLC
                string plcIp = GetPlcIpFromDb();
                _plcHelper = new S7PlcHelper(plcIp);
                bool plcConnected = _plcHelper.Connect();

                // 【新增】记录 PLC 连接日志
                Logger.Info($"PLC 连接：{(plcConnected ? "成功" : "失败")}，IP：{plcIp}");
                DbHelper.LogToDatabase(
                    Program.CurrentUserName,
                    "连接",
                    "PLC",
                    $"PLC 连接：{(plcConnected ? "成功" : "失败")}，IP：{plcIp}",
                    plcConnected ? "INFO" : "WARN"
                );

                // 2. 启动读码器监听（不阻塞）
                _scannerListener.StartListening();

                Console.WriteLine("WCS 系统已启动");

                // 【新增】记录启动完成日志
                Logger.Info("WCS 系统启动完成");
                DbHelper.LogToDatabase(
                    Program.CurrentUserName,
                    "完成",
                    "WCS",
                    "WCS 系统启动完成",
                    "INFO"
                );

                return plcConnected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WCS 系统启动失败：{ex.Message}");

                // 【新增】记录启动失败日志
                Logger.Error($"WCS 系统启动失败：{ex.Message}");
                DbHelper.LogToDatabase(
                    Program.CurrentUserName,
                    "错误",
                    "WCS",
                    $"WCS 系统启动失败：{ex.Message}",
                    "ERROR"
                );

                _isRunning = false;
                throw;
            }
        }

        /// <summary>
        /// 停止 WCS 系统
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            _scannerListener.StopListening();
            _plcHelper?.Disconnect();
            Console.WriteLine("WCS 系统已停止");

            // 【新增】记录停止日志
            Logger.Info("WCS 系统已停止");
            DbHelper.LogToDatabase(
                Program.CurrentUserName,
                "停止",
                "WCS",
                "WCS 系统已停止",
                "INFO"
            );
        }

        /// <summary>
        /// 处理读码器数据
        /// </summary>
        private async void OnScannerDataReceived(object sender, ScannerDataEventArgs e)
        {
            try
            {
                Console.WriteLine($"收到箱号：{e.BoxNo}");

                // 记录开始处理日志
                Logger.Info($"开始处理箱号：{e.BoxNo}");
                DbHelper.LogToDatabase(
                    Program.CurrentUserName,
                    "处理",
                    "WCS",
                    $"开始处理箱号：{e.BoxNo}",
                    "INFO"
                );

                // 1. 查询数据库获取任务规则
                string taskRule = GetTaskRuleFromDb(e.BoxNo);

                if (string.IsNullOrEmpty(taskRule))
                {
                    Console.WriteLine($"未找到箱号 {e.BoxNo} 的任务规则");

                    // 记录警告日志（Warning 改为 Warn）
                    Logger.Warn($"未找到箱号 {e.BoxNo} 的任务规则");
                    DbHelper.LogToDatabase(
                        Program.CurrentUserName,
                        "警告",
                        "WCS",
                        $"未找到箱号 {e.BoxNo} 的任务规则",
                        "WARN"
                    );
                    return;
                }

                Console.WriteLine($"任务规则：{taskRule}");

                // 记录任务匹配日志
                Logger.Info($"箱号 {e.BoxNo} 匹配任务规则：{taskRule}");
                DbHelper.LogToDatabase(
                    Program.CurrentUserName,
                    "匹配",
                    "WCS",
                    $"箱号 {e.BoxNo} 匹配任务规则：{taskRule}",
                    "INFO"
                );

                // 2. 解析任务规则（如：DB31.0=1）
                if (taskRule.Contains("DB31.0="))
                {
                    int value = int.Parse(taskRule.Split('=')[1]);

                    // 3. 控制 PLC
                    bool result = _plcHelper.WriteDbBool(31, 0, value > 0);

                    Console.WriteLine($"PLC 写入结果：{result}");

                    // 记录 PLC 控制日志
                    Logger.Info($"PLC 控制：DB31.0={value}，结果：{(result ? "成功" : "失败")}");
                    DbHelper.LogToDatabase(
                        Program.CurrentUserName,
                        "控制",
                        "PLC",
                        $"写入 DB31.0={value}，结果：{(result ? "成功" : "失败")}",
                        result ? "INFO" : "WARN"
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理读码器数据失败：{ex.Message}");

                // 记录错误日志
                Logger.Error($"处理读码器数据失败：{ex.Message}");
                DbHelper.LogToDatabase(
                    Program.CurrentUserName,
                    "错误",
                    "WCS",
                    $"处理读码器数据失败：{ex.Message}",
                    "ERROR"
                );
            }
        }

        /// <summary>
        /// 从数据库获取 PLC IP 地址
        /// </summary>
        private string GetPlcIpFromDb()
        {
            try
            {
                string sql = "SELECT TOP 1 IP FROM T_PLC_IP_Config WHERE PlcNo='PLC01'";
                DataTable dt = DbHelper.ExecuteQuery(sql);

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["IP"].ToString();
                }

                return "192.168.1.10"; // 默认 IP
            }
            catch
            {
                return "192.168.1.10";
            }
        }

        /// <summary>
        /// 从数据库获取任务规则
        /// </summary>
        private string GetTaskRuleFromDb(string boxNo)
        {
            try
            {
                string sql = $"SELECT TaskRule FROM T_Box_Task WHERE BoxNo='{boxNo}'";
                DataTable dt = DbHelper.ExecuteQuery(sql);

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["TaskRule"].ToString();
                }

                return "";
            }
            catch
            {
                return "";
            }
        }
    }
}