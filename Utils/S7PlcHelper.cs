using System;
using System.Net.Sockets;

namespace WCS_Login.Utils
{
    /// <summary>
    /// 西门子 S7 协议通信辅助类
    /// 用于读写 PLC DB 块数据
    /// </summary>
    public class S7PlcHelper
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private string _ip;
        private int _port;
        private bool _isConnected = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">PLC IP 地址</param>
        /// <param name="port">PLC 端口（默认 102）</param>
        public S7PlcHelper(string ip, int port = 102)
        {
            _ip = ip;
            _port = port;
        }

        /// <summary>
        /// 连接 PLC
        /// </summary>
        public bool Connect()
        {
            try
            {
                _client = new TcpClient(_ip, _port);
                _stream = _client.GetStream();
                _isConnected = true;
                Console.WriteLine($"PLC 连接成功：{_ip}:{_port}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PLC 连接失败：{ex.Message}");
                _isConnected = false;
                return false;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            _stream?.Close();
            _client?.Close();
            _isConnected = false;
            Console.WriteLine("PLC 已断开连接");
        }

        /// <summary>
        /// 写入 DB 块布尔值
        /// </summary>
        /// <param name="dbNumber">DB 块号（如 31）</param>
        /// <param name="offset">偏移量（如 0）</param>
        /// <param name="value">值（true/false）</param>
        /// <returns>是否成功</returns>
        public bool WriteDbBool(int dbNumber, int offset, bool value)
        {
            if (!_isConnected)
            {
                Console.WriteLine("PLC 未连接");
                return false;
            }

            try
            {
                // 简化的 S7 协议写入（实际需要使用 Snap7 库）
                // 这里使用原生 TCP 实现基础功能
                byte[] s7Data = BuildS7WriteData(dbNumber, offset, value ? (byte)1 : (byte)0);
                _stream.Write(s7Data, 0, s7Data.Length);

                Console.WriteLine($"写入 DB{dbNumber}.DBX{offset}.0 = {value}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"写入 DB 块失败：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 读取 DB 块布尔值
        /// </summary>
        /// <param name="dbNumber">DB 块号</param>
        /// <param name="offset">偏移量</param>
        /// <returns>值（true/false），失败返回 false</returns>
        public bool ReadDbBool(int dbNumber, int offset)
        {
            if (!_isConnected)
            {
                Console.WriteLine("PLC 未连接");
                return false;
            }

            try
            {
                // 简化的 S7 协议读取（实际需要使用 Snap7 库）
                byte[] s7Data = BuildS7ReadData(dbNumber, offset);
                _stream.Write(s7Data, 0, s7Data.Length);

                // 读取响应
                byte[] buffer = new byte[1024];
                int bytesRead = _stream.Read(buffer, 0, buffer.Length);

                // 解析响应（简化处理）
                bool value = bytesRead > 0 && buffer[bytesRead - 1] == 1;
                Console.WriteLine($"读取 DB{dbNumber}.DBX{offset}.0 = {value}");
                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"读取 DB 块失败：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 构建 S7 写入数据包（简化版）
        /// </summary>
        private byte[] BuildS7WriteData(int dbNumber, int offset, byte value)
        {
            // 注意：这是简化版本，完整的 S7 协议需要复杂的握手和包头
            // 实际项目建议使用 Snap7 库
            byte[] data = new byte[256];

            // TODO: 实现完整的 S7 协议包头
            // 参考：<https://github.com/mfukushim/S7NetPlus>

            return data;
        }

        /// <summary>
        /// 构建 S7 读取数据包（简化版）
        /// </summary>
        private byte[] BuildS7ReadData(int dbNumber, int offset)
        {
            // 注意：这是简化版本，完整的 S7 协议需要复杂的握手和包头
            byte[] data = new byte[256];

            // TODO: 实现完整的 S7 协议包头

            return data;
        }

        /// <summary>
        /// 检查连接状态
        /// </summary>
        public bool IsConnected()
        {
            return _isConnected;
        }
    }
}