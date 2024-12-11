using System;

internal static class Logger
{
    public static void Info(object message)
    {
        Console.WriteLine($"[{Time()}] INFO: {message}");
    }

    public static void Warning(object message)
    {
        Console.WriteLine($"[{Time()}] WARN: {message}");
    }

    public static void Error(object message)
    {
        Console.WriteLine($"[{Time()}] ERROR: {message}");
    }

    public static void Error(Exception exception)
    {
        Console.WriteLine($"[{Time()}] ERROR: {exception.Message}");
    }

    private static string Time()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
