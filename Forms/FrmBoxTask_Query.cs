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
using System.Data.SqlClient;
using WCS_Login.Utils;

namespace WCS_Login
{
    public partial class FrmBoxTask_Query : FrmBase
    {
        private DataTable _dataTable;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryTaskType;

        // 分页变量
        private int _currentPage = 1;
        private const int PageSize = 22;
        private int _totalRecords = 0;
        private int _totalPages = 0;

        public FrmBoxTask_Query()
        {
            InitializeComponent();
            this.Text = "周转箱任务查询";
            InitTaskTypeComboBox();
        }

        /// <summary>
        /// 初始化任务类型下拉框
        /// </summary>
        private void InitTaskTypeComboBox()
        {
            repositoryTaskType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            repositoryTaskType.Items.AddRange(new object[] { "直行", "移栽" });
            repositoryTaskType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            repositoryTaskType.AutoComplete = false;
            gridView1.Columns["TaskType"].ColumnEdit = repositoryTaskType;

            gridView1.Columns["TaskRule"].OptionsColumn.AllowEdit = false;

            gridView1.CellValueChanged += GridView1_CellValueChanged;
        }

        /// <summary>
        /// 任务类型改变时联动设置任务规则
        /// </summary>
        private void GridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "TaskType" && _dataTable != null)
            {
                string taskType = e.Value?.ToString()?.Trim();
                string taskRule = "";

                switch (taskType)
                {
                    case "直行":
                        taskRule = "DB31.0=1111";
                        break;
                    case "移栽":
                        taskRule = "DB31.0=2222";
                        break;
                }

                if (!string.IsNullOrEmpty(taskRule))
                {
                    DataRow row = gridView1.GetDataRow(e.RowHandle);
                    if (row != null)
                    {
                        row["TaskRule"] = taskRule;
                    }
                }
            }
        }

        /// <summary>
        /// 克隆 SqlParameter 数组（每次调用创建新实例，避免跨 SqlCommand 复用报错）
        /// </summary>
        private SqlParameter[] CloneParameters(SqlParameter[] original)
        {
            return original.Select(p => new SqlParameter(p.ParameterName, p.SqlDbType, p.Size)
            {
                Value = p.Value ?? DBNull.Value
            }).ToArray();
        }

        /// <summary>
        /// 加载数据（分页查询，支持筛选）
        /// </summary>
        private void LoadData()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string whereClause = BuildWhereClause(parameters);

                // 查询总记录数
                string countSql = $"SELECT COUNT(*) FROM T_Box_Task {whereClause}";
                DataTable dtCount = DbHelper.ExecuteQuery(countSql, parameters.ToArray());
                _totalRecords = dtCount.Rows.Count > 0 ? Convert.ToInt32(dtCount.Rows[0][0]) : 0;
                _totalPages = (_totalRecords + PageSize - 1) / PageSize;

                if (_currentPage > _totalPages) _currentPage = Math.Max(1, _totalPages);

                // 为分页查询创建新的参数实例（SqlParameter 不能跨 SqlCommand 复用）
                SqlParameter[] pageParams = CloneParameters(parameters.ToArray());
                List<SqlParameter> finalParams = new List<SqlParameter>(pageParams);
                finalParams.Add(new SqlParameter("@StartRow", SqlDbType.Int) { Value = (_currentPage - 1) * PageSize + 1 });
                finalParams.Add(new SqlParameter("@EndRow", SqlDbType.Int) { Value = _currentPage * PageSize });

                // ROW_NUMBER() 分页查询，按创建时间倒序
                string sql = $@"
                    SELECT Id, BoxNo, TaskType, TaskRule, CreateTime, CreateUser, Remark
                    FROM (
                        SELECT Id, BoxNo, TaskType, TaskRule, CreateTime, CreateUser, Remark,
                               ROW_NUMBER() OVER (ORDER BY CreateTime DESC) AS RowNum
                        FROM T_Box_Task
                        {whereClause}
                    ) AS t
                    WHERE RowNum BETWEEN @StartRow AND @EndRow
                    ORDER BY CreateTime DESC";

                _dataTable = DbHelper.ExecuteQuery(sql, finalParams.ToArray());
                gridControl1.DataSource = _dataTable;
                if (_dataTable != null) { _dataTable.AcceptChanges(); }

                UpdatePageInfo();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"数据库连接失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 构建筛选 WHERE 子句和参数（每次调用创建新的参数实例）
        /// </summary>
        private string BuildWhereClause(List<SqlParameter> parameters)
        {
            List<string> whereConditions = new List<string>();

            if (!string.IsNullOrWhiteSpace(txtBoxNo.Text))
            {
                whereConditions.Add("BoxNo LIKE @BoxNo");
                parameters.Add(new SqlParameter("@BoxNo", SqlDbType.NVarChar, 50) { Value = "%" + txtBoxNo.Text.Trim() + "%" });
            }
            if (!string.IsNullOrWhiteSpace(cmbTaskType.Text))
            {
                whereConditions.Add("TaskType = @TaskType");
                parameters.Add(new SqlParameter("@TaskType", SqlDbType.NVarChar, 20) { Value = cmbTaskType.Text.Trim() });
            }
            if (!string.IsNullOrWhiteSpace(txtTaskRule.Text))
            {
                whereConditions.Add("TaskRule LIKE @TaskRule");
                parameters.Add(new SqlParameter("@TaskRule", SqlDbType.NVarChar, 100) { Value = "%" + txtTaskRule.Text.Trim() + "%" });
            }
            if (dateStart.EditValue != null && dateStart.DateTime != DateTime.MinValue)
            {
                whereConditions.Add("CreateTime >= @StartTime");
                parameters.Add(new SqlParameter("@StartTime", SqlDbType.DateTime) { Value = dateStart.DateTime });
            }
            if (dateEnd.EditValue != null && dateEnd.DateTime != DateTime.MinValue)
            {
                whereConditions.Add("CreateTime <= @EndTime");
                parameters.Add(new SqlParameter("@EndTime", SqlDbType.DateTime) { Value = dateEnd.DateTime });
            }

            return whereConditions.Count > 0 ? "WHERE " + string.Join(" AND ", whereConditions) : "";
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void FrmBoxTask_Query_Load(object sender, EventArgs e)
        {
            try
            {
                LoadData();

                // ✅ 添加：设置创建时间列显示格式（精确到时分秒）
                var createTimeColumn = gridView1.Columns["CreateTime"];
                if (createTimeColumn != null)
                {
                    createTimeColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    createTimeColumn.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"数据库连接失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 更新分页信息显示到标题栏
        /// </summary>
        private void UpdatePageInfo()
        {
            this.Text = $"周转箱任务查询 — 第 {_currentPage}/{_totalPages} 页，共 {_totalRecords} 条记录";

            // 更新分页信息标签
            if (labelControlPageInfo != null)
                labelControlPageInfo.Text = $"共 {_totalPages} 页，当前第 {_currentPage} 页";

            // 更新页码输入框
            if (txtPageNo != null)
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
        /// 重写查询按钮事件 - 使用搜索面板的筛选条件进行查询
        /// </summary>
        protected override void BtnQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _currentPage = 1; // 重置到第一页
                LoadData();

                DbHelper.LogToDatabase(Program.CurrentUserName, "筛选查询", "任务配置", "执行筛选查询", "INFO");
                Logger.Info($"用户 {Program.CurrentUserName} 执行筛选查询周转箱任务规则");

                XtraMessageBox.Show("查询成功！", "提示");
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "筛选查询", "任务配置", $"查询失败：{ex.Message}", "ERROR");
                Logger.Error($"筛选查询失败：{ex.Message}", Program.CurrentUserName);
                XtraMessageBox.Show($"查询失败：{ex.Message}", "错误");
            }
        }

        /// <summary>
        /// 重写刷新按钮事件
        /// </summary>
        protected override void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                _currentPage = 1; // 重置到第一页
                LoadData();

                Logger.Info($"用户 {Program.CurrentUserName} 刷新周转箱任务规则数据");

                WcsController.LoadBoxRuleCache();

                XtraMessageBox.Show("刷新成功！", "提示");
            }
            catch (Exception ex)
            {
                Logger.Error($"刷新失败：{ex.Message}", Program.CurrentUserName);
                XtraMessageBox.Show($"刷新失败：{ex.Message}", "错误");
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
                saveFileDialog.FileName = $"周转箱任务规则_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    gridControl1.ExportToXlsx(saveFileDialog.FileName);

                    // 记录日志
                    DbHelper.LogToDatabase(Program.CurrentUserName, "导出数据", "任务配置", $"导出到 {saveFileDialog.FileName}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 导出周转箱任务规则到 {saveFileDialog.FileName}");

                    XtraMessageBox.Show("导出成功！", "提示");
                }
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "导出数据", "任务配置", $"导出失败：{ex.Message}", "ERROR");
                Logger.Error($"导出失败：{ex.Message}", Program.CurrentUserName);

                XtraMessageBox.Show($"导出失败：{ex.Message}", "错误");
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

                    string sql = "DELETE FROM T_Box_Task WHERE Id = @Id";
                    int rows = DbHelper.ExecuteNonQuery(sql, new SqlParameter[] {
                        new SqlParameter("@Id", id)
                    });

                    totalRowsAffected += rows;

                    DbHelper.LogToDatabase(Program.CurrentUserName, "删除数据", "任务配置", $"删除任务规则 {id}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 删除周转箱任务规则，ID：{id}");
                }

                if (totalRowsAffected > 0)
                {
                    UpdateRowsAffected(totalRowsAffected, true);
                    _dataTable.AcceptChanges();
                    WcsController.LoadBoxRuleCache();
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
                DbHelper.LogToDatabase(Program.CurrentUserName, "删除数据", "任务配置", $"删除失败：{ex.Message}", "ERROR");
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
                string insertSql = @"IF NOT EXISTS (SELECT 1 FROM T_Box_Task WHERE BoxNo = @BoxNo AND TaskType = @TaskType)
                                     INSERT INTO T_Box_Task (BoxNo, TaskType, TaskRule, CreateUser, Remark)
                                     VALUES (@BoxNo, @TaskType, @TaskRule, @CreateUser, @Remark)";

                for (int i = 0; i < dtExcel.Rows.Count; i++)
                {
                    try
                    {
                        string boxNo = dtExcel.Rows[i][0]?.ToString()?.Trim();
                        string taskType = dtExcel.Rows[i][1]?.ToString()?.Trim();
                        string taskRule = dtExcel.Rows[i][2]?.ToString()?.Trim();
                        string createUser = dtExcel.Rows[i][3]?.ToString()?.Trim();
                        string remark = dtExcel.Rows[i][4]?.ToString()?.Trim();

                        if (string.IsNullOrEmpty(boxNo) || string.IsNullOrEmpty(taskType)) continue;

                        SqlParameter[] parameters = {
                            new SqlParameter("@BoxNo", boxNo),
                            new SqlParameter("@TaskType", taskType),
                            new SqlParameter("@TaskRule", string.IsNullOrEmpty(taskRule) ? (object)DBNull.Value : taskRule),
                            new SqlParameter("@CreateUser", string.IsNullOrEmpty(createUser) ? (object)DBNull.Value : createUser),
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
                DbHelper.LogToDatabase(Program.CurrentUserName, "导入数据", "任务配置", $"从 {ofd.FileName} 导入，成功 {successCount} 条，失败 {failCount} 条", "INFO");
                Logger.Info($"用户 {Program.CurrentUserName} 导入周转箱任务规则，成功 {successCount} 条，失败 {failCount} 条");
                XtraMessageBox.Show($"导入完成！\n成功：{successCount} 条\n失败：{failCount} 条", "提示");
                LoadData();
            }
            catch (Exception ex)
            {
                UpdateRowsAffected(0, false);
                DbHelper.LogToDatabase(Program.CurrentUserName, "导入数据", "任务配置", $"导入失败：{ex.Message}", "ERROR");
                Logger.Error($"导入失败：{ex.Message}", Program.CurrentUserName);
                XtraMessageBox.Show($"导入失败：{ex.Message}", "错误");
            }
        }



        /// <summary>
        /// 重置筛选条件按钮点击
        /// </summary>
        private void btnFilterReset_Click(object sender, EventArgs e)
        {
            txtBoxNo.Text = "";
            cmbTaskType.SelectedIndex = -1;
            cmbTaskType.Text = "";
            txtTaskRule.Text = "";
            dateStart.EditValue = null;
            dateEnd.EditValue = null;

            _currentPage = 1;
            LoadData();
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
        /// <summary>
        /// 页码跳转
        /// </summary>
        private void txtPageNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (int.TryParse(txtPageNo.Text, out int page) && page >= 1 && page <= _totalPages)
                {
                    _currentPage = page;
                    LoadData();
                    UpdatePageInfo();
                }
                else
                {
                    XtraMessageBox.Show($"请输入 1 到 {_totalPages} 之间的页码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPageNo.EditValue = _currentPage.ToString();
                }
            }
        }
    }
}
