namespace WCS_Login
{
    partial class FrmBCRScanRate_Statistics
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
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ScannerName1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ScanDate1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TotalCount1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SuccessCount1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FailCount1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SuccessRate1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Remark1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridControl1.Location = new System.Drawing.Point(0, 31);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.MenuManager = this.barManager1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(800, 173);
            this.gridControl1.TabIndex = 4;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.Load += new System.EventHandler(this.FrmBCRScanRate_Statistics_Load);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ScannerName1,
            this.ScanDate1,
            this.TotalCount1,
            this.SuccessCount1,
            this.FailCount1,
            this.SuccessRate1,
            this.Remark1});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // ScannerName1
            // 
            this.ScannerName1.Caption = "扫描器";
            this.ScannerName1.FieldName = "ScannerName";
            this.ScannerName1.MinWidth = 30;
            this.ScannerName1.Name = "ScannerName1";
            this.ScannerName1.Visible = true;
            this.ScannerName1.VisibleIndex = 0;
            this.ScannerName1.Width = 112;
            // 
            // ScanDate1
            // 
            this.ScanDate1.Caption = "统计日期";
            this.ScanDate1.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.ScanDate1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ScanDate1.FieldName = "ScanDate";
            this.ScanDate1.MinWidth = 30;
            this.ScanDate1.Name = "ScanDate1";
            this.ScanDate1.Visible = true;
            this.ScanDate1.VisibleIndex = 1;
            this.ScanDate1.Width = 112;
            // 
            // TotalCount1
            // 
            this.TotalCount1.Caption = "总扫描数";
            this.TotalCount1.FieldName = "TotalCount";
            this.TotalCount1.MinWidth = 30;
            this.TotalCount1.Name = "TotalCount1";
            this.TotalCount1.Visible = true;
            this.TotalCount1.VisibleIndex = 2;
            this.TotalCount1.Width = 112;
            // 
            // SuccessCount1
            // 
            this.SuccessCount1.Caption = "成功数";
            this.SuccessCount1.FieldName = "SuccessCount";
            this.SuccessCount1.MinWidth = 30;
            this.SuccessCount1.Name = "SuccessCount1";
            this.SuccessCount1.Visible = true;
            this.SuccessCount1.VisibleIndex = 3;
            this.SuccessCount1.Width = 112;
            // 
            // FailCount1
            // 
            this.FailCount1.Caption = "失败数";
            this.FailCount1.FieldName = "FailCount";
            this.FailCount1.MinWidth = 30;
            this.FailCount1.Name = "FailCount1";
            this.FailCount1.Visible = true;
            this.FailCount1.VisibleIndex = 4;
            this.FailCount1.Width = 112;
            // 
            // SuccessRate1
            // 
            this.SuccessRate1.Caption = "成功率 (%)";
            this.SuccessRate1.FieldName = "SuccessRate";
            this.SuccessRate1.MinWidth = 30;
            this.SuccessRate1.Name = "SuccessRate1";
            this.SuccessRate1.Visible = true;
            this.SuccessRate1.VisibleIndex = 5;
            this.SuccessRate1.Width = 112;
            // 
            // Remark1
            // 
            this.Remark1.Caption = "备注";
            this.Remark1.FieldName = "Remark";
            this.Remark1.MinWidth = 30;
            this.Remark1.Name = "Remark1";
            this.Remark1.Visible = true;
            this.Remark1.VisibleIndex = 6;
            this.Remark1.Width = 112;
            // 
            // chartControl1
            // 
            xyDiagram1.AxisX.DateTimeScaleOptions.AggregateFunction = DevExpress.XtraCharts.AggregateFunction.Custom;
            xyDiagram1.AxisX.DateTimeScaleOptions.AutoGrid = false;
            xyDiagram1.AxisX.DateTimeScaleOptions.GridAlignment = DevExpress.XtraCharts.DateTimeGridAlignment.Day;
            xyDiagram1.AxisX.Label.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisX.Label.TextPattern = "{A:M/d}";
            xyDiagram1.AxisX.Title.Text = "日期";
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.Title.Text = "扫描次数、";
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            this.chartControl1.Diagram = xyDiagram1;
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Location = new System.Drawing.Point(0, 204);
            this.chartControl1.Name = "chartControl1";
            series1.ArgumentDataMember = "ScanDate";
            series1.Name = "总扫描数";
            series1.ValueDataMembersSerializable = "TotalCount";
            series2.ArgumentDataMember = "ScanDate";
            series2.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.DateTime;
            series2.Name = "成功扫描数";
            series2.ValueDataMembersSerializable = "SuccessCount";
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2};
            this.chartControl1.Size = new System.Drawing.Size(800, 226);
            this.chartControl1.TabIndex = 5;
            // 
            // FrmBCRScanRate_Statistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chartControl1);
            this.Controls.Add(this.gridControl1);
            this.Name = "FrmBCRScanRate_Statistics";
            this.Text = "BCR 扫描率统计";
            this.Controls.SetChildIndex(this.gridControl1, 0);
            this.Controls.SetChildIndex(this.chartControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn ScannerName1;
        private DevExpress.XtraGrid.Columns.GridColumn ScanDate1;
        private DevExpress.XtraGrid.Columns.GridColumn TotalCount1;
        private DevExpress.XtraGrid.Columns.GridColumn SuccessCount1;
        private DevExpress.XtraGrid.Columns.GridColumn FailCount1;
        private DevExpress.XtraGrid.Columns.GridColumn SuccessRate1;
        private DevExpress.XtraGrid.Columns.GridColumn Remark1;
        private DevExpress.XtraCharts.ChartControl chartControl1;
    }
}