using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCS_Login.Utils;

namespace WCS_Login
{
    public partial class FrmMain : Form
    {
        private WcsController _wcsController;  // ← 添加这个
        public FrmMain()
        {
            InitializeComponent();
            InitMainForm();
            // 启动 WCS 系统
            this.Shown += FrmMain_Shown;
        }

        /// <summary>
        /// 初始化主窗体
        /// </summary>
        private void InitMainForm()
        {
            // 设置为 MDI 容器
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            this.Text = "蓓安科仪 WCS";

            // 设置 Ribbon 默认最小化
            ribbonControl1.Minimized = true;

            // 初始化左侧导航
            InitTreeList();

            // 初始化底部状态栏
            InitStatusBar();
        }

        /// <summary>
        /// 初始化左侧导航树
        /// </summary>
        private void InitTreeList()
        {
            
        }

        /// <summary>
        /// 初始化底部状态栏
        /// </summary>
        private void InitStatusBar()
        {
            // 设置状态栏内容
            barLinkUser.Caption = "当前用户：admin";
            barLinkDate.Caption = $"当前日期：{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            barLinkCount.Caption = "总条目：0";
        }

        #region Ribbon 按钮事件

        private void btnExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Application.Exit();
        }

        private void btnAbout_ItemClick(object sender, ItemClickEventArgs e)
        {
            XtraMessageBox.Show("蓓安科仪 WCS 系统\n\n版本：1.0.0\n\n© 2026 蓓安科仪", "关于",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion


        private void treeList1_Click(object sender, EventArgs e)
        {
            // 获取当前选中的节点
            var node = treeList1.FocusedNode;
            if (node != null)
            {
                // 只响应子节点点击（不是根节点）
                if (node.ParentNode != null)
                {
                    string menuText = node.GetDisplayText(0);
                    OpenSubForm(menuText);
                }
            }
        }
        private void OpenSubForm(string menuText)
        {

            // 检查是否已存在该子窗体
            Form existingForm = FindChildForm(menuText);

            if (existingForm != null)
            {
                // 已存在 → 激活并前置
                existingForm.Activate();
                return;
            }

            // 不存在 → 创建新实例
            Form childForm = CreateChildForm(menuText);
            if (childForm != null)
            {
                childForm.MdiParent = this;
                childForm.Show();
            }
        }
        private Form FindChildForm(string menuText)
        {
            foreach (Form form in this.MdiChildren)
            {
                if (form.Text == menuText)
                {
                    return form;
                }
            }
            return null;
        }

        private Form CreateChildForm(string menuText)
        {
            switch (menuText)
            {
                case "PLC_IP 配置":
                    return new FrmPLC_IP_Config();
                case "以太网扫描器配置":  // ← 添加这个 case
                    return new FrmEthernetScanner_Config();
                case "周转箱扫描记录查询":  // ← 添加这个
                    return new FrmBoxScanRecord_Query();
                case "PLC 写变量地址配置":  // ← 添加这个
                    return new FrmPLC_WriteAddress_Config();
                case "周转箱定义任务查询":  // ← 添加这个
                    return new FrmBoxTask_Query();
                case "手动调试":  // ← 添加这个
                    return new FrmManualDebug();
                case "PLC实时监控":
                    return new FrmPLC_Monitor();
                case "站点信息配置":  // ← 添加这个
                    return new FrmStation_Config();
                default:
                    XtraMessageBox.Show($"暂未实现：{menuText}");
                    return null;
            }
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 窗体完全显示后触发
        /// </summary>
        private async void FrmMain_Shown(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("StartWcsSystemAsync 开始执行...");

                _wcsController = new WcsController();
                bool plcConnected = await _wcsController.StartAsync();

                Console.WriteLine($"WCS 控制器启动成功，PLC 连接：{plcConnected}");

                // 更新状态栏
                if (barStaticItem1 != null)
                {
                    barStaticItem1.Caption = "WCS 系统：运行中";
                }

                // 显示 PLC 状态弹窗
                if (plcConnected)
                {
                    XtraMessageBox.Show(this, "WCS 系统已启动！\n\nTCP 读码器监听端口：2112\nPLC 地址：192.168.2.9:102\n通讯库：HSLCommunication", "系统提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show(this, "WCS 系统已启动！\n\nTCP 读码器监听端口：2112\nPLC 地址：192.168.2.9:102\n\n⚠️ PLC 连接失败！\n\n请检查：\n1. PLC 是否已上电\n2. 网络是否连通\n3. IP 地址配置是否正确\n4. PLC 是否允许 PUT/GET 访问", "系统提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"异常：{ex.Message}");

                if (barStaticItem1 != null)
                {
                    barStaticItem1.Caption = "WCS 系统：启动失败";
                }

                XtraMessageBox.Show(this, $"WCS 系统启动失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 启动 WCS 系统
        /// </summary>
        private async Task StartWcsSystemAsync()
        {
            try
            {
                Console.WriteLine("StartWcsSystemAsync 开始执行...");

                _wcsController = new WcsController();
                bool plcConnected = await _wcsController.StartAsync();

                Console.WriteLine($"WCS 控制器启动成功，PLC 连接：{plcConnected}");

                // 更新状态栏
                if (barStaticItem1 != null)
                {
                    barStaticItem1.Caption = "WCS 系统：运行中";
                }

                // 立即显示 PLC 状态弹窗（不等待）
                this.Invoke(new Action(() =>
                {
                    if (plcConnected)
                    {
                        XtraMessageBox.Show(this, "WCS 系统已启动！\n\nTCP 读码器监听端口：8899\nPLC 连接：已建立", "系统提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        XtraMessageBox.Show(this, "WCS 系统已启动！\n\nTCP 读码器监听端口：8899\n\n⚠️ PLC 连接失败！\n\n请检查：\n1. PLC 是否已上电\n2. 网络是否连通\n3. IP 地址配置是否正确", "系统提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"异常：{ex.Message}");

                if (barStaticItem1 != null)
                {
                    barStaticItem1.Caption = "WCS 系统：启动失败";
                }
            }
        }

        /// <summary>
        /// 窗体关闭时停止 WCS 系统
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                Console.WriteLine("主窗体正在关闭...");

                if (_wcsController != null)
                {
                    Console.WriteLine("正在停止 WCS 系统...");
                    _wcsController.Stop();
                    Console.WriteLine("WCS 系统已停止");
                }

                // 强制退出程序
                Application.Exit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"关闭时出错：{ex.Message}");
            }

            base.OnFormClosing(e);
        }

        private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {

        }
    }
}