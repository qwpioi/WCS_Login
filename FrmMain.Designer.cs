namespace WCS_Login
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barLinkUser = new DevExpress.XtraBars.BarLinkContainerItem();
            this.barLinkDate = new DevExpress.XtraBars.BarLinkContainerItem();
            this.barLinkCount = new DevExpress.XtraBars.BarLinkContainerItem();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.btnAbout = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage_System = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup_Operation = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPage_Help = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup_About = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.colName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.barLinkUser,
            this.barLinkDate,
            this.barLinkCount,
            this.btnExit,
            this.btnAbout,
            this.barStaticItem1});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 7;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage_System,
            this.ribbonPage_Help});
            this.ribbonControl1.Size = new System.Drawing.Size(1178, 225);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
            this.ribbonControl1.Click += new System.EventHandler(this.ribbonControl1_Click);
            // 
            // barLinkUser
            // 
            this.barLinkUser.Caption = "当前用户：";
            this.barLinkUser.Id = 1;
            this.barLinkUser.Name = "barLinkUser";
            // 
            // barLinkDate
            // 
            this.barLinkDate.Caption = "当前日期：";
            this.barLinkDate.Id = 2;
            this.barLinkDate.Name = "barLinkDate";
            // 
            // barLinkCount
            // 
            this.barLinkCount.Caption = "总条目：";
            this.barLinkCount.Id = 3;
            this.barLinkCount.Name = "barLinkCount";
            // 
            // btnExit
            // 
            this.btnExit.Caption = "退出";
            this.btnExit.Id = 4;
            this.btnExit.Name = "btnExit";
            this.btnExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExit_ItemClick);
            // 
            // btnAbout
            // 
            this.btnAbout.Caption = "关于";
            this.btnAbout.Id = 5;
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAbout_ItemClick);
            // 
            // ribbonPage_System
            // 
            this.ribbonPage_System.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup_Operation});
            this.ribbonPage_System.Name = "ribbonPage_System";
            this.ribbonPage_System.Text = "系统";
            // 
            // ribbonPageGroup_Operation
            // 
            this.ribbonPageGroup_Operation.ItemLinks.Add(this.btnExit);
            this.ribbonPageGroup_Operation.Name = "ribbonPageGroup_Operation";
            this.ribbonPageGroup_Operation.Text = "操作";
            // 
            // ribbonPage_Help
            // 
            this.ribbonPage_Help.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup_About});
            this.ribbonPage_Help.Name = "ribbonPage_Help";
            this.ribbonPage_Help.Text = "帮助";
            // 
            // ribbonPageGroup_About
            // 
            this.ribbonPageGroup_About.ItemLinks.Add(this.btnAbout);
            this.ribbonPageGroup_About.Name = "ribbonPageGroup_About";
            this.ribbonPageGroup_About.Text = "关于";
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.ItemLinks.Add(this.barLinkUser);
            this.ribbonStatusBar1.ItemLinks.Add(this.barLinkDate);
            this.ribbonStatusBar1.ItemLinks.Add(this.barLinkCount);
            this.ribbonStatusBar1.ItemLinks.Add(this.barStaticItem1);
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 654);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1178, 40);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl",
            "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl"});
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanel1.ID = new System.Guid("9621b2d4-0c42-4f25-8a09-efd7aeea9058");
            this.dockPanel1.Location = new System.Drawing.Point(0, 225);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.OriginalSize = new System.Drawing.Size(260, 200);
            this.dockPanel1.Size = new System.Drawing.Size(260, 429);
            this.dockPanel1.Text = "dockPanel1";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.treeList1);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 38);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(249, 387);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // treeList1
            // 
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colName});
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.Location = new System.Drawing.Point(0, 0);
            this.treeList1.MenuManager = this.ribbonControl1;
            this.treeList1.Name = "treeList1";
            this.treeList1.BeginUnboundLoad();
            this.treeList1.AppendNode(new object[] {
            "基础信息配置"}, -1);
            this.treeList1.AppendNode(new object[] {
            "PLC_IP 配置"}, 0);
            this.treeList1.AppendNode(new object[] {
            "扫描器路由配置"}, 0);
            this.treeList1.AppendNode(new object[] {
            "以太网扫描器配置"}, 0);
            this.treeList1.AppendNode(new object[] {
            "PLC 写变量地址配置"}, 0);
            this.treeList1.AppendNode(new object[] {
            "特殊解绑扫描器"}, 0);
            this.treeList1.AppendNode(new object[] {
            "固定路径站点信息配置"}, 0);
            this.treeList1.AppendNode(new object[] {
            "站点信息配置"}, 0);
            this.treeList1.AppendNode(new object[] {
            "RFID 读写配置"}, 0);
            this.treeList1.AppendNode(new object[] {
            "手动调试"}, 0);
            this.treeList1.AppendNode(new object[] {
            "数据查询"}, -1);
            this.treeList1.AppendNode(new object[] {
            "周转箱扫描记录查询"}, 10);
            this.treeList1.AppendNode(new object[] {
            "周转箱定义任务查询"}, 10);
            this.treeList1.AppendNode(new object[] {
            "PLC 交互信息查询"}, 10);
            this.treeList1.AppendNode(new object[] {
            "BCR 扫描率统计"}, 10);
            this.treeList1.AppendNode(new object[] {
            "箱子已到达站点查询"}, 10);
            this.treeList1.EndUnboundLoad();
            this.treeList1.OptionsBehavior.Editable = false;
            this.treeList1.Size = new System.Drawing.Size(249, 387);
            this.treeList1.TabIndex = 0;
            this.treeList1.Click += new System.EventHandler(this.treeList1_Click);
            // 
            // colName
            // 
            this.colName.Caption = "名称";
            this.colName.FieldName = "名称";
            this.colName.Name = "colName";
            this.colName.OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.True;
            this.colName.Visible = true;
            this.colName.VisibleIndex = 0;
            // 
            // xtraTabbedMdiManager1
            // 
            this.xtraTabbedMdiManager1.MdiParent = this;
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "WCS 系统：未启动";
            this.barStaticItem1.Id = 6;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.Caption;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 694);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.ribbonControl1);
            this.IsMdiContainer = true;
            this.Name = "FrmMain";
            this.Text = "FrmMain";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage_System;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup_Operation;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.BarLinkContainerItem barLinkUser;
        private DevExpress.XtraBars.BarLinkContainerItem barLinkDate;
        private DevExpress.XtraBars.BarLinkContainerItem barLinkCount;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private DevExpress.XtraBars.BarButtonItem btnAbout;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage_Help;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup_About;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colName;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
    }
}