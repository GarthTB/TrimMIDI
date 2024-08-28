using Melanchall.DryWetMidi.Core;
using System.IO;

namespace TrimMIDI.Tool
{
    internal static class Buffer
    {
        private static string _rootPath = string.Empty;

        public static MidiFile[] MidiFiles { get; private set; } = [];

        public static void Clear()
        {
            _rootPath = string.Empty;
            MidiFiles = [];
        }

        public static void LoadMidiFiles(string path)
        {
            var midiFiles = new List<MidiFile>();

            try
            {
                if (ParsePath(path, out bool isSingleFile))
                {
                    if (isSingleFile)
                    {
                        _rootPath = Path.GetDirectoryName(path)
                            ?? throw new Exception("无法获取文件所在目录！");
                        Collect(path, midiFiles);
                    }
                    else
                    {
                        _rootPath = path;
                        var paths = Directory.GetFiles(path).Where(MIDIProc.IsMIDIFile);
                        foreach (var p in paths)
                            Collect(p, midiFiles);
                    }

                    MsgB.OkInfo($"成功载入{midiFiles.Count}个MIDI文件！", "提示");
                }
                else throw new Exception($"路径{path}不存在或不是有效的MIDI文件或文件夹！");

                MidiFiles = [.. midiFiles];
            }
            catch (Exception ex)
            {
                MsgB.OkErr($"载入文件出错：{ex.Message}", "错误");
            }
        }

        public static void SaveMidiFiles(MidiFile f)
        {
            try
            {
                var oriName = f.GetOriName();
                var filePath = Path.Combine(_rootPath, $"{oriName}_trimmed.mid");
                f.Write(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"保存文件出错：{ex.Message}");
            }
        }

        private static bool ParsePath(string path, out bool isSingleFile)
        {
            if (File.Exists(path))
            {
                isSingleFile = true;
                return true;
            }
            else if (Directory.Exists(path))
            {
                isSingleFile = false;
                return true;
            }
            else
            {
                isSingleFile = false;
                return false;
            }
        }

        private static void Collect(string path, List<MidiFile> midiFiles)
        {
            var midiFile = MidiFile.Read(path);
            var fileName = Path.GetFileNameWithoutExtension(Path.GetFileName(path));
            midiFile.SetOriName(fileName);
            midiFiles.Add(midiFile);
        }
    }
}
