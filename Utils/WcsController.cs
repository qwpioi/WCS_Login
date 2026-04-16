using System;
using System.Data;
using System.Threading.Tasks;

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

                // 1. 连接 PLC
                string plcIp = GetPlcIpFromDb();
                _plcHelper = new S7PlcHelper(plcIp);
                bool plcConnected = _plcHelper.Connect();

                // 2. 启动读码器监听（不阻塞）
                _scannerListener.StartListening();

                Console.WriteLine("WCS 系统已启动");

                return plcConnected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WCS 系统启动失败：{ex.Message}");
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
        }

        /// <summary>
        /// 处理读码器数据
        /// </summary>
        private async void OnScannerDataReceived(object sender, ScannerDataEventArgs e)
        {
            try
            {
                Console.WriteLine($"收到箱号：{e.BoxNo}");

                // 1. 查询数据库获取任务规则
                string taskRule = GetTaskRuleFromDb(e.BoxNo);

                if (string.IsNullOrEmpty(taskRule))
                {
                    Console.WriteLine($"未找到箱号 {e.BoxNo} 的任务规则");
                    return;
                }

                Console.WriteLine($"任务规则：{taskRule}");

                // 2. 解析任务规则（如：DB31.0=1）
                if (taskRule.Contains("DB31.0="))
                {
                    int value = int.Parse(taskRule.Split('=')[1]);

                    // 3. 控制 PLC
                    bool result = _plcHelper.WriteDbBool(31, 0, value > 0);

                    Console.WriteLine($"PLC 写入结果：{result}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理读码器数据失败：{ex.Message}");
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