namespace WCS_Login
{
    partial class FrmPLCAdd
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPlcNo = new DevExpress.XtraEditors.TextEdit();
            this.txtIP = new DevExpress.XtraEditors.TextEdit();
            this.txtPort = new DevExpress.XtraEditors.TextEdit();
            this.cboPlcType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtHeartbeatAddress = new DevExpress.XtraEditors.TextEdit();
            this.txtRemark = new DevExpress.XtraEditors.TextEdit();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlcNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlcType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeartbeatAddress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).BeginInit();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(30, 18);
            this.label1.Size = new System.Drawing.Size(80, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "PLC 编号 *";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // label2
            //
            this.label2.Location = new System.Drawing.Point(30, 53);
            this.label2.Size = new System.Drawing.Size(80, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "IP 地址 *";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // label3
            //
            this.label3.Location = new System.Drawing.Point(30, 88);
            this.label3.Size = new System.Drawing.Size(80, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "端口";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // label4
            //
            this.label4.Location = new System.Drawing.Point(30, 123);
            this.label4.Size = new System.Drawing.Size(80, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "PLC 类型";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // label5
            //
            this.label5.Location = new System.Drawing.Point(30, 158);
            this.label5.Size = new System.Drawing.Size(80, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "心跳变量地址";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // label6
            //
            this.label6.Location = new System.Drawing.Point(30, 193);
            this.label6.Size = new System.Drawing.Size(80, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "备注";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtPlcNo
            //
            this.txtPlcNo.Location = new System.Drawing.Point(120, 16);
            this.txtPlcNo.Name = "txtPlcNo";
            this.txtPlcNo.Size = new System.Drawing.Size(200, 24);
            this.txtPlcNo.TabIndex = 6;
            //
            // txtIP
            //
            this.txtIP.Location = new System.Drawing.Point(120, 51);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(200, 24);
            this.txtIP.TabIndex = 7;
            //
            // txtPort
            //
            this.txtPort.Location = new System.Drawing.Point(120, 86);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(200, 24);
            this.txtPort.TabIndex = 8;
            //
            // cboPlcType
            //
            this.cboPlcType.Location = new System.Drawing.Point(120, 121);
            this.cboPlcType.Name = "cboPlcType";
            this.cboPlcType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboPlcType.Properties.Items.AddRange(new object[] {
                "S7-200",
                "S7-300",
                "S7-1200",
                "S7-1500"});
            this.cboPlcType.Size = new System.Drawing.Size(200, 24);
            this.cboPlcType.TabIndex = 9;
            //
            // txtHeartbeatAddress
            //
            this.txtHeartbeatAddress.Location = new System.Drawing.Point(120, 156);
            this.txtHeartbeatAddress.Name = "txtHeartbeatAddress";
            this.txtHeartbeatAddress.Size = new System.Drawing.Size(200, 24);
            this.txtHeartbeatAddress.TabIndex = 10;
            //
            // txtRemark
            //
            this.txtRemark.Location = new System.Drawing.Point(120, 191);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(200, 24);
            this.txtRemark.TabIndex = 11;
            //
            // btnOk
            //
            this.btnOk.Location = new System.Drawing.Point(135, 235);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 30);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(235, 235);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // FrmPLCAdd
            //
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(360, 285);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.txtHeartbeatAddress);
            this.Controls.Add(this.cboPlcType);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.txtPlcNo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPLCAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新增 PLC";
            ((System.ComponentModel.ISupportInitialize)(this.txtPlcNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlcType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeartbeatAddress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit txtPlcNo;
        private DevExpress.XtraEditors.TextEdit txtIP;
        private DevExpress.XtraEditors.TextEdit txtPort;
        private DevExpress.XtraEditors.ComboBoxEdit cboPlcType;
        private DevExpress.XtraEditors.TextEdit txtHeartbeatAddress;
        private DevExpress.XtraEditors.TextEdit txtRemark;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}
