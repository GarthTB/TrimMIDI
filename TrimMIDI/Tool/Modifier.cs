using Melanchall.DryWetMidi.Core;

namespace TrimMIDI.Tool
{
    internal static class Modifier
    {
        public static List<Action<MidiFile>> Process { get; set; } = [];

        /// <summary>
        /// 运行修改器，包含保存和清空缓冲区的操作
        /// </summary>
        public static void Run()
        {
            try
            {
                Process.Add(Buffer.SaveMidiFiles);
                if (Buffer.MidiFiles.Length < 16)
                    RunSerial();
                else RunParallel();
                MsgB.OkInfo($"{Buffer.MidiFiles.Length}个文件修改成功！", "成功");
            }
            catch (Exception ex)
            {
                MsgB.OkErr($"修改失败，可能产生了垃圾，请检查。\n错误：{ex.Message}", "错误");
            }
            Process = [];
            Buffer.Clear();
        }

        private static void RunSerial()
        {
            foreach (var f in Buffer.MidiFiles)
                foreach (var p in Process)
                    p(f);
        }

        private static void RunParallel()
        {
            _ = Parallel.ForEach(Buffer.MidiFiles, static f =>
            {
                foreach (var p in Process)
                    p(f);
            });
        }

        public static Action<MidiFile> ShortCut(int threshold)
            => f =>
            {
                var count = MIDIProc.ShortCut(f, threshold);
                Logger.Add($"文件：{f.GetOriName()}\t除去短于{threshold}的音符。\t共{count}个。");
            };

        public static Action<MidiFile> Gap(int gapTime)
            => f =>
            {
                var count = MIDIProc.Gap(f, gapTime);
                Logger.Add($"文件：{f.GetOriName()}\t缩短后间隔小于{gapTime}的音符。\t共{count}个。");
            };

        public static Action<MidiFile> EQ((int p1, int p2, int p3, int p4, double g1, double g2, double g3, double g4) eq)
            => f =>
            {
                MIDIProc.EQ(f, eq.p1, eq.p2, eq.p3, eq.p4, eq.g1, eq.g2, eq.g3, eq.g4);
                Logger.Add($"文件：{f.GetOriName()}\t调节不同音高的力度。\t音高：{eq.p1}, {eq.p2}, {eq.p3}, {eq.p4}\t增益：{eq.g1}, {eq.g2}, {eq.g3}, {eq.g4}");
            };
    }
}
