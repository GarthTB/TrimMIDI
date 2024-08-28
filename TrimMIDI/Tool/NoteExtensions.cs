using Melanchall.DryWetMidi.Common;
using Note = Melanchall.DryWetMidi.Interaction.Note;

namespace TrimMIDI.Tool
{
    internal static class NoteExtensions
    {
        /// <summary>
        /// 给予Note力度增益，自带限幅
        /// </summary>
        public static void GainBy(this Note note, double gain)
        {
            // 给予增益
            var newVel = note.Velocity * gain;
            // 限制到0-127
            note.Velocity = (SevenBitNumber)Math.Clamp(newVel, 0, 127);
        }
    }
}
