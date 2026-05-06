using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
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
            // 注册 DevExpress 内置所有皮肤
            SkinManager.Default.RegisterAssembly(typeof(UserLookAndFeel).Assembly);
            /*BonusSkins.Register();
            OfficeSkins.Register();*/

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormLogin());
        }
    }
}
