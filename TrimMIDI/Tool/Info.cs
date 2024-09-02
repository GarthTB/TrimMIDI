using System.Text;

namespace TrimMIDI.Tool
{
    internal static class Info
    {
        /// <summary> 版本号 </summary>
        private const string version = "0.1.1";

        /// <summary> 欢迎信息 </summary>
        public static bool Welc()
        {
            var sb = new StringBuilder();
            var msg = sb.AppendLine("欢迎使用MIDI批量修饰工具！")
                        .AppendLine("用法详见README.md，按F1复制仓库链接。")
                        .AppendLine($"版本号：{version}")
                        .AppendLine("作者：Garth\n")
                        .AppendLine("是否不再显示此欢迎页？")
                        .ToString();
            return MsgB.YNInfo(msg, "欢迎");
        }
    }
}
