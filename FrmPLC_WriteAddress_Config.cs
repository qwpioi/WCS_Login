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
    public partial class FrmPLC_WriteAddress_Config : FrmBase
    {
        public FrmPLC_WriteAddress_Config()
        {
            InitializeComponent();
            this.Text = "PLC 写变量地址配置";
        }

        private void FrmPLC_WriteAddress_Config_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT Id, VariableName, Address, DataType, PlcNo, Description, Remark FROM T_PLC_WriteAddress_Config";
                gridControl1.DataSource = DbHelper.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"数据库连接失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }
    }
}
