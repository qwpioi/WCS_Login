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
    public partial class FrmPLC_IP_Config : FrmBase
    {
        public FrmPLC_IP_Config()
        {
            InitializeComponent();
            this.Text = "PLC_IP 配置";
        }

        /// <summary>
        /// 窗体加载事件 - 从数据库加载数据
        /// </summary>
        private void FrmPLC_IP_Config_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT PlcNo, IP, Port, PlcType, HeartbeatAddress, Remark FROM T_PLC_IP_Config";
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
