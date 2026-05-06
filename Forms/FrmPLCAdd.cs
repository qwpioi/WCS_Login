using DevExpress.XtraEditors;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WCS_Login
{
    public partial class FrmPLCAdd : XtraForm
    {
        public string PlcNo { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string PlcType { get; set; }
        public string HeartbeatAddress { get; set; }
        public string Remark { get; set; }

        public FrmPLCAdd()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlcNo.Text))
            {
                XtraMessageBox.Show("请输入 PLC 编号！", "提示");
                txtPlcNo.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtIP.Text))
            {
                XtraMessageBox.Show("请输入 IP 地址！", "提示");
                txtIP.Focus();
                return;
            }

            PlcNo = txtPlcNo.Text.Trim();
            IP = txtIP.Text.Trim();
            Port = txtPort.Text.Trim();
            PlcType = cboPlcType.Text.Trim();
            HeartbeatAddress = txtHeartbeatAddress.Text.Trim();
            Remark = txtRemark.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
