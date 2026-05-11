namespace WCS_Login
{
    partial class FrmBoxScanRecord_Query
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
            this.panelControlSearch = new DevExpress.XtraEditors.PanelControl();
            this.labelControlBoxNo = new DevExpress.XtraEditors.LabelControl();
            this.labelControlScanner = new DevExpress.XtraEditors.LabelControl();
            this.labelControlStation = new DevExpress.XtraEditors.LabelControl();
            this.labelControlScanResult = new DevExpress.XtraEditors.LabelControl();
            this.labelControlStartDate = new DevExpress.XtraEditors.LabelControl();
            this.labelControlEndDate = new DevExpress.XtraEditors.LabelControl();
            this.btnFilterReset = new DevExpress.XtraEditors.SimpleButton();
            this.txtBoxNo = new DevExpress.XtraEditors.TextEdit();
            this.txtScanner = new DevExpress.XtraEditors.TextEdit();
            this.txtStation = new DevExpress.XtraEditors.TextEdit();
            this.cmbScanResult = new DevExpress.XtraEditors.ComboBoxEdit();
            this.dateStart = new DevExpress.XtraEditors.DateEdit();
            this.dateEnd = new DevExpress.XtraEditors.DateEdit();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnRowNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelPagination = new DevExpress.XtraEditors.PanelControl();
            this.txtPageNo = new DevExpress.XtraEditors.TextEdit();
            this.btnNextPage = new DevExpress.XtraEditors.SimpleButton();
            this.btnPrevPage = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlSearch)).BeginInit();
            this.panelControlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBoxNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtScanner.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbScanResult.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateStart.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelPagination)).BeginInit();
            this.panelPagination.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPageNo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControlSearch
            // 
            this.panelControlSearch.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControlSearch.Controls.Add(this.labelControlBoxNo);
            this.panelControlSearch.Controls.Add(this.labelControlScanner);
            this.panelControlSearch.Controls.Add(this.labelControlStation);
            this.panelControlSearch.Controls.Add(this.labelControlScanResult);
            this.panelControlSearch.Controls.Add(this.labelControlStartDate);
            this.panelControlSearch.Controls.Add(this.labelControlEndDate);
            this.panelControlSearch.Controls.Add(this.btnFilterReset);
            this.panelControlSearch.Controls.Add(this.txtBoxNo);
            this.panelControlSearch.Controls.Add(this.txtScanner);
            this.panelControlSearch.Controls.Add(this.txtStation);
            this.panelControlSearch.Controls.Add(this.cmbScanResult);
            this.panelControlSearch.Controls.Add(this.dateStart);
            this.panelControlSearch.Controls.Add(this.dateEnd);
            this.panelControlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlSearch.Location = new System.Drawing.Point(0, 31);
            this.panelControlSearch.Name = "panelControlSearch";
            this.panelControlSearch.Size = new System.Drawing.Size(948, 100);
            this.panelControlSearch.TabIndex = 6;
            // 
            // labelControlBoxNo
            // 
            this.labelControlBoxNo.Appearance.Options.UseTextOptions = true;
            this.labelControlBoxNo.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControlBoxNo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControlBoxNo.Location = new System.Drawing.Point(12, 12);
            this.labelControlBoxNo.Name = "labelControlBoxNo";
            this.labelControlBoxNo.Size = new System.Drawing.Size(60, 24);
            this.labelControlBoxNo.TabIndex = 0;
            this.labelControlBoxNo.Text = "箱号：";
            // 
            // labelControlScanner
            // 
            this.labelControlScanner.Appearance.Options.UseTextOptions = true;
            this.labelControlScanner.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControlScanner.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControlScanner.Location = new System.Drawing.Point(210, 12);
            this.labelControlScanner.Name = "labelControlScanner";
            this.labelControlScanner.Size = new System.Drawing.Size(70, 24);
            this.labelControlScanner.TabIndex = 2;
            this.labelControlScanner.Text = "扫描器：";
            // 
            // labelControlStation
            // 
            this.labelControlStation.Appearance.Options.UseTextOptions = true;
            this.labelControlStation.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControlStation.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControlStation.Location = new System.Drawing.Point(420, 12);
            this.labelControlStation.Name = "labelControlStation";
            this.labelControlStation.Size = new System.Drawing.Size(60, 24);
            this.labelControlStation.TabIndex = 4;
            this.labelControlStation.Text = "站点：";
            // 
            // labelControlScanResult
            // 
            this.labelControlScanResult.Appearance.Options.UseTextOptions = true;
            this.labelControlScanResult.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControlScanResult.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControlScanResult.Location = new System.Drawing.Point(-1, 51);
            this.labelControlScanResult.Name = "labelControlScanResult";
            this.labelControlScanResult.Size = new System.Drawing.Size(95, 24);
            this.labelControlScanResult.TabIndex = 6;
            this.labelControlScanResult.Text = "扫描结果：";
            // 
            // labelControlStartDate
            // 
            this.labelControlStartDate.Appearance.Options.UseTextOptions = true;
            this.labelControlStartDate.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControlStartDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControlStartDate.Location = new System.Drawing.Point(221, 50);
            this.labelControlStartDate.Name = "labelControlStartDate";
            this.labelControlStartDate.Size = new System.Drawing.Size(89, 24);
            this.labelControlStartDate.TabIndex = 8;
            this.labelControlStartDate.Text = "开始时间：";
            // 
            // labelControlEndDate
            // 
            this.labelControlEndDate.Appearance.Options.UseTextOptions = true;
            this.labelControlEndDate.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControlEndDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControlEndDate.Location = new System.Drawing.Point(459, 50);
            this.labelControlEndDate.Name = "labelControlEndDate";
            this.labelControlEndDate.Size = new System.Drawing.Size(91, 24);
            this.labelControlEndDate.TabIndex = 10;
            this.labelControlEndDate.Text = "结束时间：";
            // 
            // btnFilterReset
            // 
            this.btnFilterReset.Location = new System.Drawing.Point(720, 48);
            this.btnFilterReset.Name = "btnFilterReset";
            this.btnFilterReset.Size = new System.Drawing.Size(80, 28);
            this.btnFilterReset.TabIndex = 12;
            this.btnFilterReset.Text = "重置";
            // 
            // txtBoxNo
            // 
            this.txtBoxNo.Location = new System.Drawing.Point(80, 12);
            this.txtBoxNo.Name = "txtBoxNo";
            this.txtBoxNo.Size = new System.Drawing.Size(120, 28);
            this.txtBoxNo.TabIndex = 1;
            // 
            // txtScanner
            // 
            this.txtScanner.Location = new System.Drawing.Point(290, 12);
            this.txtScanner.Name = "txtScanner";
            this.txtScanner.Size = new System.Drawing.Size(120, 28);
            this.txtScanner.TabIndex = 3;
            // 
            // txtStation
            // 
            this.txtStation.Location = new System.Drawing.Point(490, 12);
            this.txtStation.Name = "txtStation";
            this.txtStation.Size = new System.Drawing.Size(120, 28);
            this.txtStation.TabIndex = 5;
            // 
            // cmbScanResult
            // 
            this.cmbScanResult.Location = new System.Drawing.Point(95, 50);
            this.cmbScanResult.Name = "cmbScanResult";
            this.cmbScanResult.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbScanResult.Properties.Items.AddRange(new object[] {
            "",
            "成功",
            "失败"});
            this.cmbScanResult.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbScanResult.Size = new System.Drawing.Size(120, 28);
            this.cmbScanResult.TabIndex = 7;
            // 
            // dateStart
            // 
            this.dateStart.EditValue = null;
            this.dateStart.Location = new System.Drawing.Point(313, 50);
            this.dateStart.Name = "dateStart";
            this.dateStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateStart.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateStart.Size = new System.Drawing.Size(140, 28);
            this.dateStart.TabIndex = 9;
            // 
            // dateEnd
            // 
            this.dateEnd.EditValue = null;
            this.dateEnd.Location = new System.Drawing.Point(560, 50);
            this.dateEnd.Name = "dateEnd";
            this.dateEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEnd.Size = new System.Drawing.Size(140, 28);
            this.dateEnd.TabIndex = 11;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 131);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(948, 278);
            this.gridControl1.TabIndex = 4;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnRowNo,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);
            // 
            // gridColumnRowNo
            // 
            this.gridColumnRowNo.Caption = "行号";
            this.gridColumnRowNo.FieldName = "RowNo";
            this.gridColumnRowNo.MinWidth = 30;
            this.gridColumnRowNo.Name = "gridColumnRowNo";
            this.gridColumnRowNo.OptionsColumn.AllowEdit = false;
            this.gridColumnRowNo.OptionsColumn.ReadOnly = true;
            this.gridColumnRowNo.UnboundDataType = typeof(string);
            this.gridColumnRowNo.Visible = true;
            this.gridColumnRowNo.VisibleIndex = 0;
            this.gridColumnRowNo.Width = 30;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "ID";
            this.gridColumn1.FieldName = "Id";
            this.gridColumn1.MinWidth = 30;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 87;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "箱号";
            this.gridColumn2.FieldName = "BoxNo";
            this.gridColumn2.MinWidth = 30;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 164;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "扫描器";
            this.gridColumn3.FieldName = "ScannerName";
            this.gridColumn3.MinWidth = 30;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            this.gridColumn3.Width = 164;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "扫描时间";
            this.gridColumn4.FieldName = "ScanTime";
            this.gridColumn4.MinWidth = 30;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 4;
            this.gridColumn4.Width = 164;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "扫描结果";
            this.gridColumn5.FieldName = "ScanResult";
            this.gridColumn5.MinWidth = 30;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 5;
            this.gridColumn5.Width = 131;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "站点";
            this.gridColumn6.FieldName = "StationName";
            this.gridColumn6.MinWidth = 30;
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 6;
            this.gridColumn6.Width = 110;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "备注";
            this.gridColumn7.FieldName = "Remark";
            this.gridColumn7.MinWidth = 30;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 7;
            this.gridColumn7.Width = 92;
            // 
            // panelPagination
            // 
            this.panelPagination.Controls.Add(this.txtPageNo);
            this.panelPagination.Controls.Add(this.btnNextPage);
            this.panelPagination.Controls.Add(this.btnPrevPage);
            this.panelPagination.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelPagination.Location = new System.Drawing.Point(0, 409);
            this.panelPagination.Name = "panelPagination";
            this.panelPagination.Size = new System.Drawing.Size(948, 29);
            this.panelPagination.TabIndex = 5;
            // 
            // txtPageNo
            // 
            this.txtPageNo.EditValue = "1";
            this.txtPageNo.Location = new System.Drawing.Point(92, 1);
            this.txtPageNo.Name = "txtPageNo";
            this.txtPageNo.Properties.MaxLength = 5;
            this.txtPageNo.Size = new System.Drawing.Size(50, 28);
            this.txtPageNo.TabIndex = 2;
            // 
            // btnNextPage
            // 
            this.btnNextPage.Location = new System.Drawing.Point(148, 0);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(80, 29);
            this.btnNextPage.TabIndex = 1;
            this.btnNextPage.Text = "下一页";
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.Location = new System.Drawing.Point(6, 0);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(80, 29);
            this.btnPrevPage.TabIndex = 0;
            this.btnPrevPage.Text = "上一页";
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // FrmBoxScanRecord_Query
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 458);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.panelPagination);
            this.Controls.Add(this.panelControlSearch);
            this.Name = "FrmBoxScanRecord_Query";
            this.Text = "周转箱扫描记录查询";
            this.Load += new System.EventHandler(this.FrmBoxScanRecord_Query_Load);
            this.Controls.SetChildIndex(this.panelControlSearch, 0);
            this.Controls.SetChildIndex(this.panelPagination, 0);
            this.Controls.SetChildIndex(this.gridControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlSearch)).EndInit();
            this.panelControlSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtBoxNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtScanner.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbScanResult.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateStart.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelPagination)).EndInit();
            this.panelPagination.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtPageNo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnRowNo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.PanelControl panelControlSearch;
        private DevExpress.XtraEditors.LabelControl labelControlBoxNo;
        private DevExpress.XtraEditors.TextEdit txtBoxNo;
        private DevExpress.XtraEditors.LabelControl labelControlScanner;
        private DevExpress.XtraEditors.TextEdit txtScanner;
        private DevExpress.XtraEditors.LabelControl labelControlStation;
        private DevExpress.XtraEditors.TextEdit txtStation;
        private DevExpress.XtraEditors.LabelControl labelControlScanResult;
        private DevExpress.XtraEditors.ComboBoxEdit cmbScanResult;
        private DevExpress.XtraEditors.LabelControl labelControlStartDate;
        private DevExpress.XtraEditors.DateEdit dateStart;
        private DevExpress.XtraEditors.LabelControl labelControlEndDate;
        private DevExpress.XtraEditors.DateEdit dateEnd;
        private DevExpress.XtraEditors.SimpleButton btnFilterReset;
        private DevExpress.XtraEditors.PanelControl panelPagination;
        private DevExpress.XtraEditors.SimpleButton btnNextPage;
        private DevExpress.XtraEditors.SimpleButton btnPrevPage;
        private DevExpress.XtraEditors.TextEdit txtPageNo;
    }
}