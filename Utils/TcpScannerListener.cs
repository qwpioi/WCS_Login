using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WCS_Login.Utils;  // ← 添加这个（如果还没有）

namespace WCS_Login.Utils
{
    /// <summary>
    /// TCP 读码器监听器
    /// 监听 TCP 端口，接收读码器数据
    /// 数据格式：<编号> 箱号<EOF>
    /// </summary>
    public class TcpScannerListener
    {
        private TcpListener _listener;
        private bool _isListening = false;
        private int _port;
        private Task _listenTask;  // ← 添加这个，保存任务引用

        /// <summary>
        /// 数据接收事件
        /// </summary>
        public event EventHandler<ScannerDataEventArgs> DataReceived;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="port">监听端口（默认 8899）</param>
        public TcpScannerListener(int port = 8899)
        {
            _port = port;
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        public void StartListening()
        {
            if (_isListening) return;

            try
            {
                _listener = new TcpListener(IPAddress.Any, _port);
                _listener.Start();
                _isListening = true;

                Console.WriteLine($"TCP 监听器已启动，端口：{_port}");

                // 在后台运行，保存任务引用
                _listenTask = Task.Run(async () =>
                {
                    while (_isListening)
                    {
                        try
                        {
                            TcpClient client = await _listener.AcceptTcpClientAsync();
                            _ = ProcessClientAsync(client);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"接受客户端失败：{ex.Message}");
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TCP 监听器启动失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 处理客户端连接
        /// </summary>
        private async Task ProcessClientAsync(TcpClient client)
        {
            string clientIp = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            int clientPort = ((IPEndPoint)client.Client.RemoteEndPoint).Port;

            try
            {
                // 【新增】记录连接日志
                Console.WriteLine($"读码器已连接：{clientIp}:{clientPort}");
                Logger.Info($"读码器已连接：{clientIp}:{clientPort}");
                DbHelper.LogToDatabase(
                    Program.CurrentUserName,
                    "连接",
                    "读码器",
                    $"读码器已连接：{clientIp}:{clientPort}",
                    "INFO"
                );

                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    string rawData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    string boxNo = ParseBoxNo(rawData);

                    Console.WriteLine($"收到读码器数据：{rawData}");
                    Console.WriteLine($"解析箱号：{boxNo}");

                    // 【新增】记录扫描日志
                    Logger.Info($"读码器扫描：{boxNo} (来源：{clientIp}:{clientPort})");
                    DbHelper.LogToDatabase(
                        Program.CurrentUserName,
                        "扫描",
                        "读码器",
                        $"收到箱号：{boxNo}，来源：{clientIp}:{clientPort}",
                        "INFO"
                    );

                    // 触发事件
                    OnDataReceived(new ScannerDataEventArgs
                    {
                        RawData = rawData,
                        BoxNo = boxNo,
                        ReceiveTime = DateTime.Now
                    });
                }

                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理客户端失败：{ex.Message}");
                // 【新增】记录错误日志
                Logger.Error($"处理客户端失败：{ex.Message}");
                DbHelper.LogToDatabase(
                    Program.CurrentUserName,
                    "错误",
                    "读码器",
                    $"处理客户端失败：{ex.Message}",
                    "ERROR"
                );
            }
            finally
            {
                // 【新增】记录断开日志
                Console.WriteLine($"读码器断开连接：{clientIp}:{clientPort}");
                Logger.Info($"读码器断开连接：{clientIp}:{clientPort}");
                DbHelper.LogToDatabase(
                    Program.CurrentUserName,
                    "断开",
                    "读码器",
                    $"读码器断开连接：{clientIp}:{clientPort}",
                    "INFO"
                );
            }
        }

        /// <summary>
        /// 解析箱号 - 截取 <> 中间的内容
        /// </summary>
        /// <param name="rawData">原始数据：<编号> 箱号<EOF></param>
        /// <returns>箱号</returns>
        private string ParseBoxNo(string rawData)
        {
            try
            {
                // 格式：<编号> 箱号<EOF>
                // 示例：<001>11111<EOF>
                // 目标：提取 11111

                // 1. 找到第一个 '>' 的位置（编号结束）
                int firstEndIndex = rawData.IndexOf('>');
                if (firstEndIndex < 0)
                {
                    Console.WriteLine("格式错误：未找到第一个 '>'");
                    return rawData;
                }

                // 2. 找到 '<EOF>' 的位置
                int eofIndex = rawData.IndexOf("<EOF>");
                if (eofIndex < 0)
                {
                    Console.WriteLine("格式错误：未找到 '<EOF>'");
                    return rawData;
                }

                // 3. 截取箱号（从第一个 '>' 后面到 '<EOF>' 前面）
                string boxNo = rawData.Substring(firstEndIndex + 1, eofIndex - firstEndIndex - 1);

                Console.WriteLine($"解析成功：'{boxNo}'");
                return boxNo.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"解析异常：{ex.Message}");
                return rawData;
            }
        }

        /// <summary>
        /// 触发数据接收事件
        /// </summary>
        protected virtual void OnDataReceived(ScannerDataEventArgs e)
        {
            DataReceived?.Invoke(this, e);
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        public void StopListening()
        {
            _isListening = false;

            // 停止监听器
            try
            {
                _listener?.Stop();
                Console.WriteLine("TCP 监听器已停止");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"停止监听器失败：{ex.Message}");
            }

            // 等待后台线程退出（最多等待 2 秒）
            if (_listenTask != null)
            {
                try
                {
                    _listenTask.Wait(TimeSpan.FromSeconds(2));
                    Console.WriteLine("后台监听线程已退出");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"等待线程退出失败：{ex.Message}");
                }
            }
        }
    }

    /// <summary>
    /// 读码器数据事件参数
    /// </summary>
    public class ScannerDataEventArgs : EventArgs
    {
        /// <summary>
        /// 原始数据
        /// </summary>
        public string RawData { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime ReceiveTime { get; set; }
    }
}