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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //1.获取内容
            string user = txtUser.Text.Trim();
            string pwd = txtPwd.Text.Trim();
            string mode = cmbMode.Text;

            //2.验证输入是否为空
            if (user == "" || pwd == "")
            {
                MessageBox.Show("用户名或密码不能为空！","提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUser.Focus();
                return;
            }

            //3.验证账号密码（测试账号:admin / 123456）
            if(user == "admin" && pwd == "123456")
            {
                MessageBox.Show(
                    $"登录成功！\\n\\n系统模式:{mode}\\n\n欢迎进入 WCS 系统",
                    "成功",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // TODO:后续跳转到主窗体
                // this.Hide();
                // frmMain main = new frmMain();
                // main.ShowDialog();
            }
            else
            {
                MessageBox.Show("用户名或密码错误！", "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtUser.Focus();
                txtUser.SelectAll();
                return;
            }
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
