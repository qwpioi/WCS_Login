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
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using WCS_Login.Utils;

namespace WCS_Login
{
    public partial class FrmPLC_WriteAddress_Config : FrmBase
    {
        private DataTable _dataTable;

        public FrmPLC_WriteAddress_Config()
        {
            InitializeComponent();
            this.Text = "PLC 写变量地址配置";
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            string sql = "SELECT Id, VariableName, Address, DataType, PlcNo, Description, Remark FROM T_PLC_WriteAddress_Config";
            _dataTable = DbHelper.ExecuteQuery(sql);
            gridControl1.DataSource = _dataTable;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void FrmPLC_WriteAddress_Config_Load(object sender, EventArgs e)
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
        /// 重写查询按钮事件
        /// </summary>
        protected override void BtnQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadData();

                // 记录日志
                DbHelper.LogToDatabase(Program.CurrentUserName, "查询数据", "PLC 写地址配置", "查询 PLC 写变量地址配置列表", "INFO");
                Logger.Info($"用户 {Program.CurrentUserName} 查询 PLC 写变量地址配置");

                XtraMessageBox.Show("查询成功！", "提示");
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "查询数据", "PLC 写地址配置", $"查询失败：{ex.Message}", "ERROR");
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
                saveFileDialog.FileName = $"PLC 写变量地址配置_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    gridControl1.ExportToXlsx(saveFileDialog.FileName);

                    // 记录日志
                    DbHelper.LogToDatabase(Program.CurrentUserName, "导出数据", "PLC 写地址配置", $"导出到 {saveFileDialog.FileName}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 导出 PLC 写变量地址配置到 {saveFileDialog.FileName}");

                    XtraMessageBox.Show("导出成功！", "提示");
                }
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "导出数据", "PLC 写地址配置", $"导出失败：{ex.Message}", "ERROR");
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
                gridView1.UpdateCurrentRow();

                if (_dataTable == null || _dataTable.Rows.Count == 0)
                {
                    XtraMessageBox.Show("没有要保存的数据！", "提示");
                    return;
                }

                int insertCount = 0, updateCount = 0;

                foreach (DataRow row in _dataTable.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;

                    string variableName = row["VariableName"]?.ToString()?.Trim();
                    string address = row["Address"]?.ToString()?.Trim();

                    if (string.IsNullOrEmpty(variableName) && string.IsNullOrEmpty(address)) continue;

                    string dataType = row["DataType"]?.ToString()?.Trim();
                    string plcNo = row["PlcNo"]?.ToString()?.Trim();
                    string description = row["Description"]?.ToString()?.Trim();
                    string remark = row["Remark"]?.ToString()?.Trim();

                    if (row.RowState == DataRowState.Added)
                    {
                        string insertSql = @"INSERT INTO T_PLC_WriteAddress_Config (VariableName, Address, DataType, PlcNo, Description, Remark)
                                             VALUES (@VariableName, @Address, @DataType, @PlcNo, @Description, @Remark)";

                        SqlParameter[] parameters = {
                            new SqlParameter("@VariableName", variableName),
                            new SqlParameter("@Address", address),
                            new SqlParameter("@DataType", string.IsNullOrEmpty(dataType) ? (object)DBNull.Value : dataType),
                            new SqlParameter("@PlcNo", string.IsNullOrEmpty(plcNo) ? (object)DBNull.Value : plcNo),
                            new SqlParameter("@Description", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description),
                            new SqlParameter("@Remark", string.IsNullOrEmpty(remark) ? (object)DBNull.Value : remark)
                        };

                        DbHelper.ExecuteNonQuery(insertSql, parameters);
                        insertCount++;
                    }
                    else
                    {
                        string id = row["Id"]?.ToString()?.Trim();
                        string updateSql = @"UPDATE T_PLC_WriteAddress_Config
                                             SET VariableName = @VariableName, Address = @Address, DataType = @DataType,
                                                 PlcNo = @PlcNo, Description = @Description, Remark = @Remark
                                             WHERE Id = @Id";

                        SqlParameter[] parameters = {
                            new SqlParameter("@VariableName", variableName),
                            new SqlParameter("@Address", address),
                            new SqlParameter("@DataType", string.IsNullOrEmpty(dataType) ? (object)DBNull.Value : dataType),
                            new SqlParameter("@PlcNo", string.IsNullOrEmpty(plcNo) ? (object)DBNull.Value : plcNo),
                            new SqlParameter("@Description", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description),
                            new SqlParameter("@Remark", string.IsNullOrEmpty(remark) ? (object)DBNull.Value : remark),
                            new SqlParameter("@Id", id)
                        };

                        DbHelper.ExecuteNonQuery(updateSql, parameters);
                        updateCount++;
                    }
                }

                if (insertCount > 0 || updateCount > 0)
                {
                    string msg = $"保存成功！";
                    if (insertCount > 0) msg += $"\n新增 {insertCount} 条";
                    if (updateCount > 0) msg += $"\n修改 {updateCount} 条";

                    DbHelper.LogToDatabase(Program.CurrentUserName, "保存数据", "PLC 写地址配置", $"新增 {insertCount} 条，修改 {updateCount} 条", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 保存 PLC 写变量地址配置，新增 {insertCount} 条，修改 {updateCount} 条");
                    XtraMessageBox.Show(msg, "提示");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "保存数据", "PLC 写地址配置", $"保存失败：{ex.Message}", "ERROR");
                Logger.Error($"保存失败：{ex.Message}", Program.CurrentUserName);
                XtraMessageBox.Show($"保存失败：{ex.Message}", "错误");
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

                string sql = "DELETE FROM T_PLC_WriteAddress_Config WHERE Id = @Id";
                int rows = DbHelper.ExecuteNonQuery(sql, new SqlParameter[] {
            new SqlParameter("@Id", id)
        });

                if (rows > 0)
                {
                    DbHelper.LogToDatabase(Program.CurrentUserName, "删除数据", "PLC 写地址配置", $"删除变量 {id}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 删除 PLC 写变量地址配置，ID：{id}");

                    XtraMessageBox.Show("删除成功！", "提示");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "删除数据", "PLC 写地址配置", $"删除失败：{ex.Message}", "ERROR");
                Logger.Error($"删除失败：{ex.Message}", Program.CurrentUserName);

                XtraMessageBox.Show($"删除失败：{ex.Message}", "错误");
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
                string insertSql = @"IF NOT EXISTS (SELECT 1 FROM T_PLC_WriteAddress_Config WHERE VariableName = @VariableName)
                                     INSERT INTO T_PLC_WriteAddress_Config (VariableName, Address, DataType, PlcNo, Description, Remark)
                                     VALUES (@VariableName, @Address, @DataType, @PlcNo, @Description, @Remark)";

                for (int i = 0; i < dtExcel.Rows.Count; i++)
                {
                    try
                    {
                        string variableName = dtExcel.Rows[i][0]?.ToString()?.Trim();
                        string address = dtExcel.Rows[i][1]?.ToString()?.Trim();
                        string dataType = dtExcel.Rows[i][2]?.ToString()?.Trim();
                        string plcNo = dtExcel.Rows[i][3]?.ToString()?.Trim();
                        string description = dtExcel.Rows[i][4]?.ToString()?.Trim();
                        string remark = dtExcel.Rows[i][5]?.ToString()?.Trim();

                        if (string.IsNullOrEmpty(variableName) || string.IsNullOrEmpty(address)) continue;

                        SqlParameter[] parameters = {
                            new SqlParameter("@VariableName", variableName),
                            new SqlParameter("@Address", address),
                            new SqlParameter("@DataType", string.IsNullOrEmpty(dataType) ? (object)DBNull.Value : dataType),
                            new SqlParameter("@PlcNo", string.IsNullOrEmpty(plcNo) ? (object)DBNull.Value : plcNo),
                            new SqlParameter("@Description", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description),
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

                DbHelper.LogToDatabase(Program.CurrentUserName, "导入数据", "PLC 写地址配置", $"从 {ofd.FileName} 导入，成功 {successCount} 条，失败 {failCount} 条", "INFO");
                Logger.Info($"用户 {Program.CurrentUserName} 导入 PLC 写变量地址配置，成功 {successCount} 条，失败 {failCount} 条");
                XtraMessageBox.Show($"导入完成！\n成功：{successCount} 条\n失败：{failCount} 条", "提示");
                LoadData();
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "导入数据", "PLC 写地址配置", $"导入失败：{ex.Message}", "ERROR");
                Logger.Error($"导入失败：{ex.Message}", Program.CurrentUserName);
                XtraMessageBox.Show($"导入失败：{ex.Message}", "错误");
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

                Logger.Info($"用户 {Program.CurrentUserName} 刷新 PLC 写变量地址配置数据");

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
