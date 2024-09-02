using Note = Melanchall.DryWetMidi.Interaction.Note;

namespace TrimMIDI.Tool
{
    internal static class NoteProc
    {
        /// <summary>
        /// 缩短离下一个音符小于指定间隔的音符
        /// </summary>
        /// <returns> 修改的音符数 </returns>
        public static int ShortenNotes(IEnumerable<Note> notes, int minGap)
        {
            int count = 0;
            foreach (var note in notes)
            {
                var nextNote = GetNextNote(notes, note);
                if (nextNote is null)
                    continue;

                long maxLen = GetMaxLen(minGap, note, nextNote);
                if (note.Length <= maxLen)
                    continue;

                note.Length = maxLen;
                count++;
            }
            return count;
        }

        private static Note? GetNextNote(IEnumerable<Note> notes, Note note)
            => notes.Where(n => n.NoteNumber == note.NoteNumber && n.Time > note.Time)
                    .OrderBy(n => n.Time)
                    .FirstOrDefault();

        private static long GetMaxLen(int minGap, Note note, Note nextNote)
        {
            var maxLen1 = nextNote.Time - note.Time - minGap;
            var maxLen2 = (nextNote.Time - note.Time) / 2;
            return Math.Max(maxLen1, maxLen2);
        }

        /// <summary>
        /// 给[pitch1, pitch2)的音一个线性插值的力度增益
        /// </summary>
        public static void VelGradGain(IEnumerable<Note> notes, int pitch1, int pitch2, double gain1, double gain2)
        {
            if (gain1 == 1 && gain2 == 1)
                return;
            double GetGain(Note n)
                => gain1 + (gain2 - gain1) * ((n.NoteNumber - pitch1) / (pitch2 - pitch1));
            var notesToGain = notes.Where(
                n => n.NoteNumber >= pitch1 && n.NoteNumber < pitch2);
            foreach (var note in notesToGain)
                note.GainBy(GetGain(note));
        }
    }
}
