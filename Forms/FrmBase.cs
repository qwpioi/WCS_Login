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
            btnAdd.ItemClick += BtnAdd_ItemClick;
            btnImport.ItemClick += BtnImport_ItemClick;
        }

        protected virtual void BtnQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraMessageBox.Show("请在子类中实现查询逻辑", "提示");
        }

        protected virtual void BtnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraMessageBox.Show("请在子类中实现导出逻辑", "提示");
        }

        protected virtual void BtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rows = OnSave();
                UpdateRowsAffected(rows, rows > 0);
                if (rows > 0)
                {
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                UpdateRowsAffected(0, false);
                XtraMessageBox.Show($"保存出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected virtual void BtnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rows = OnDelete();
                UpdateRowsAffected(rows, rows > 0);
                if (rows > 0)
                {
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                UpdateRowsAffected(0, false);
                XtraMessageBox.Show($"删除出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected virtual void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraMessageBox.Show("请在子类中实现刷新逻辑", "提示");
        }

        protected virtual void BtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraMessageBox.Show("请在子类中实现新增逻辑", "提示");
        }

        protected virtual void BtnImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraMessageBox.Show("请在子类中实现导入逻辑", "提示");
        }

        protected virtual int OnSave()
        {
            return 0;
        }

        protected virtual int OnDelete()
        {
            return 0;
        }

        protected virtual void LoadData()
        {
        }

        protected void UpdateRowsAffected(int rows, bool isSuccess = true)
        {
            if (this.MdiParent is FrmMain mainForm && mainForm.barStaticItemResult != null)
            {
                if (isSuccess)
                {
                    mainForm.barStaticItemResult.Caption = "✅ 影响行数：" + rows;
                }
                else
                {
                    mainForm.barStaticItemResult.Caption = "❌ 影响行数：0";
                }
            }
        }
    }
}
