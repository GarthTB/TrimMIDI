using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TrimMIDI.Tool
{
    internal static class Logger
    {
        private static readonly ConcurrentQueue<string> _log = new();

        public static void Add(string message)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            _log.Enqueue($"{timestamp}\t{message}");
        }

        public static void Save()
        {
            TryCatch.Do("保存日志", () =>
            {
                if (_log.IsEmpty)
                    throw new InvalidOperationException("日志为空！");

                string directory = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(directory, "Log.txt");

                StringBuilder sb = new();
                while (_log.TryDequeue(out var log))
                    _ = sb.AppendLine(log);

                using (StreamWriter writer = new(filePath, append: true))
                    writer.Write(sb.ToString());

                using Process process = new()
                {
                    StartInfo = { FileName = filePath, UseShellExecute = true }
                };

                if (MsgB.YNInfo($"日志已成功保存到： {filePath}\n是否立即打开？", "提示"))
                    _ = process.Start();
            });
        }
    }
}
