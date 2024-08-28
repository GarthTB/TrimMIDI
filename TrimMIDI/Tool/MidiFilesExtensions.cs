using Melanchall.DryWetMidi.Core;
using System.Collections.Concurrent;

namespace TrimMIDI.Tool
{
    internal static class MidiFilesExtensions
    {
        /// <summary>
        /// 用于存储原始文件名的字典
        /// </summary>
        private static readonly ConcurrentDictionary<MidiFile, string> _oriName = new();

        /// <summary> 获取原始文件名 </summary>
        public static string GetOriName(this MidiFile note)
            => _oriName.TryGetValue(note, out var category)
                ? category
                : string.Empty;

        /// <summary> 设置原始文件名 </summary>
        public static void SetOriName(this MidiFile note, string category)
            => _oriName[note] = category;
    }
}
