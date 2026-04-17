using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using WCS_Login.Utils;

namespace WCS_Login
{
    public partial class FrmEthernetScanner_Config : FrmBase
    {
        public FrmEthernetScanner_Config()
        {
            InitializeComponent();
            this.Text = "以太网扫描器配置";
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void FrmEthernetScanner_Config_Load(object sender, EventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"数据库连接失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            string sql = "SELECT ScannerNo, IP, Port, ScannerType, Remark FROM T_EthernetScanner_Config";
            gridControl1.DataSource = DbHelper.ExecuteQuery(sql);
        }

        /// <summary>
        /// 重写查询按钮事件
        /// </summary>
        protected override void BtnQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadData();

                // 记录日志
                DbHelper.LogToDatabase(Program.CurrentUserName, "查询数据", "扫描器配置", "查询以太网扫描器配置列表", "INFO");
                Logger.Info($"用户 {Program.CurrentUserName} 查询以太网扫描器配置");

                XtraMessageBox.Show("查询成功！", "提示");
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "查询数据", "扫描器配置", $"查询失败：{ex.Message}", "ERROR");
                Logger.Error($"查询失败：{ex.Message}", Program.CurrentUserName);

                XtraMessageBox.Show($"查询失败：{ex.Message}", "错误");
            }
        }

        /// <summary>
        /// 重写导出按钮事件
        /// </summary>
        protected override void BtnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel 文件|*.xlsx";
                saveFileDialog.Title = "导出数据";
                saveFileDialog.FileName = $"以太网扫描器配置_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    gridControl1.ExportToXlsx(saveFileDialog.FileName);

                    // 记录日志
                    DbHelper.LogToDatabase(Program.CurrentUserName, "导出数据", "扫描器配置", $"导出到 {saveFileDialog.FileName}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 导出以太网扫描器配置到 {saveFileDialog.FileName}");

                    XtraMessageBox.Show("导出成功！", "提示");
                }
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "导出数据", "扫描器配置", $"导出失败：{ex.Message}", "ERROR");
                Logger.Error($"导出失败：{ex.Message}", Program.CurrentUserName);

                XtraMessageBox.Show($"导出失败：{ex.Message}", "错误");
            }
        }

        /// <summary>
        /// 重写保存按钮事件
        /// </summary>
        protected override void BtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gridView1.CloseEditor();

                var row = gridView1.GetFocusedRow();
                if (row == null)
                {
                    XtraMessageBox.Show("请选择要保存的记录！", "提示");
                    return;
                }

                string scannerNo = gridView1.GetFocusedRowCellValue("ScannerNo").ToString();
                string ip = gridView1.GetFocusedRowCellValue("IP").ToString();
                string port = gridView1.GetFocusedRowCellValue("Port").ToString();
                string scannerType = gridView1.GetFocusedRowCellValue("ScannerType").ToString();
                string remark = gridView1.GetFocusedRowCellValue("Remark").ToString();

                string sql = @"UPDATE T_EthernetScanner_Config
                       SET IP = @IP, Port = @Port, ScannerType = @ScannerType, Remark = @Remark
                       WHERE ScannerNo = @ScannerNo";

                SqlParameter[] parameters = {
            new SqlParameter("@IP", ip),
            new SqlParameter("@Port", port),
            new SqlParameter("@ScannerType", scannerType),
            new SqlParameter("@Remark", remark),
            new SqlParameter("@ScannerNo", scannerNo)
        };

                int rows = DbHelper.ExecuteNonQuery(sql, parameters);

                if (rows > 0)
                {
                    DbHelper.LogToDatabase(Program.CurrentUserName, "保存数据", "扫描器配置", $"修改扫描器 {scannerNo}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 保存以太网扫描器配置，扫描器编号：{scannerNo}");

                    XtraMessageBox.Show("保存成功！", "提示");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "保存数据", "扫描器配置", $"保存失败：{ex.Message}", "ERROR");
                Logger.Error($"保存失败：{ex.Message}", Program.CurrentUserName);

                XtraMessageBox.Show($"保存失败：{ex.Message}", "错误");
            }
        }

        /// <summary>
        /// 重写删除按钮事件
        /// </summary>
        protected override void BtnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var row = gridView1.GetFocusedRow();
                if (row == null)
                {
                    XtraMessageBox.Show("请选择要删除的记录！", "提示");
                    return;
                }

                if (XtraMessageBox.Show("确定要删除选中记录吗？", "确认",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                string scannerNo = gridView1.GetFocusedRowCellValue("ScannerNo").ToString();

                string sql = "DELETE FROM T_EthernetScanner_Config WHERE ScannerNo = @ScannerNo";
                int rows = DbHelper.ExecuteNonQuery(sql, new SqlParameter[] {
            new SqlParameter("@ScannerNo", scannerNo)
        });

                if (rows > 0)
                {
                    DbHelper.LogToDatabase(Program.CurrentUserName, "删除数据", "扫描器配置", $"删除扫描器 {scannerNo}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 删除以太网扫描器配置，扫描器编号：{scannerNo}");

                    XtraMessageBox.Show("删除成功！", "提示");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "删除数据", "扫描器配置", $"删除失败：{ex.Message}", "ERROR");
                Logger.Error($"删除失败：{ex.Message}", Program.CurrentUserName);

                XtraMessageBox.Show($"删除失败：{ex.Message}", "错误");
            }
        }

        /// <summary>
        /// 重写刷新按钮事件
        /// </summary>
        protected override void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadData();

                Logger.Info($"用户 {Program.CurrentUserName} 刷新以太网扫描器配置数据");

                XtraMessageBox.Show("刷新成功！", "提示");
            }
            catch (Exception ex)
            {
                Logger.Error($"刷新失败：{ex.Message}", Program.CurrentUserName);
                XtraMessageBox.Show($"刷新失败：{ex.Message}", "错误");
            }
        }

        



    }
}
