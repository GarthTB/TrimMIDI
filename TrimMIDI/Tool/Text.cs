namespace TrimMIDI.Tool
{
    internal static class Text
    {
        /// <summary>
        /// 检验输入是否是正整数，包含判空
        /// </summary>
        public static bool IsPosInt(string input)
            => !string.IsNullOrWhiteSpace(input)
               && int.TryParse(input, out int num)
               && num > 0;

        /// <summary>
        /// 检验输入是否是非负浮点数，包含判空
        /// </summary>
        public static bool IsNonNegDouble(string input)
            => !string.IsNullOrWhiteSpace(input)
               && double.TryParse(input, out double num)
               && num >= 0;
    }
}
