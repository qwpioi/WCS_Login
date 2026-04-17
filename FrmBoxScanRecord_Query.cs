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
    public partial class FrmBoxScanRecord_Query : FrmBase
    {
        public FrmBoxScanRecord_Query()
        {
            InitializeComponent();
            this.Text = "周转箱扫描记录查询";
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void FrmBoxScanRecord_Query_Load(object sender, EventArgs e)
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
            string sql = "SELECT Id, BoxNo, ScannerName, ScanTime, ScanResult, StationName, Remark FROM T_BoxScanRecord";
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
                DbHelper.LogToDatabase(Program.CurrentUserName, "查询数据", "扫描记录", "查询周转箱扫描记录列表", "INFO");
                Logger.Info($"用户 {Program.CurrentUserName} 查询周转箱扫描记录");

                XtraMessageBox.Show("查询成功！", "提示");
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "查询数据", "扫描记录", $"查询失败：{ex.Message}", "ERROR");
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
                saveFileDialog.FileName = $"周转箱扫描记录_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    gridControl1.ExportToXlsx(saveFileDialog.FileName);

                    // 记录日志
                    DbHelper.LogToDatabase(Program.CurrentUserName, "导出数据", "扫描记录", $"导出到 {saveFileDialog.FileName}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 导出周转箱扫描记录到 {saveFileDialog.FileName}");

                    XtraMessageBox.Show("导出成功！", "提示");
                }
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "导出数据", "扫描记录", $"导出失败：{ex.Message}", "ERROR");
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

                string id = gridView1.GetFocusedRowCellValue("Id").ToString();
                string boxNo = gridView1.GetFocusedRowCellValue("BoxNo").ToString();
                string scannerName = gridView1.GetFocusedRowCellValue("ScannerName").ToString();
                string scanResult = gridView1.GetFocusedRowCellValue("ScanResult").ToString();
                string stationName = gridView1.GetFocusedRowCellValue("StationName").ToString();
                string remark = gridView1.GetFocusedRowCellValue("Remark").ToString();

                string sql = @"UPDATE T_BoxScanRecord 
                       SET BoxNo = @BoxNo, ScannerName = @ScannerName, ScanResult = @ScanResult, StationName = @StationName, Remark = @Remark 
                       WHERE Id = @Id";

                SqlParameter[] parameters = {
            new SqlParameter("@BoxNo", boxNo),
            new SqlParameter("@ScannerName", scannerName),
            new SqlParameter("@ScanResult", scanResult),
            new SqlParameter("@StationName", stationName),
            new SqlParameter("@Remark", remark),
            new SqlParameter("@Id", id)
        };

                int rows = DbHelper.ExecuteNonQuery(sql, parameters);

                if (rows > 0)
                {
                    DbHelper.LogToDatabase(Program.CurrentUserName, "保存数据", "扫描记录", $"修改扫描记录 {id}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 保存周转箱扫描记录，记录 ID：{id}");

                    XtraMessageBox.Show("保存成功！", "提示");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "保存数据", "扫描记录", $"保存失败：{ex.Message}", "ERROR");
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

                string id = gridView1.GetFocusedRowCellValue("Id").ToString();

                string sql = "DELETE FROM T_BoxScanRecord WHERE Id = @Id";
                int rows = DbHelper.ExecuteNonQuery(sql, new SqlParameter[] {
            new SqlParameter("@Id", id)
        });

                if (rows > 0)
                {
                    DbHelper.LogToDatabase(Program.CurrentUserName, "删除数据", "扫描记录", $"删除扫描记录 {id}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 删除周转箱扫描记录，记录 ID：{id}");

                    XtraMessageBox.Show("删除成功！", "提示");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "删除数据", "扫描记录", $"删除失败：{ex.Message}", "ERROR");
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

                Logger.Info($"用户 {Program.CurrentUserName} 刷新周转箱扫描记录数据");

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
