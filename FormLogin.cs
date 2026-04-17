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
using WCS_Login.Utils;

namespace WCS_Login
{
    public partial class FormLogin : Form
    {
        public FormLogin()
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

                // ✅ 记录日志 - 数据库日志
                DbHelper.LogToDatabase(user, "用户登录", "用户管理", $"登录成功，角色：{role}, 模式：{mode}", "INFO");

                // ✅ 记录日志 - 文本日志
                Logger.Info($"用户 {user} 登录成功，角色：{role}, 模式：{mode}");

                // ✅ VS 调试输出
                Console.WriteLine($"[DEBUG] 登录成功：{user} ({role}), 模式：{mode}");

                // 保存当前用户（供其他窗体使用）
                Program.CurrentUserName = user;

                XtraMessageBox.Show(
                    $"登录成功！\n\n用户：{user}\n角色：{role}\n系统模式：{mode}\n\n欢迎进入 WCS 系统",
                    "成功",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // ✅ 打开主窗体
                FrmMain mainForm = new FrmMain();
                this.Hide();
                mainForm.Show();
                //this.Close();
            }
            else
            {
                // ✅ 登录失败 - 记录日志
                DbHelper.LogToDatabase(user, "用户登录", "用户管理", "登录失败 - 账号或密码错误", "WARN");
                Logger.Warn($"用户 {user} 登录失败 - 账号或密码错误");

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
            // 记录退出日志
            Logger.Info("用户取消登录，退出系统");
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