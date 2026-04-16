using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Windows.Forms;

namespace WCS_Login
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            InitMainForm();
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
                default:
                    XtraMessageBox.Show($"暂未实现：{menuText}");
                    return null;
            }
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }
    }
}