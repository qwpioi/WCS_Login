using HslCommunication;
using HslCommunication.Profinet.Siemens;
using System;
using System.Data;
using System.Threading;

namespace WCS_Login.Utils
{
    /// <summary>
    /// 西门子 S7 协议通信辅助类（基于 HSLCommunication）
    /// 用于读写 PLC DB 块数据
    /// PLC 配置：192.168.2.9:102
    /// </summary>
    public class S7PlcHelper
    {
        private SiemensS7Net _plc;
        private string _ip;
        private int _port;
        private SiemensPLCS _plcType;
        private bool _isConnected = false;
        private Thread _heartBeatThread; // 心跳线程
        // 【新增：只读属性，返回内部已经连好的原生PLC实例】
        public SiemensS7Net NativePlcInstance => _plc;

        /// <summary>
        /// 构造函数（支持从数据库读取配置，或传入参数）
        /// </summary>
        public S7PlcHelper()
        {
            // 默认值
            _ip = "192.168.2.9";
            _port = 102;
            _plcType = SiemensPLCS.S1200;

            try
            {
                DataTable dt = DbHelper.ExecuteQuery(
                    "SELECT TOP 1 IP, Port, PlcType FROM T_PLC_IP_Config WHERE PlcNo='PLC01'");
                
                if (dt.Rows.Count > 0)
                {
                    _ip = dt.Rows[0]["IP"].ToString();
                    _port = int.Parse(dt.Rows[0]["Port"].ToString());
                    string plcTypeStr = dt.Rows[0]["PlcType"].ToString();
                    
                    // 打印实际值用于调试
                    Console.WriteLine($"[PLC] 数据库读取 PlcType 原始值：'{plcTypeStr}'");
                    
                    // 根据实际值匹配枚举（兼容多种格式）
                    if (plcTypeStr.Contains("1200") || plcTypeStr.Contains("S1200"))
                        _plcType = SiemensPLCS.S1200;
                    else if (plcTypeStr.Contains("1500") || plcTypeStr.Contains("S1500"))
                        _plcType = SiemensPLCS.S1500;
                    else if (plcTypeStr.Contains("300") || plcTypeStr.Contains("S300"))
                        _plcType = SiemensPLCS.S300;
                    else if (plcTypeStr.Contains("200") || plcTypeStr.Contains("S200"))
                        _plcType = SiemensPLCS.S200;
                    
                    Console.WriteLine($"[PLC] 从数据库读取配置：IP={_ip}, Port={_port}, Type={_plcType}");
                }
                else
                {
                    Console.WriteLine("[PLC] 数据库无 PLC01 配置，使用默认值");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PLC] 读取配置失败：{ex.Message}，使用默认值");
            }
        }

        /// <summary>
        /// 连接 PLC
        /// </summary>
        /// <returns>是否连接成功</returns>
        public bool Connect()
        {
            try
            {
                // 创建 S7 客户端
                _plc = new SiemensS7Net(_plcType);

                // 设置 IP 和端口
                _plc.IpAddress = _ip;
                _plc.Port = _port;

                // 连接 PLC
                OperateResult connect = _plc.ConnectServer();

                if (connect.IsSuccess)
                {
                    _isConnected = true;
                    // 启动心跳保活线程，30秒发一次心跳
                    _heartBeatThread = new Thread(HeartBeatLoop);
                    _heartBeatThread.IsBackground = true;
                    _heartBeatThread.Start();
                    Console.WriteLine($"✅ PLC 连接成功：{_ip}:{_port}");
                    Logger.Info($"PLC 连接成功：{_ip}:{_port}");
                    return true;
                }
                else
                {
                    _isConnected = false;
                    Console.WriteLine($"❌ PLC 连接失败：{connect.Message}");
                    Logger.Error($"PLC 连接失败：{connect.Message}");
                    Console.WriteLine("请检查：1.PLC 是否运行 2.IP 是否正确 3.是否允许 PUT/GET");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PLC 连接异常：{ex.Message}");
                Logger.Error($"PLC 连接异常：{ex.Message}");
                _isConnected = false;
                return false;
            }
        }

        /// <summary>
        /// 心跳保活线程，30秒发一次读请求，保持连接不被断开
        /// </summary>
        private void HeartBeatLoop()
        {
            while (_isConnected && _plc != null)
            {
                try
                {
                    // 读DB31的第0个字节做心跳，不管成功失败，只要发了请求就行
                    _plc.ReadByte("DB31.0");
                }
                catch
                {
                    // 心跳失败，自动重连
                    Console.WriteLine("⚠️ PLC心跳失败，自动重连...");
                    Disconnect();
                    Connect();
                }
                // 30秒发一次心跳
                Thread.Sleep(30000);
            }
        }


        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (_plc != null)
                {
                    _plc.ConnectClose();
                    _plc.Dispose();
                    _plc = null;
                    _isConnected = false;
                    Console.WriteLine("PLC 已断开连接");
                    Logger.Info("PLC 已断开连接");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"断开连接异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 写入 DB 块字节值
        /// </summary>
        /// <param name="dbNumber">DB 块号（如 31）</param>
        /// <param name="offset">偏移量（如 0）</param>
        /// <param name="value">值（0-255）</param>
        /// <returns>是否成功</returns>
        public bool WriteDbByte(int dbNumber, int offset, byte value)
        {
            if (!_isConnected || _plc == null)
            {
                Console.WriteLine("❌ PLC 未连接");
                Logger.Warn("PLC 未连接，无法写入 byte 数据");
                return false;
            }

            try
            {
                // 地址格式：DB31.0
                string address = $"DB{dbNumber}.{offset}";
                OperateResult write = _plc.Write(address, value);

                if (write.IsSuccess)
                {
                    Console.WriteLine($"✅ 写入成功 - {address} = {value}");
                    Logger.Info($"PLC 写入 byte 成功：{address} = {value}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"❌ 写入失败 - {write.Message}");
                    Logger.Error($"PLC 写入 byte 失败：{address} = {value}, 错误：{write.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"写入异常：{ex.Message}");
                Logger.Error($"PLC 写入 byte 异常：{ex.Message}");
                return false;
            }
        }
        /// <summary>
        /// 写入DB块16位short值（纯写入，不做读回验证）
        /// </summary>
        public bool WriteDbShort(int dbNumber, int offset, short value)
        {
            if (!_isConnected || _plc == null)
            {
                Console.WriteLine("PLC 未连接");
                return false;
            }

            try
            {
                string address = $"DB{dbNumber}.{offset}";
                OperateResult write = _plc.Write(address, value);

                if (write.IsSuccess)
                {
                    Logger.Info($"PLC 写入成功：{address} = {value}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"PLC 写入失败：{write.Message}");
                    Logger.Error($"PLC 写入失败：{address} = {value}, 错误：{write.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"写入异常：{ex.Message}");
                Logger.Error($"PLC 写入异常：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 写入 DB 块 int 值（用于控制值 1111/2222）
        /// </summary>
        /// <param name="dbNumber">DB 块号（如 31）</param>
        /// <param name="offset">偏移量（如 0）</param>
        /// <param name="value">int 值（如 1111 或 2222）</param>
        /// <returns>是否成功</returns>
        public bool WriteDbInt(int dbNumber, int offset, int value)
        {
            if (!_isConnected || _plc == null)
            {
                Console.WriteLine("❌ PLC 未连接");
                Logger.Warn("PLC 未连接，无法写入 int 数据");
                return false;
            }

            try
            {
                // HSL 地址格式：DB31.D0（D 表示 Int/双字）
                //string address = $"DB{dbNumber}.D{offset}";
                string address = $"DB{dbNumber}.DBD{offset}";
                OperateResult write = _plc.Write(address, value);

                if (write.IsSuccess)
                {
                    Console.WriteLine($"✅ 写入成功 - {address} = {value}");
                    Logger.Info($"PLC 写入 int 成功：{address} = {value}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"❌ 写入失败 - {write.Message}");
                    Logger.Error($"PLC 写入 int 失败：{address} = {value}, 错误：{write.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"写入异常：{ex.Message}");
                Logger.Error($"PLC 写入 int 异常：{ex.Message}");
                return false;
            }
        }

        

        /// <summary>
        /// 读取 DB 块字节值
        /// </summary>
        /// <param name="dbNumber">DB 块号</param>
        /// <param name="offset">偏移量</param>
        /// <returns>值（0-255），失败返回 null</returns>
        public byte? ReadDbByte(int dbNumber, int offset)
        {
            if (!_isConnected || _plc == null)
            {
                Console.WriteLine("❌ PLC 未连接");
                Logger.Warn("PLC 未连接，无法读取 byte 数据");
                return null;
            }

            try
            {
                // 地址格式：DB31.0
                string address = $"DB{dbNumber}.{offset}";
                OperateResult<byte> read = _plc.ReadByte(address);

                if (read.IsSuccess)
                {
                    Console.WriteLine($"✅ 读取成功 - {address} = {read.Content}");
                    Logger.Info($"PLC 读取 byte 成功：{address} = {read.Content}");
                    return read.Content;
                }
                else
                {
                    Console.WriteLine($"❌ 读取失败 - {read.Message}");
                    Logger.Warn($"PLC 读取 byte 失败：{address}, 错误：{read.Message}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"读取异常：{ex.Message}");
                Logger.Error($"PLC 读取 byte 异常：{ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 读取 DB 块 short 值（HSL 地址格式，如 "DB31.0"）
        /// </summary>
        public OperateResult<short> ReadInt16(string address)
        {
            if (!_isConnected || _plc == null)
            {
                return new OperateResult<short> { IsSuccess = false, Message = "PLC 未连接" };
            }

            return _plc.ReadInt16(address);
        }

        /// <summary>
        /// 读取 DB 块 int 值（手动指定 DB 号和偏移量）
        /// </summary>
        /// <param name="dbNumber">DB 块号</param>
        /// <param name="offset">偏移量</param>
        /// <returns>int 值，失败返回 null</returns>
        public int? ReadDbInt(int dbNumber, int offset)
        {
            if (!_isConnected || _plc == null)
            {
                Console.WriteLine("❌ PLC 未连接");
                Logger.Warn("PLC 未连接，无法读取 int 数据");
                return null;
            }

            try
            {
                // HSL 地址格式：DB31.D0（D 表示 Int/双字）
                //string address = $"DB{dbNumber}.D{offset}";
                string address = $"DB{dbNumber}.DBD{offset}";
                // ✅ 修正：使用 ReadInt32
                OperateResult<int> read = _plc.ReadInt32(address);

                if (read.IsSuccess)
                {
                    Console.WriteLine($"✅ 读取成功 - {address} = {read.Content}");
                    return read.Content;
                }
                else
                {
                    Console.WriteLine($"❌ 读取失败 - {read.Message}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"读取异常：{ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 检查连接状态
        /// </summary>
        /// <returns>是否已连接</returns>
        public bool IsConnected()
        {
            return _isConnected;
        }
    }
}