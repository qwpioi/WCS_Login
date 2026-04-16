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
    public partial class FrmBoxTask_Query : FrmBase
    {
        public FrmBoxTask_Query()
        {
            InitializeComponent();
            this.Text = "周转箱任务查询";
        }

        private void FrmBoxTask_Query_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT Id, BoxNo, TaskType, TaskRule, CreateTime, CreateUser, Remark FROM T_Box_Task";
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
