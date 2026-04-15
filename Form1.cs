using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WCS_Login
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // 1. 获取输入内容
            string user = txtUser.Text.Trim();
            string pwd = txtPwd.Text.Trim();
            string mode = cmbMode.Text;

            // 2. 验证输入是否为空
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pwd))
            {
                XtraMessageBox.Show("账号或密码不能为空！", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUser.Focus();
                return;
            }

            // 3. 数据库验证（参数化查询防止 SQL 注入）
            string sql = "SELECT COUNT(*) FROM Users WHERE UserName = @UserName AND Password = @Password";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserName", user),
                new SqlParameter("@Password", pwd)
            };

            // 4. 执行查询
            DataTable dt = DbHelper.ExecuteQuery(sql, parameters);
            int count = Convert.ToInt32(dt.Rows[0][0]);

            // 5. 判断结果
            if (count > 0)
            {
                // 登录成功，获取用户角色
                string role = GetUserRole(user);

                XtraMessageBox.Show(
                    $"登录成功！\n\n用户：{user}\n角色：{role}\n系统模式：{mode}\n\n欢迎进入 WCS 系统",
                    "成功",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // TODO: 后续跳转到主窗体
                // this.Hide();
                // FrmMain main = new FrmMain();
                // main.ShowDialog();
                // this.Close();
            }
            else
            {
                XtraMessageBox.Show("账号或密码错误！", "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtPwd.Clear();
                txtUser.Focus();
                txtUser.SelectAll();
            }
        }

        /// <summary>
        /// 获取用户角色信息
        /// </summary>
        private string GetUserRole(string userName)
        {
            string sql = "SELECT Role FROM Users WHERE UserName = @UserName";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserName", userName)
            };

            DataTable dt = DbHelper.ExecuteQuery(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["Role"].ToString();
            }
            return "Unknown";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 默认选择运行模式
            cmbMode.SelectedIndex = 0;

            // 默认聚焦账号输入框
            txtUser.Focus();
        }
    }
}