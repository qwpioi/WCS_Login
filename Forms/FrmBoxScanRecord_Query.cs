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
        private System.Windows.Forms.Timer _autoRefreshTimer;
        private int _lastRecordCount = 0;

        // 分页变量
        private int _currentPage = 1;
        private const int PageSize = 23;
        private int _totalRecords = 0;
        private int _totalPages = 0;

        public FrmBoxScanRecord_Query()
        {
            InitializeComponent();
            this.Text = "周转箱扫描记录查询";

            InitAutoRefreshTimer();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void FrmBoxScanRecord_Query_Load(object sender, EventArgs e)
        {
            try
            {
                LoadData();
                // ✅ 添加：设置时间列显示格式（精确到时分秒）
                var scanTimeColumn = gridView1.Columns["ScanTime"];
                if (scanTimeColumn != null)
                {
                    scanTimeColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    scanTimeColumn.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                }


                if (gridControl1.DataSource is DataTable dt)
                {
                    _lastRecordCount = dt.Rows.Count;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"数据库连接失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 行号列数据
        /// </summary>
        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "RowNo" && e.IsGetData)
            {
                e.Value = (e.ListSourceRowIndex + 1).ToString();
            }
        }

        /// <summary>
        /// 加载数据（分页查询）
        /// </summary>
        private void LoadData()
        {
            try
            {
                // 查询总记录数
                string countSql = "SELECT COUNT(*) FROM T_BoxScanRecord";
                DataTable dtCount = DbHelper.ExecuteQuery(countSql);
                _totalRecords = dtCount.Rows.Count > 0 ? Convert.ToInt32(dtCount.Rows[0][0]) : 0;
                _totalPages = (_totalRecords + PageSize - 1) / PageSize;

                if (_currentPage > _totalPages) _currentPage = Math.Max(1, _totalPages);

                // ROW_NUMBER() 分页查询，按扫描时间倒序
                string sql = @"
                    SELECT Id, BoxNo, ScannerName, ScanTime, ScanResult, StationName, Remark
                    FROM (
                        SELECT Id, BoxNo, ScannerName, ScanTime, ScanResult, StationName, Remark,
                               ROW_NUMBER() OVER (ORDER BY ScanTime DESC) AS RowNum
                        FROM T_BoxScanRecord
                    ) AS t
                    WHERE RowNum BETWEEN @StartRow AND @EndRow
                    ORDER BY ScanTime DESC";

                SqlParameter[] parameters = {
                    new SqlParameter("@StartRow", SqlDbType.Int) { Value = (_currentPage - 1) * PageSize + 1 },
                    new SqlParameter("@EndRow", SqlDbType.Int) { Value = _currentPage * PageSize }
                };

                gridControl1.DataSource = DbHelper.ExecuteQuery(sql, parameters);

                UpdatePageInfo();

                if (gridControl1.DataSource is DataTable dt)
                {
                    _lastRecordCount = dt.Rows.Count;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"加载分页数据失败：{ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 更新分页信息显示到标题栏
        /// </summary>
        private void UpdatePageInfo()
        {
            this.Text = $"周转箱扫描记录查询 — 第 {_currentPage}/{_totalPages} 页，共 {_totalRecords} 条记录";
            txtPageNo.EditValue = _currentPage.ToString();
        }

        /// <summary>
        /// 上一页
        /// </summary>
        private void PrevPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadData();
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        private void NextPage()
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                LoadData();
            }
        }

        /// <summary>
        /// 初始化自动刷新定时器
        /// </summary>
        private void InitAutoRefreshTimer()
        {
            _autoRefreshTimer = new System.Windows.Forms.Timer();
            _autoRefreshTimer.Interval = 2000;
            _autoRefreshTimer.Tick += AutoRefreshTimer_Tick;
            _autoRefreshTimer.Start();
        }

        /// <summary>
        /// 定时器事件 - 自动检测新数据
        /// </summary>
        private void AutoRefreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                string countSql = "SELECT COUNT(*) FROM T_BoxScanRecord";
                DataTable dt = DbHelper.ExecuteQuery(countSql);
                int currentCount = dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][0]) : 0;

                if (currentCount != _lastRecordCount)
                {
                    LoadData();
                    _lastRecordCount = currentCount;
                    this.Text = $"周转箱扫描记录查询 (已更新：{DateTime.Now:HH:mm:ss})";
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"自动刷新失败：{ex.Message}");
            }
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
                    UpdateRowsAffected(rows, true);
                    DbHelper.LogToDatabase(Program.CurrentUserName, "保存数据", "扫描记录", $"修改扫描记录 {id}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 保存周转箱扫描记录，记录 ID：{id}");

                    XtraMessageBox.Show("保存成功！", "提示");
                    LoadData();
                }
                else
                {
                    UpdateRowsAffected(0, false);
                }
            }
            catch (Exception ex)
            {
                UpdateRowsAffected(0, false);
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
                int[] selectedRows = gridView1.GetSelectedRows();
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    XtraMessageBox.Show("请选择要删除的记录！", "提示");
                    return;
                }

                if (XtraMessageBox.Show($"确定要删除选中的 {selectedRows.Length} 条记录吗？", "确认",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                int totalRowsAffected = 0;
                foreach (int rowHandle in selectedRows)
                {
                    if (!gridView1.IsDataRow(rowHandle)) continue;

                    string id = gridView1.GetRowCellValue(rowHandle, "Id").ToString();

                    string sql = "DELETE FROM T_BoxScanRecord WHERE Id = @Id";
                    int rows = DbHelper.ExecuteNonQuery(sql, new SqlParameter[] {
                        new SqlParameter("@Id", id)
                    });

                    totalRowsAffected += rows;

                    DbHelper.LogToDatabase(Program.CurrentUserName, "删除数据", "扫描记录", $"删除扫描记录 {id}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 删除周转箱扫描记录，记录 ID：{id}");
                }

                if (totalRowsAffected > 0)
                {
                    UpdateRowsAffected(totalRowsAffected, true);
                    XtraMessageBox.Show($"成功删除 {totalRowsAffected} 条记录！", "提示");
                    LoadData();
                }
                else
                {
                    UpdateRowsAffected(0, false);
                }
            }
            catch (Exception ex)
            {
                UpdateRowsAffected(0, false);
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
                _currentPage = 1;
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

        /// <summary>
        /// 窗体关闭时停止定时器，防止内存泄漏
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_autoRefreshTimer != null)
            {
                _autoRefreshTimer.Stop();
                _autoRefreshTimer.Dispose();
            }
            base.OnFormClosing(e);
        }

        /// <summary>
        /// 上一页按钮点击
        /// </summary>
        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadData();
                UpdatePageInfo();
            }
            else
            {
                XtraMessageBox.Show("已经是第一页了！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        /// <summary>
        /// 下一页按钮点击
        /// </summary>
        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                LoadData();
                UpdatePageInfo();
            }
            else
            {
                XtraMessageBox.Show("已经是最后一页了！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
