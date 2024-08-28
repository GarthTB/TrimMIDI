using System.Text;
using System.Windows;

namespace TrimMIDI.Tool
{
    internal static class Info
    {
        /// <summary> 版本号 </summary>
        private const string version = "0.1.0";

        /// <summary> 帮助信息 </summary>
        public static void Help()
        {
            var sb = new StringBuilder();
            var msg = sb.AppendLine("快捷键：")
                        .AppendLine("    F1：帮助")
                        .AppendLine("    F3：保存日志")
                        .AppendLine("    F5：清空\n")
                        .AppendLine("用法详见README.md\n")
                        .AppendLine($"MIDI批量修饰工具 v{version}")
                        .AppendLine("© Garth 2024\n")
                        .AppendLine("是否复制仓库地址？")
                        .ToString();
            if (MsgB.YNInfo(msg, "帮助"))
            {
                Clipboard.SetText("https://github.com/GarthTB/TrimMIDI");
                MsgB.OkInfo("仓库地址已复制到剪贴板。", "提示");
            }
        }

        /// <summary> 欢迎信息 </summary>
        public static void Welc()
        {
            var sb = new StringBuilder();
            var msg = sb.AppendLine("欢迎使用MIDI批量修饰工具！")
                        .AppendLine("按F1查看帮助")
                        .AppendLine($"版本号：{version}")
                        .AppendLine("作者：Garth")
                        .ToString();
            MsgB.OkInfo(msg, "欢迎");
        }
    }
}
