using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WCS_Login
{
    public partial class FrmBase : Form
    {
        public FrmBase()
        {
            InitializeComponent();

            // 在基类中绑定所有按钮事件
            btnQuery.ItemClick += BtnQuery_ItemClick;
            btnExport.ItemClick += BtnExport_ItemClick;
            btnSave.ItemClick += BtnSave_ItemClick;
            btnDelete.ItemClick += BtnDelete_ItemClick;
            btnRefresh.ItemClick += BtnRefresh_ItemClick;
        }

        /// <summary>
        /// 查询按钮点击事件（虚方法，子类可重写）
        /// </summary>
        protected virtual void BtnQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraMessageBox.Show("请在子类中实现查询逻辑", "提示");
        }

        /// <summary>
        /// 导出按钮点击事件（虚方法，子类可重写）
        /// </summary>
        protected virtual void BtnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraMessageBox.Show("请在子类中实现导出逻辑", "提示");
        }

        /// <summary>
        /// 保存按钮点击事件（虚方法，子类可重写）
        /// </summary>
        protected virtual void BtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraMessageBox.Show("请在子类中实现保存逻辑", "提示");
        }

        /// <summary>
        /// 删除按钮点击事件（虚方法，子类可重写）
        /// </summary>
        protected virtual void BtnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraMessageBox.Show("请在子类中实现删除逻辑", "提示");
        }

        /// <summary>
        /// 刷新按钮点击事件（虚方法，子类可重写）
        /// </summary>
        protected virtual void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraMessageBox.Show("请在子类中实现刷新逻辑", "提示");
        }
    }
}
