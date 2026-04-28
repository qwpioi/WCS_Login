namespace WCS_Login
{
    partial class FrmPLC_Monitor
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
            this.chkAutoScroll = new DevExpress.XtraEditors.CheckEdit();
            this.memoLog = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoScroll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chkAutoScroll
            // 
            this.chkAutoScroll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chkAutoScroll.EditValue = true;
            this.chkAutoScroll.Location = new System.Drawing.Point(0, 403);
            this.chkAutoScroll.MenuManager = this.barManager1;
            this.chkAutoScroll.Name = "chkAutoScroll";
            this.chkAutoScroll.Properties.Caption = "自动滚动到最新日志";
            this.chkAutoScroll.Size = new System.Drawing.Size(800, 27);
            this.chkAutoScroll.TabIndex = 4;
            // 
            // memoLog
            // 
            this.memoLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoLog.Location = new System.Drawing.Point(0, 31);
            this.memoLog.MenuManager = this.barManager1;
            this.memoLog.Name = "memoLog";
            this.memoLog.Properties.ReadOnly = true;
            this.memoLog.Size = new System.Drawing.Size(800, 372);
            this.memoLog.TabIndex = 5;
            // 
            // FrmPLC_Monitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.memoLog);
            this.Controls.Add(this.chkAutoScroll);
            this.Name = "FrmPLC_Monitor";
            this.Text = "FrmPLC_Monitor";
            this.Controls.SetChildIndex(this.chkAutoScroll, 0);
            this.Controls.SetChildIndex(this.memoLog, 0);
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoScroll.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit chkAutoScroll;
        private DevExpress.XtraEditors.MemoEdit memoLog;
    }
}