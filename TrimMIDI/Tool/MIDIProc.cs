using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using Note = Melanchall.DryWetMidi.Interaction.Note;

namespace TrimMIDI.Tool
{
    internal static class MIDIProc
    {
        public static int ShortCut(MidiFile f, int threshold)
            => f.RemoveNotes(n => n.Length < threshold);

        public static int Gap(MidiFile f, int gapTime)
        {
            int count = 0;
            foreach (var trackChunk in f.GetTrackChunks())
            {
                using var notesManager = trackChunk.ManageNotes();
                var notes = notesManager.Objects.OfType<Note>();
                count += NoteProc.ShortenNotes(notes, gapTime);
                notesManager.SaveChanges();
            }
            return count;
        }

        public static void EQ(MidiFile f, int pitch1, int pitch2, int pitch3, int pitch4, double gain1, double gain2, double gain3, double gain4)
        {
            foreach (var trackChunk in f.GetTrackChunks())
            {
                using var notesManager = trackChunk.ManageNotes();
                var notes = notesManager.Objects.OfType<Note>();
                NoteProc.VelGradGain(notes, 0, pitch1, gain1, gain1);
                NoteProc.VelGradGain(notes, pitch1, pitch2, gain1, gain2);
                NoteProc.VelGradGain(notes, pitch2, pitch3, gain2, gain3);
                NoteProc.VelGradGain(notes, pitch3, pitch4, gain3, gain4);
                NoteProc.VelGradGain(notes, pitch4, 128, gain4, gain4);
                notesManager.SaveChanges();
            }
        }

        public static bool IsMIDIFileOrDirectory(string path)
        {
            if (File.Exists(path) && IsMIDIFile(path))
                return true;
            else if (Directory.Exists(path))
            {
                var midis = Directory.GetFiles(path).Where(IsMIDIFile);
                if (midis.Any())
                    return true;
            }
            return false;
        }

        public static bool IsMIDIFile(string path)
        {
            if (!string.Equals(Path.GetExtension(path),
                ".mid",
                StringComparison.OrdinalIgnoreCase))
                return false;
            try
            {
                _ = MidiFile.Read(path);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
