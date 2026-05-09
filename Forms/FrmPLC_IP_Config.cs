using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using WCS_Login.Utils;

namespace WCS_Login
{
    public partial class FrmPLC_IP_Config : FrmBase
    {
        private DataTable _dataTable;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryPlcType;

        public FrmPLC_IP_Config()
        {
            InitializeComponent();
            this.Text = "PLC_IP 配置";
            InitPlcTypeComboBox();
        }

        /// <summary>
        /// 窗体加载事件 - 从数据库加载数据
        /// </summary>
        private void FrmPLC_IP_Config_Load(object sender, EventArgs e)
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
        /// 初始化 PLC 型号下拉框
        /// </summary>
        private void InitPlcTypeComboBox()
        {
            repositoryPlcType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            repositoryPlcType.Items.AddRange(new object[] { "S7-1200", "S7-1500", "S7-300", "S7-200" });
            repositoryPlcType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            repositoryPlcType.AutoComplete = false;  // 禁用自动完成
            gridView1.Columns["PlcType"].ColumnEdit = repositoryPlcType;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            _dataTable = DbHelper.ExecuteQuery("SELECT PlcNo, IP, Port, PlcType, HeartbeatAddress, Remark FROM T_PLC_IP_Config");
            gridControl1.DataSource = _dataTable;
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
                DbHelper.LogToDatabase(Program.CurrentUserName, "查询数据", "PLC 配置", "查询 PLC IP 配置列表", "INFO");
                Logger.Info($"用户 {Program.CurrentUserName} 查询 PLC IP 配置");

                XtraMessageBox.Show("查询成功！", "提示");
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "查询数据", "PLC 配置", $"查询失败：{ex.Message}", "ERROR");
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
                saveFileDialog.FileName = $"PLC_IP 配置_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    gridControl1.ExportToXlsx(saveFileDialog.FileName);

                    // 记录日志
                    DbHelper.LogToDatabase(Program.CurrentUserName, "导出数据", "PLC 配置", $"导出到 {saveFileDialog.FileName}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 导出 PLC IP 配置到 {saveFileDialog.FileName}");

                    XtraMessageBox.Show("导出成功！", "提示");
                }
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "导出数据", "PLC 配置", $"导出失败：{ex.Message}", "ERROR");
                Logger.Error($"导出失败：{ex.Message}", Program.CurrentUserName);

                XtraMessageBox.Show($"导出失败：{ex.Message}", "错误");
            }
        }

        /// <summary>
        /// 重写保存按钮事件 — 同时支持新增和修改
        /// </summary>
        protected override void BtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gridView1.CloseEditor();
                gridView1.UpdateCurrentRow();

                if (_dataTable == null || _dataTable.Rows.Count == 0)
                {
                    UpdateRowsAffected(0, false);
                    XtraMessageBox.Show("没有要保存的数据！", "提示");
                    return;
                }

                // 获取有变化的行（Added + Modified）
                DataTable changes = _dataTable.GetChanges();
                if (changes == null || changes.Rows.Count == 0)
                {
                    UpdateRowsAffected(0, false);
                    XtraMessageBox.Show("没有需要保存的数据！", "提示");
                    return;
                }

                int insertCount = 0, updateCount = 0;

                foreach (DataRow row in changes.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;

                    string plcNo = row["PlcNo"]?.ToString()?.Trim();
                    string ip = row["IP"]?.ToString()?.Trim();

                    // 跳过空行
                    if (string.IsNullOrEmpty(plcNo) && string.IsNullOrEmpty(ip)) continue;

                    string port = row["Port"]?.ToString()?.Trim();
                    string plcType = row["PlcType"]?.ToString()?.Trim();
                    string heartbeatAddress = row["HeartbeatAddress"]?.ToString()?.Trim();
                    string remark = row["Remark"]?.ToString()?.Trim();

                    if (string.IsNullOrEmpty(ip))
                    {
                        UpdateRowsAffected(0, false);
                        XtraMessageBox.Show($"PLC 编号 {plcNo} 的 IP 地址不能为空！", "提示");
                        return;
                    }

                    if (row.RowState == DataRowState.Added)
                    {
                        string insertSql = @"INSERT INTO T_PLC_IP_Config (PlcNo, IP, Port, PlcType, HeartbeatAddress, Remark)
                                             VALUES (@PlcNo, @IP, @Port, @PlcType, @HeartbeatAddress, @Remark)";

                        SqlParameter[] parameters = {
                            new SqlParameter("@PlcNo", plcNo),
                            new SqlParameter("@IP", ip),
                            new SqlParameter("@Port", string.IsNullOrEmpty(port) ? (object)DBNull.Value : port),
                            new SqlParameter("@PlcType", string.IsNullOrEmpty(plcType) ? (object)DBNull.Value : plcType),
                            new SqlParameter("@HeartbeatAddress", string.IsNullOrEmpty(heartbeatAddress) ? (object)DBNull.Value : heartbeatAddress),
                            new SqlParameter("@Remark", string.IsNullOrEmpty(remark) ? (object)DBNull.Value : remark)
                        };

                        int rows = DbHelper.ExecuteNonQuery(insertSql, parameters);
                        insertCount += rows;
                    }
                    else if (row.RowState == DataRowState.Modified)
                    {
                        string updateSql = @"UPDATE T_PLC_IP_Config
                                             SET IP = @IP, Port = @Port, PlcType = @PlcType,
                                                 HeartbeatAddress = @HeartbeatAddress, Remark = @Remark
                                             WHERE PlcNo = @PlcNo
                                             AND (ISNULL(IP,'') <> ISNULL(@IP,'')
                                                  OR ISNULL(Port,'') <> ISNULL(@Port,'')
                                                  OR ISNULL(PlcType,'') <> ISNULL(@PlcType,'')
                                                  OR ISNULL(HeartbeatAddress,'') <> ISNULL(@HeartbeatAddress,'')
                                                  OR ISNULL(Remark,'') <> ISNULL(@Remark,''))";

                        SqlParameter[] parameters = {
                            new SqlParameter("@IP", ip),
                            new SqlParameter("@Port", string.IsNullOrEmpty(port) ? (object)DBNull.Value : port),
                            new SqlParameter("@PlcType", string.IsNullOrEmpty(plcType) ? (object)DBNull.Value : plcType),
                            new SqlParameter("@HeartbeatAddress", string.IsNullOrEmpty(heartbeatAddress) ? (object)DBNull.Value : heartbeatAddress),
                            new SqlParameter("@Remark", string.IsNullOrEmpty(remark) ? (object)DBNull.Value : remark),
                            new SqlParameter("@PlcNo", plcNo)
                        };

                        int rows = DbHelper.ExecuteNonQuery(updateSql, parameters);
                        updateCount += rows;
                    }
                }

                if (insertCount > 0 || updateCount > 0)
                {
                    int totalRows = insertCount + updateCount;
                    UpdateRowsAffected(totalRows, true);

                    string msg = $"保存成功！";
                    if (insertCount > 0) msg += $"\n新增 {insertCount} 条";
                    if (updateCount > 0) msg += $"\n修改 {updateCount} 条";

                    DbHelper.LogToDatabase(Program.CurrentUserName, "保存数据", "PLC 配置", $"新增 {insertCount} 条，修改 {updateCount} 条", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 保存 PLC IP 配置，新增 {insertCount} 条，修改 {updateCount} 条");
                    LoadData();
                }
                else
                {
                    UpdateRowsAffected(0, false);
                    XtraMessageBox.Show("没有实际需要更新的数据！", "提示");
                }
            }
            catch (Exception ex)
            {
                UpdateRowsAffected(0, false);
                DbHelper.LogToDatabase(Program.CurrentUserName, "保存数据", "PLC 配置", $"保存失败：{ex.Message}", "ERROR");
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

                // 确认删除
                if (XtraMessageBox.Show("确定要删除选中记录吗？", "确认",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                string plcNo = gridView1.GetFocusedRowCellValue("PlcNo").ToString();

                string sql = "DELETE FROM T_PLC_IP_Config WHERE PlcNo = @PlcNo";
                int rows = DbHelper.ExecuteNonQuery(sql, new SqlParameter[] {
            new SqlParameter("@PlcNo", plcNo)
        });

                if (rows > 0)
                {
                    UpdateRowsAffected(rows, true);

                    // 记录日志
                    DbHelper.LogToDatabase(Program.CurrentUserName, "删除数据", "PLC 配置", $"删除 PLC {plcNo}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 删除 PLC IP 配置，PLC 编号：{plcNo}");

                    LoadData();
                }
            }
            catch (Exception ex)
            {
                UpdateRowsAffected(0, false);
                DbHelper.LogToDatabase(Program.CurrentUserName, "删除数据", "PLC 配置", $"删除失败：{ex.Message}", "ERROR");
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

                // 记录日志（可选）
                Logger.Info($"用户 {Program.CurrentUserName} 刷新 PLC IP 配置数据");

                XtraMessageBox.Show("刷新成功！", "提示");
            }
            catch (Exception ex)
            {
                Logger.Error($"刷新失败：{ex.Message}", Program.CurrentUserName);
                XtraMessageBox.Show($"刷新失败：{ex.Message}", "错误");
            }
        }

        /// <summary>
        /// 重写新增按钮事件 — 在表格顶部插入空行
        /// </summary>
        protected override void BtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow newRow = _dataTable.NewRow();
            _dataTable.Rows.InsertAt(newRow, 0);
            gridView1.FocusedRowHandle = 0;
            gridView1.ShowEditor();
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
        /// 重写导入按钮事件
        /// </summary>
        protected override void BtnImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Excel 文件|*.xlsx;*.xls";
                ofd.Title = "选择要导入的 Excel 文件";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                string connStr;
                string ext = System.IO.Path.GetExtension(ofd.FileName).ToLower();
                if (ext == ".xlsx")
                    connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={ofd.FileName};Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\"";
                else
                    connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={ofd.FileName};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";

                DataTable dtExcel = new DataTable();
                using (var conn = new System.Data.OleDb.OleDbConnection(connStr))
                {
                    conn.Open();
                    var schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                    if (schemaTable == null || schemaTable.Rows.Count == 0)
                    {
                        XtraMessageBox.Show("Excel 文件中没有找到工作表！", "提示");
                        return;
                    }
                    string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString();
                    using (var cmd = new System.Data.OleDb.OleDbCommand($"SELECT * FROM [{sheetName}]", conn))
                    using (var adapter = new System.Data.OleDb.OleDbDataAdapter(cmd))
                    {
                        adapter.Fill(dtExcel);
                    }
                }

                if (dtExcel.Rows.Count == 0)
                {
                    XtraMessageBox.Show("Excel 文件中没有数据行！", "提示");
                    return;
                }

                int successCount = 0, failCount = 0;
                string insertSql = @"IF NOT EXISTS (SELECT 1 FROM T_PLC_IP_Config WHERE PlcNo = @PlcNo)
                                     INSERT INTO T_PLC_IP_Config (PlcNo, IP, Port, PlcType, HeartbeatAddress, Remark)
                                     VALUES (@PlcNo, @IP, @Port, @PlcType, @HeartbeatAddress, @Remark)";

                for (int i = 0; i < dtExcel.Rows.Count; i++)
                {
                    try
                    {
                        string plcNo = dtExcel.Rows[i][0]?.ToString()?.Trim();
                        string ip = dtExcel.Rows[i][1]?.ToString()?.Trim();
                        string port = dtExcel.Rows[i][2]?.ToString()?.Trim();
                        string plcType = dtExcel.Rows[i][3]?.ToString()?.Trim();
                        string heartbeatAddress = dtExcel.Rows[i][4]?.ToString()?.Trim();
                        string remark = dtExcel.Rows[i][5]?.ToString()?.Trim();

                        if (string.IsNullOrEmpty(plcNo) || string.IsNullOrEmpty(ip)) continue;

                        SqlParameter[] parameters = {
                            new SqlParameter("@PlcNo", plcNo),
                            new SqlParameter("@IP", ip),
                            new SqlParameter("@Port", string.IsNullOrEmpty(port) ? (object)DBNull.Value : port),
                            new SqlParameter("@PlcType", string.IsNullOrEmpty(plcType) ? (object)DBNull.Value : plcType),
                            new SqlParameter("@HeartbeatAddress", string.IsNullOrEmpty(heartbeatAddress) ? (object)DBNull.Value : heartbeatAddress),
                            new SqlParameter("@Remark", string.IsNullOrEmpty(remark) ? (object)DBNull.Value : remark)
                        };

                        int rows = DbHelper.ExecuteNonQuery(insertSql, parameters);
                        successCount += rows;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"导入第 {i + 2} 行失败：{ex.Message}", Program.CurrentUserName);
                        failCount++;
                    }
                }

                UpdateRowsAffected(successCount, successCount > 0);
                DbHelper.LogToDatabase(Program.CurrentUserName, "导入数据", "PLC 配置", $"从 {ofd.FileName} 导入，成功 {successCount} 条，失败 {failCount} 条", "INFO");
                Logger.Info($"用户 {Program.CurrentUserName} 导入 PLC IP 配置，成功 {successCount} 条，失败 {failCount} 条");
                XtraMessageBox.Show($"导入完成！\n成功：{successCount} 条\n失败：{failCount} 条", "提示");
                LoadData();
            }
            catch (Exception ex)
            {
                UpdateRowsAffected(0, false);
                DbHelper.LogToDatabase(Program.CurrentUserName, "导入数据", "PLC 配置", $"导入失败：{ex.Message}", "ERROR");
                Logger.Error($"导入失败：{ex.Message}", Program.CurrentUserName);
                XtraMessageBox.Show($"导入失败：{ex.Message}", "错误");
            }
        }

    }
}
