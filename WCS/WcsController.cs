using HslCommunication;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using WCS_Login.Utils;

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
        public static S7PlcHelper PlcHelperInstance { get; private set; }

        // 线程安全的缓存
        private static ConcurrentDictionary<string, string> _boxRuleCache = new ConcurrentDictionary<string, string>();

        // 串行化扫码处理，防止多箱竞态覆盖 PLC 写入
        private static readonly SemaphoreSlim _processLock = new SemaphoreSlim(1, 1);

        // 去重控制
        private static string _lastBoxNo = "";
        private static DateTime _lastScanTime = DateTime.MinValue;
        private static readonly TimeSpan DedupInterval = TimeSpan.FromSeconds(2);

        /// <summary>
        /// 构造函数
        /// </summary>
        public WcsController()
        {
            _scannerListener = new TcpScannerListener(0);
            _scannerListener.DataReceived += OnScannerDataReceived;

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] WCS 控制器初始化完成");
        }

        /// <summary>
        /// 启动 WCS 系统
        /// </summary>
        public async Task<bool> StartAsync()
        {
            if (_isRunning) return false;

            try
            {
                _isRunning = true;

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] WCS 系统启动中...");
                Logger.Info("WCS 系统启动中...");
                DbHelper.LogToDatabase(
                    Program.CurrentUserName,
                    "启动",
                    "WCS",
                    "WCS 系统启动中...",
                    "INFO"
                );

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 正在连接 PLC...");

                _plcHelper = new S7PlcHelper();
                bool plcConnected = _plcHelper.Connect();

                if (plcConnected)
                {
                    PlcHelperInstance = _plcHelper;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] PLC 连接成功");
                }
                else
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] PLC 连接失败");
                }

                Logger.Info($"PLC 连接：{(plcConnected ? "成功" : "失败")}");
                DbHelper.LogToDatabase(
                    Program.CurrentUserName,
                    "连接",
                    "PLC",
                    $"PLC 连接：{(plcConnected ? "成功" : "失败")}",
                    plcConnected ? "INFO" : "WARN"
                );

                _scannerListener.StartListening();
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 读码器监听已启动");

                LoadBoxRuleCache();
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 箱号规则缓存已加载 ({_boxRuleCache.Count}条)");

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ═══════════════════════════════════════════════════");
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] WCS 系统已启动，等待扫码...");
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ═══════════════════════════════════════════════════");

                Logger.Info("WCS 系统启动完成");
                DbHelper.LogToDatabase(
                    Program.CurrentUserName,
                    "完成",
                    "WCS",
                    "WCS 系统启动完成",
                    "INFO"
                );

                _ = Task.Run(async () =>
                {
                    while (_isRunning)
                    {
                        await Task.Delay(TimeSpan.FromMinutes(5));
                        LoadBoxRuleCache();
                        Logger.Info("定时刷新箱号规则缓存完成");
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 箱号规则缓存已刷新 ({_boxRuleCache.Count}条)");
                    }
                });

                return plcConnected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] WCS 系统启动失败：{ex.Message}");

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
        /// 加载所有箱号规则到内存缓存
        /// </summary>
        public static void LoadBoxRuleCache()
        {
            try
            {
                string sql = "SELECT BoxNo, TaskRule FROM T_Box_Task";
                DataTable dt = DbHelper.ExecuteQuery(sql);
                var newCache = new ConcurrentDictionary<string, string>();
                foreach (DataRow row in dt.Rows)
                {
                    newCache[row["BoxNo"].ToString()] = row["TaskRule"].ToString();
                }
                _boxRuleCache = newCache;
                Logger.Info($"加载箱号规则缓存完成，共{_boxRuleCache.Count}条规则");
            }
            catch (Exception ex)
            {
                Logger.Error($"加载箱号规则缓存失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 处理读码器数据
        /// </summary>
        private async void OnScannerDataReceived(object sender, ScannerDataEventArgs e)
        {
            // 去重检查（快速路径，在锁外执行）
            if (e.BoxNo == _lastBoxNo && (DateTime.Now - _lastScanTime) < DedupInterval)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] 去重：箱号 {e.BoxNo} 在 2 秒内重复，跳过处理");
                return;
            }

            _lastBoxNo = e.BoxNo;
            _lastScanTime = DateTime.Now;

            // 获取信号量，确保同一时间只有一个箱子在处理 PLC 写入
            await _processLock.WaitAsync();
            try
            {
                await ProcessScanDataAsync(e);
            }
            finally
            {
                _processLock.Release();
            }
        }

        /// <summary>
        /// 实际处理扫码数据（串行化执行）
        /// </summary>
        private async Task ProcessScanDataAsync(ScannerDataEventArgs e)
        {
            long receiveTick = Environment.TickCount;
            try
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] 收到扫码数据：扫描器={e.ScannerName}, 箱号={e.BoxNo}");

                Logger.Info($"[{DateTime.Now:HH:mm:ss.fff}] 收到读码器上报：{e.BoxNo}");

                // 1. 从缓存解析控制值（极快，无数据库查询）
                int controlValue = GetControlValueFromCache(e.BoxNo);
                long parseTick = Environment.TickCount;

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] 解析完成：耗时={parseTick - receiveTick}ms, 控制值={controlValue}");

                // 2. 等待 PLC 清零（上一个箱子处理完成）
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] 等待 PLC 清零...");
                await WaitUntilPlcClearedAsync(20000);

                // 3. 单次写入 + 读回验证，验证成功则停止
                bool writeOk = false;
                long writeStart = Environment.TickCount;
                while ((Environment.TickCount - writeStart) < 5000)
                {
                    _plcHelper.WriteDbShort(31, 0, (short)controlValue);
                    await Task.Delay(50);

                    var verifyResult = _plcHelper.ReadInt16("DB31.0");
                    if (verifyResult.IsSuccess && verifyResult.Content == controlValue)
                    {
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] PLC 写入验证成功：控制值={controlValue}");
                        Logger.Info($"PLC 写入验证成功：控制值={controlValue}");
                        writeOk = true;
                        break;
                    }

                    await Task.Delay(50);
                }

                if (!writeOk)
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] PLC 写入超时（5秒），控制值={controlValue}");
                    Logger.Warn($"PLC 写入超时（5秒），控制值={controlValue}");
                }

                long writeTick = Environment.TickCount;
                string actionName = GetActionName(controlValue);

                Logger.Info($"写入完成：耗时={writeTick - parseTick}ms，总耗时={writeTick - receiveTick}ms，结果={writeOk}");

                string scannerName = e.ScannerName;

                // 异步插入扫描记录
                _ = Task.Run(() =>
                {
                    try
                    {
                        InsertScanRecord(e.BoxNo, scannerName, controlValue, actionName, writeOk);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"插入扫描记录失败：{ex.Message}");
                    }
                });

                // 异步写数据库日志
                _ = Task.Run(() =>
                {
                    try
                    {
                        DbHelper.LogToDatabase(
                            Program.CurrentUserName,
                            "控制", "PLC",
                            $"写入={controlValue}，动作={actionName}，结果={writeOk}，总耗时={writeTick - receiveTick}ms",
                            writeOk ? "INFO" : "WARN");
                    }
                    catch { }
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"处理异常：{ex.Message}");
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] 异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 从缓存获取控制值，缓存未命中时查数据库（同步，不阻塞异步流）
        /// </summary>
        private int GetControlValueFromCache(string boxNo)
        {
            try
            {
                if (_boxRuleCache.TryGetValue(boxNo, out string rule))
                {
                    return ParseControlValue(rule);
                }

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] 缓存未命中，查询数据库：箱号={boxNo}");

                string sql = "SELECT TaskRule FROM T_Box_Task WHERE BoxNo = @BoxNo";
                SqlParameter[] parameters = { new SqlParameter("@BoxNo", boxNo) };
                DataTable dt = DbHelper.ExecuteQuery(sql, parameters);

                if (dt.Rows.Count > 0)
                {
                    string dbRule = dt.Rows[0]["TaskRule"].ToString();
                    _boxRuleCache[boxNo] = dbRule;
                    return ParseControlValue(dbRule);
                }

                Logger.Warn($"箱号 {boxNo} 不存在，使用默认规则（直行）");
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] 箱号 {boxNo} 不存在，使用默认规则（直行）");

                string defaultRule = "DB31.0=1111";

                string insertSql = @"
                    INSERT INTO T_Box_Task (BoxNo, TaskType, TaskRule, CreateTime, CreateUser, Remark)
                    VALUES (@BoxNo, @TaskType, @TaskRule, @CreateTime, @CreateUser, @Remark)";

                SqlParameter[] insertParams = {
                    new SqlParameter("@BoxNo", boxNo),
                    new SqlParameter("@TaskType", "直行"),
                    new SqlParameter("@TaskRule", defaultRule),
                    new SqlParameter("@CreateTime", DateTime.Now),
                    new SqlParameter("@CreateUser", Program.CurrentUserName ?? "SYSTEM"),
                    new SqlParameter("@Remark", "自动创建 - 默认直行")
                };

                DbHelper.ExecuteNonQuery(insertSql, insertParams);
                _boxRuleCache[boxNo] = defaultRule;
                LoadBoxRuleCache();

                Logger.Info($"自动创建箱号规则成功：{boxNo} → {defaultRule}");
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] 自动创建箱号规则成功：{boxNo} → {defaultRule}");

                return ParseControlValue(defaultRule);
            }
            catch (Exception ex)
            {
                Logger.Error($"获取箱号规则失败：{ex.Message}");
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] 获取箱号规则失败：{ex.Message}");
                return 1111;
            }
        }

        /// <summary>
        /// 插入扫描记录到数据库
        /// </summary>
        private void InsertScanRecord(string boxNo, string scannerName, int controlValue, string actionName, bool plcWriteResult)
        {
            try
            {
                string targetStation = GetTargetStationByControlValue(controlValue);
                string scanId = $"SCAN{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";

                string sql = @"
                    INSERT INTO T_BoxScanRecord
                    (Id, BoxNo, ScannerName, ScanTime, ScanResult, StationName, Remark)
                    VALUES
                    (@Id, @BoxNo, @ScannerName, @ScanTime, @ScanResult, @StationName, @Remark)";

                SqlParameter[] parameters = {
                    new SqlParameter("@Id", scanId),
                    new SqlParameter("@BoxNo", boxNo),
                    new SqlParameter("@ScannerName", scannerName),
                    new SqlParameter("@ScanTime", DateTime.Now),
                    new SqlParameter("@ScanResult", plcWriteResult ? "成功" : "失败"),
                    new SqlParameter("@StationName", targetStation),
                    new SqlParameter("@Remark", $"控制值={controlValue},动作={actionName}")
                };

                DbHelper.ExecuteNonQuery(sql, parameters);
                Logger.Info($"插入扫描记录成功：箱号={boxNo}, 扫描器={scannerName}, 动作={actionName}");
            }
            catch (Exception ex)
            {
                Logger.Error($"插入扫描记录失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 根据控制值获取目标站点名称
        /// </summary>
        private string GetTargetStationByControlValue(int controlValue)
        {
            switch (controlValue)
            {
                case 1111: return "主输送线";
                case 2222: return "移栽输送线";
                default: return "未知站点";
            }
        }

        /// <summary>
        /// 从任务规则中解析控制值
        /// </summary>
        private int ParseControlValue(string taskRule)
        {
            try
            {
                if (taskRule.Contains("="))
                {
                    string valuePart = taskRule.Split('=')[1];
                    return int.Parse(valuePart);
                }

                Logger.Warn($"任务规则格式异常：{taskRule}，使用默认值 1111");
                return 1111;
            }
            catch (Exception ex)
            {
                Logger.Error($"解析控制值失败：{ex.Message}，使用默认值 1111");
                return 1111;
            }
        }

        /// <summary>
        /// 根据控制值获取动作名称
        /// </summary>
        private string GetActionName(int controlValue)
        {
            switch (controlValue)
            {
                case 1111: return "直行";
                case 2222: return "移栽";
                default: return $"未知动作 ({controlValue})";
            }
        }

        /// <summary>
        /// 等待 PLC 地址清零（异步版本，不阻塞线程）
        /// </summary>
        private async Task<bool> WaitUntilPlcClearedAsync(int maxWaitMs = 5000)
        {
            long startTime = Environment.TickCount;
            int checkCount = 0;

            while ((Environment.TickCount - startTime) < maxWaitMs)
            {
                try
                {
                    var readResult = _plcHelper.ReadInt16("DB31.0");

                    if (readResult.IsSuccess && readResult.Content == 0)
                    {
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] PLC 地址已清零（等待了 {Environment.TickCount - startTime}ms，检查{checkCount}次）");
                        return true;
                    }

                    checkCount++;
                }
                catch
                {
                    // 读取失败，继续等待
                }

                await Task.Delay(100);
            }

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] PLC 清零超时（{maxWaitMs}ms），当前值可能不为 0，仍尝试写入");
            return false;
        }
    }
}
