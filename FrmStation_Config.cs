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
    public partial class FrmStation_Config : FrmBase
    {
        public FrmStation_Config()
        {
            InitializeComponent();
            this.Text = "站点信息配置";
        }

        private void FrmStation_Config_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT Id, StationNo, StationName, Location, PlcNo, ControlAddress, Status, Remark FROM T_Station_Config";
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
