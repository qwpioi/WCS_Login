using DevExpress.XtraEditors;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCS_Login.Utils;

namespace WCS_Login
{
    public partial class FrmPLC_Monitor : FrmBase
    {
        
        private SiemensS7Net _plc;
        private bool _isMonitoring = false;
        private byte[] _lastInput = new byte[20]; // 输入区I0.0~I19.7，覆盖所有可能点位
        private byte[] _lastOutput = new byte[20]; // 输出区Q0.0~Q19.7，监控电机/继电器动作
        private byte[] _lastDb31 = new byte[200]; // DB31块前200字节，覆盖所有数据地址
        

        public FrmPLC_Monitor()
        {
            InitializeComponent();
        }

        
        /// <summary>
        /// 【查询按钮】→ 开始监控PLC
        /// </summary>
        protected override void BtnQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_isMonitoring)
            {
                XtraMessageBox.Show("已经在监控中了，无需重复启动", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 【替换原来的新建连接逻辑：直接复用WcsController已经连好的PLC实例】
            if (WcsController.PlcHelperInstance == null
                || !WcsController.PlcHelperInstance.IsConnected()
                || WcsController.PlcHelperInstance.NativePlcInstance == null)
            {
                Log("❌ PLC未连接，请先启动WCS系统，等待PLC连接成功后再开启监控");
                XtraMessageBox.Show("请先启动WCS系统，等待PLC连接成功后再开启监控", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 直接拿已经连好的原生PLC实例，不用新建连接
            _plc = WcsController.PlcHelperInstance.NativePlcInstance;
            Log("✅ 复用WCS系统PLC连接成功，开始监控数据变化");
            _isMonitoring = true;
            // 原来的监控逻辑不变，直接启动
            _ = MonitorPLCDataLoopAsync();

            
        }

        /// <summary>
        /// 【保存按钮】→ 导出日志到文件
        /// </summary>
        protected override void BtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (memoLog.Text.Length == 0)
            {
                XtraMessageBox.Show("没有日志可以导出", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "文本文件|*.txt|所有文件|*.*";
                sfd.FileName = $"PLC监控日志_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, memoLog.Text);
                    XtraMessageBox.Show($"日志已导出到：{sfd.FileName}", "导出成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 【删除按钮】→ 停止监控
        /// </summary>
        protected override void BtnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            StopMonitor();
            XtraMessageBox.Show("已停止监控，断开PLC连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }




        
        /// <summary>
        /// 连接PLC并启动监控
        /// </summary>
        private async Task ConnectAndStartMonitorAsync()
        {
            try
            {
                OperateResult connect = await Task.Run(() => _plc.ConnectServer());
                if (connect.IsSuccess)
                {
                    Log("✅ PLC连接成功！开始监控数据变化");
                    _isMonitoring = true;
                    // 启动后台监控线程
                    _ = MonitorPLCDataLoopAsync();
                }
                else
                {
                    Log($"❌ 连接失败：{connect.Message}");
                }
            }
            catch (Exception ex)
            {
                Log($"❌ 连接出错：{ex.Message}");
            }
        }

        /// <summary>
        /// 监控循环
        /// </summary>
        private async Task MonitorPLCDataLoopAsync()
        {
            while (_isMonitoring)
            {
                try
                {
                    // 1. 读取输入区（传感器/读码器触发信号）
                    var inputResult = await Task.Run(() => _plc.Read("I0.0", 20));
                    if (inputResult.IsSuccess)
                    {
                        CompareAndLogChange("输入区I", _lastInput, inputResult.Content);
                        _lastInput = inputResult.Content;
                    }

                    // 2. 读取输出区（电机/继电器/移栽机构动作信号）
                    var outputResult = await Task.Run(() => _plc.Read("Q0.0", 20));
                    if (outputResult.IsSuccess)
                    {
                        CompareAndLogChange("输出区Q", _lastOutput, outputResult.Content);
                        _lastOutput = outputResult.Content;
                    }

                    // 3. 读取DB31块（业务数据/控制值/箱号存储区）
                    var dbResult = await Task.Run(() => _plc.Read("DB31.DBB0", 200));
                    if (dbResult.IsSuccess)
                    {
                        CompareAndLogChange("DB31块", _lastDb31, dbResult.Content);
                        _lastDb31 = dbResult.Content;
                    }

                    // 每100ms读一次，性能开销可忽略
                    await Task.Delay(100);
                }
                catch (Exception ex)
                {
                    Log($"❌ 监控出错：{ex.Message}");
                    StopMonitor();
                    break;
                }
            }
        }

        /// <summary>
        /// 对比数据变化并记录日志
        /// </summary>
        private void CompareAndLogChange(string area, byte[] oldData, byte[] newData)
        {
            for (int i = 0; i < Math.Min(oldData.Length, newData.Length); i++)
            {
                if (oldData[i] != newData[i])
                {
                    // 1. 记录位变化（传感器/触发信号）
                    for (int bit = 0; bit < 8; bit++)
                    {
                        bool oldBit = (oldData[i] & (1 << bit)) != 0;
                        bool newBit = (newData[i] & (1 << bit)) != 0;
                        if (oldBit != newBit)
                        {
                            string address;
                            if (area == "输入区I")
                            {
                                address = $"I{i}.{bit}";
                            }
                            else if (area == "输出区Q")
                            {
                                address = $"Q{i}.{bit}";
                            }
                            else if (area == "DB31块")
                            {
                                address = $"DB31.DBX{i}.{bit}";
                            }
                            else
                            {
                                address = $"未知地址{i}.{bit}";
                            }

                            string log = $"[{DateTime.Now:HH:mm:ss.fff}] {address} 状态变化：{oldBit} → {newBit}";
                            if (newBit && !oldBit)
                            {
                                log += " ⚠️ 上升沿触发（有物体经过/设备动作）";
                            }
                            Log(log);
                        }
                    }

                    // 2. 记录int类型变化（控制值/数值数据，2字节）
                    if (i % 2 == 0 && i + 1 < newData.Length)
                    {
                        // 西门子是大端字节序，需要反转数组
                        byte[] oldIntBytes = oldData.Skip(i).Take(2).Reverse().ToArray();
                        byte[] newIntBytes = newData.Skip(i).Take(2).Reverse().ToArray();
                        short oldValue = BitConverter.ToInt16(oldIntBytes, 0);
                        short newValue = BitConverter.ToInt16(newIntBytes, 0);

                        if (oldValue != newValue)
                        {
                            string address = area == "DB31块" ? $"DB31.D{i / 2}" : $"{area}.{i}";
                            Log($"[{DateTime.Now:HH:mm:ss.fff}] {address} (int) 数值变化：{oldValue} → {newValue}");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 停止监控并释放资源
        /// </summary>
        private void StopMonitor()
        {
            _isMonitoring = false;
            //_plc?.ConnectClose();
            //_plc?.Dispose();
        }
        

        
        /// <summary>
        /// 写日志到界面
        /// </summary>
        private void Log(string msg)
        {
            if (IsDisposed) return;

            // 线程安全的UI更新
            Invoke(new Action(() =>
            {
                memoLog.AppendText(msg + Environment.NewLine);

                // 自动滚动到最新行
                if (chkAutoScroll.Checked)
                {
                    memoLog.SelectionStart = memoLog.Text.Length;
                    memoLog.ScrollToCaret();
                }
            }));
        }

        /// <summary>
        /// 窗体关闭时停止监控
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopMonitor();
            base.OnFormClosing(e);
        }
        
    }
}