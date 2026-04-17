using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WCS_Login
{
    internal static class Program
    {
        /// <summary>
        /// 当前登录用户（全局可访问）
        /// </summary>
        public static string CurrentUserName { get; set; } = "System";

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormLogin());
        }
    }
}
