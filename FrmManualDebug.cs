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

namespace WCS_Login
{
    public partial class FrmManualDebug : FrmBase
    {
        public FrmManualDebug()
        {
            InitializeComponent();
            this.Text = "手动调试";
        }

        private void FrmManualDebug_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT Id, DebugItem, CurrentValue, TargetValue, Status, LastUpdateTime, Remark FROM T_ManualDebug";
                gridControl1.DataSource = DbHelper.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"数据库连接失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
