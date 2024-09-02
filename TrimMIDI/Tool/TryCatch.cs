using TrimMIDI.Tool;

/// <summary>
/// 为简化代码，将TryCatch块封装成一个类
/// </summary>
internal static class TryCatch
{
    public static void Do(string name, Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            MsgB.OkErr($"{name}错误：\n{ex.Message}", "错误");
        }
    }
}
