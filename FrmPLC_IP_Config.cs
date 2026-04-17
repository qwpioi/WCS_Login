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
using System.Data.SqlClient;
using WCS_Login.Utils;

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
                LoadData();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"数据库连接失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            string sql = "SELECT PlcNo, IP, Port, PlcType, HeartbeatAddress, Remark FROM T_PLC_IP_Config";
            gridControl1.DataSource = DbHelper.ExecuteQuery(sql);
        }

        /// <summary>
        /// 重写查询按钮事件
        /// </summary>
        protected override void BtnQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadData();

                // 记录日志
                DbHelper.LogToDatabase(Program.CurrentUserName, "查询数据", "PLC 配置", "查询 PLC IP 配置列表", "INFO");
                Logger.Info($"用户 {Program.CurrentUserName} 查询 PLC IP 配置");

                XtraMessageBox.Show("查询成功！", "提示");
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "查询数据", "PLC 配置", $"查询失败：{ex.Message}", "ERROR");
                Logger.Error($"查询失败：{ex.Message}", Program.CurrentUserName);

                XtraMessageBox.Show($"查询失败：{ex.Message}", "错误");
            }
        }

        /// <summary>
        /// 重写导出按钮事件
        /// </summary>
        protected override void BtnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel 文件|*.xlsx";
                saveFileDialog.Title = "导出数据";
                saveFileDialog.FileName = $"PLC_IP 配置_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    gridControl1.ExportToXlsx(saveFileDialog.FileName);

                    // 记录日志
                    DbHelper.LogToDatabase(Program.CurrentUserName, "导出数据", "PLC 配置", $"导出到 {saveFileDialog.FileName}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 导出 PLC IP 配置到 {saveFileDialog.FileName}");

                    XtraMessageBox.Show("导出成功！", "提示");
                }
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "导出数据", "PLC 配置", $"导出失败：{ex.Message}", "ERROR");
                Logger.Error($"导出失败：{ex.Message}", Program.CurrentUserName);

                XtraMessageBox.Show($"导出失败：{ex.Message}", "错误");
            }
        }

        /// <summary>
        /// 重写保存按钮事件
        /// </summary>
        protected override void BtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            try
            {
                gridView1.CloseEditor(); // 关闭当前编辑

                var row = gridView1.GetFocusedRow();
                if (row == null)
                {
                    XtraMessageBox.Show("请选择要保存的记录！", "提示");
                    return;
                }

                // 获取当前行数据
                string plcNo = gridView1.GetFocusedRowCellValue("PlcNo").ToString();
                string ip = gridView1.GetFocusedRowCellValue("IP").ToString();
                string port = gridView1.GetFocusedRowCellValue("Port").ToString();
                string plcType = gridView1.GetFocusedRowCellValue("PlcType").ToString();
                string heartbeatAddress = gridView1.GetFocusedRowCellValue("HeartbeatAddress").ToString();
                string remark = gridView1.GetFocusedRowCellValue("Remark").ToString();

                // 构建更新 SQL
                string sql = @"UPDATE T_PLC_IP_Config
                       SET IP = @IP, Port = @Port, PlcType = @PlcType,
                           HeartbeatAddress = @HeartbeatAddress, Remark = @Remark
                       WHERE PlcNo = @PlcNo";

                SqlParameter[] parameters = {
            new SqlParameter("@IP", ip),
            new SqlParameter("@Port", port),
            new SqlParameter("@PlcType", plcType),
            new SqlParameter("@HeartbeatAddress", heartbeatAddress),
            new SqlParameter("@Remark", remark),
            new SqlParameter("@PlcNo", plcNo)
        };

                int rows = DbHelper.ExecuteNonQuery(sql, parameters);

                if (rows > 0)
                {
                    // 记录日志
                    DbHelper.LogToDatabase(Program.CurrentUserName, "保存数据", "PLC 配置", $"修改 PLC {plcNo}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 保存 PLC IP 配置，PLC 编号：{plcNo}");

                    XtraMessageBox.Show("保存成功！", "提示");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "保存数据", "PLC 配置", $"保存失败：{ex.Message}", "ERROR");
                Logger.Error($"保存失败：{ex.Message}", Program.CurrentUserName);

                XtraMessageBox.Show($"保存失败：{ex.Message}", "错误");
            }
        }

        /// <summary>
        /// 重写删除按钮事件
        /// </summary>
        protected override void BtnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var row = gridView1.GetFocusedRow();
                if (row == null)
                {
                    XtraMessageBox.Show("请选择要删除的记录！", "提示");
                    return;
                }

                // 确认删除
                if (XtraMessageBox.Show("确定要删除选中记录吗？", "确认",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                string plcNo = gridView1.GetFocusedRowCellValue("PlcNo").ToString();

                string sql = "DELETE FROM T_PLC_IP_Config WHERE PlcNo = @PlcNo";
                int rows = DbHelper.ExecuteNonQuery(sql, new SqlParameter[] {
            new SqlParameter("@PlcNo", plcNo)
        });

                if (rows > 0)
                {
                    // 记录日志
                    DbHelper.LogToDatabase(Program.CurrentUserName, "删除数据", "PLC 配置", $"删除 PLC {plcNo}", "INFO");
                    Logger.Info($"用户 {Program.CurrentUserName} 删除 PLC IP 配置，PLC 编号：{plcNo}");

                    XtraMessageBox.Show("删除成功！", "提示");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                DbHelper.LogToDatabase(Program.CurrentUserName, "删除数据", "PLC 配置", $"删除失败：{ex.Message}", "ERROR");
                Logger.Error($"删除失败：{ex.Message}", Program.CurrentUserName);

                XtraMessageBox.Show($"删除失败：{ex.Message}", "错误");
            }
        }

        /// <summary>
        /// 重写刷新按钮事件
        /// </summary>
        protected override void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadData();

                // 记录日志（可选）
                Logger.Info($"用户 {Program.CurrentUserName} 刷新 PLC IP 配置数据");

                XtraMessageBox.Show("刷新成功！", "提示");
            }
            catch (Exception ex)
            {
                Logger.Error($"刷新失败：{ex.Message}", Program.CurrentUserName);
                XtraMessageBox.Show($"刷新失败：{ex.Message}", "错误");
            }
        }

    }
}
