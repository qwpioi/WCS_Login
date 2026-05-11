using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraCharts;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;

namespace WCS_Login
{
    public partial class FrmBCRScanRate_Statistics : FrmBase
    {
        private DataTable _dataTable;
        private string _selectedScanner = ""; // 空字符串表示全部扫描器

        public FrmBCRScanRate_Statistics()
        {
            InitializeComponent();
            this.Text = "BCR 扫描率统计";

            // 隐藏不需要的按钮
            if (btnAdd != null) btnAdd.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            if (btnSave != null) btnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            if (btnDelete != null) btnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            if (btnImport != null) btnImport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // btnQuery 保持可见，用于查询全部数据
        }

        private void FrmBCRScanRate_Statistics_Load(object sender, EventArgs e)
        {

            // ✅ 新增：限制图表最大高度为 300 像素

            LoadData();
            // ✅ 改用这个事件：当光标（选中行）变化时触发，比 Click 更稳定
            gridView1.FocusedRowChanged += GridView1_FocusedRowChanged;
        }


        /// <summary>
        /// 点击表格行时，根据扫描器名称筛选图表数据
        /// </summary>
        private void GridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                // 获取当前选中行的扫描器名称
                string scannerName = gridView1.GetFocusedRowCellValue("ScannerName")?.ToString().Trim();

                if (!string.IsNullOrEmpty(scannerName))
                {
                    // 只有当选择的扫描器发生变化时才刷新图表
                    if (_selectedScanner != scannerName)
                    {
                        _selectedScanner = scannerName;

                        // 更新标题
                        this.Text = $"BCR 扫描率统计 - 筛选：{scannerName}";

                        // 重新加载图表
                        LoadChart(_dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("筛选异常：" + ex.Message);
            }
        }


        private void LoadData()
        {
            try
            {
                // 查询最近 7 天的数据
                string sql = @"
                SELECT
                    ScannerName,
                    CONVERT(date, ScanTime) AS ScanDate,
                    COUNT(*) AS TotalCount,
                    SUM(CASE WHEN LTRIM(RTRIM(ScanResult)) = '成功' THEN 1 ELSE 0 END) AS SuccessCount,
                    SUM(CASE WHEN LTRIM(RTRIM(ScanResult)) = '失败' THEN 1 ELSE 0 END) AS FailCount,
                    CAST(SUM(CASE WHEN LTRIM(RTRIM(ScanResult)) = '成功' THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*), 0) AS DECIMAL(5,2)) AS SuccessRate,
                    '' AS Remark
                FROM T_BoxScanRecord
                WHERE ScanTime >= DATEADD(day, -7, GETDATE())
                GROUP BY ScannerName, CONVERT(date, ScanTime)
                ORDER BY ScanDate DESC, ScannerName";

                _dataTable = DbHelper.ExecuteQuery(sql);

                if (_dataTable == null || _dataTable.Rows.Count == 0)
                {
                    XtraMessageBox.Show("未查询到最近 7 天的数据。", "提示");
                    return;
                }

                // 调试：输出查询结果信息
                var distinctScanners = _dataTable.AsEnumerable()
                    .Select(row => row.Field<string>("ScannerName"))
                    .Distinct()
                    .OrderBy(name => name);

                string scannerList = string.Join(", ", distinctScanners);
                int totalRows = _dataTable.Rows.Count;

                // XtraMessageBox.Show($"查询到 {totalRows} 条记录，涉及扫描器：{scannerList}", "调试信息");

                // 绑定表格
                gridControl1.DataSource = _dataTable;

                // 立即清除选择状态，避免自动选择第一行
                gridView1.ClearSelection();
                gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;

                gridView1.RefreshData();
                gridView1.BestFitColumns();

                // 加载图表（默认显示全部扫描器）
                _selectedScanner = "";
                LoadChart(_dataTable, true);  // 强制显示全部数据
                this.Text = "BCR 扫描率统计 - 全部扫描器";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"加载异常：{ex.Message}", "错误");
            }
        }

        private void LoadChart(DataTable dt, bool forceShowAll = false)
        {
            if (dt == null || dt.Rows.Count == 0) return;

            // 1. 清空旧系列
            chartControl1.Series.Clear();

            // 2. 根据筛选条件过滤数据
            // 如果 forceShowAll 为 true，则忽略 _selectedScanner 的值，显示全部数据
            DataTable filteredDt = dt;
            if (!forceShowAll && !string.IsNullOrEmpty(_selectedScanner))
            {
                string filterExp = $"ScannerName = '{_selectedScanner.Replace("'", "''")}'";
                DataRow[] rows = dt.Select(filterExp);

                filteredDt = dt.Clone();
                foreach (DataRow row in rows)
                {
                    filteredDt.ImportRow(row);
                }
            }
            else if (forceShowAll)
            {
                // 当 forceShowAll 为 true 时，需要按日期汇总所有扫描器的数据
                filteredDt = AggregateByDate(dt);
            }

            // 3. 创建系列 1：总扫描数
            Series seriesTotal = new Series("总扫描数", ViewType.Bar);
            // ✅ 修改点 1：改为 DateTime 类型，以便应用日期格式
            seriesTotal.ArgumentScaleType = ScaleType.DateTime;
            seriesTotal.ValueScaleType = ScaleType.Numerical;
            seriesTotal.ArgumentDataMember = "ScanDate";
            seriesTotal.ValueDataMembers.AddRange(new string[] { "TotalCount" });
            seriesTotal.DataSource = filteredDt;

            // 4. 创建系列 2：成功数
            Series seriesSuccess = new Series("成功数", ViewType.Bar);
            seriesSuccess.ArgumentScaleType = ScaleType.DateTime; // ✅ 同样改为 DateTime
            seriesSuccess.ValueScaleType = ScaleType.Numerical;
            seriesSuccess.ArgumentDataMember = "ScanDate";
            seriesSuccess.ValueDataMembers.AddRange(new string[] { "SuccessCount" });
            seriesSuccess.DataSource = filteredDt;

            // 5. 添加到图表
            chartControl1.Series.Add(seriesTotal);
            chartControl1.Series.Add(seriesSuccess);

            // 6. 配置坐标轴
            XYDiagram diagram = chartControl1.Diagram as XYDiagram;
            if (diagram != null)
            {
                // ✅ 修改点 2：设置日期格式为 月/日 (例如 5/9)
                //diagram.AxisX.DateTimeOptions.Format = DateTimeFormat.Custom;
                //diagram.AxisX.DateTimeOptions.FormatString = "M/d";

                // ✅ 修改点 3：取消倾斜，让标签水平居中显示
                diagram.AxisX.Label.Angle = 0;
                diagram.AxisX.Label.Staggered = false;
                //diagram.AxisX.Label.TextAntialiasing = true;

                diagram.AxisX.Title.Visible = true;
                diagram.AxisX.Title.Text = "日期";
                diagram.AxisY.Title.Visible = true;
                diagram.AxisY.Title.Text = "扫描次数";
            }

            // 7. 刷新
            chartControl1.RefreshData();
            chartControl1.Update();
        }

        /// <summary>
        /// 按日期汇总所有扫描器的数据
        /// </summary>
        private DataTable AggregateByDate(DataTable originalDt)
        {
            // 创建新的汇总表结构
            DataTable aggregatedDt = new DataTable();
            aggregatedDt.Columns.Add("ScanDate", typeof(DateTime));
            aggregatedDt.Columns.Add("TotalCount", typeof(int));
            aggregatedDt.Columns.Add("SuccessCount", typeof(int));
            aggregatedDt.Columns.Add("FailCount", typeof(int));
            aggregatedDt.Columns.Add("SuccessRate", typeof(decimal));
            aggregatedDt.Columns.Add("Remark", typeof(string));

            // 按日期分组并汇总
            var groupedByDate = originalDt.AsEnumerable()
                .GroupBy(row => DateTime.Parse(row["ScanDate"].ToString()))
                .Select(group => new
                {
                    ScanDate = group.Key,
                    TotalCount = group.Sum(row => Convert.ToInt32(row["TotalCount"])),
                    SuccessCount = group.Sum(row => Convert.ToInt32(row["SuccessCount"])),
                    FailCount = group.Sum(row => Convert.ToInt32(row["FailCount"]))
                })
                .OrderBy(x => x.ScanDate);

            // 添加汇总后的数据行
            foreach (var group in groupedByDate)
            {
                decimal successRate = group.TotalCount > 0 ?
                    Math.Round((decimal)group.SuccessCount * 100 / group.TotalCount, 2) : 0;

                DataRow newRow = aggregatedDt.NewRow();
                newRow["ScanDate"] = group.ScanDate;
                newRow["TotalCount"] = group.TotalCount;
                newRow["SuccessCount"] = group.SuccessCount;
                newRow["FailCount"] = group.FailCount;
                newRow["SuccessRate"] = successRate;
                newRow["Remark"] = "";
                aggregatedDt.Rows.Add(newRow);
            }

            return aggregatedDt;
        }

        protected override void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _selectedScanner = "";
            LoadData();

            // 确保在刷新后也取消选择状态
            gridView1.ClearSelection();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;
        }

        protected override void BtnQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // 点击查询按钮时，强制显示全部扫描器的数据
            _selectedScanner = "";
            LoadData();

            // 确保在查询后也取消选择状态
            gridView1.ClearSelection();
            gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;
        }

        protected override void BtnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel 文件|*.xlsx";
                saveFileDialog.FileName = $"BCR统计_{DateTime.Now:yyyyMMdd}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 创建打印系统
                    var ps = new PrintingSystem();
                    var compositeLink = new CompositeLink(ps);

                    // 添加表格链接（第一页）
                    var gridLink = new PrintableComponentLink
                    {
                        Component = gridControl1
                    };
                    compositeLink.Links.Add(gridLink);

                    // 添加图表链接（第二页）
                    var chartLink = new PrintableComponentLink
                    {
                        Component = chartControl1
                    };
                    compositeLink.Links.Add(chartLink);

                    // 生成文档并导出 Excel
                    compositeLink.CreateDocument();
                    compositeLink.ExportToXlsx(saveFileDialog.FileName);

                    XtraMessageBox.Show("导出成功（含表格和图表）！", "提示");
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"导出失败：{ex.Message}", "错误");
            }
        }
    }
}