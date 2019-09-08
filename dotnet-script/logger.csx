#load "command-line.csx"

public static class Logger
{
    public static string ProcessMessagePadding = "    ";
    public static string SuccessIcon = "\u2714";
    public static string FailureIcon = "\u274c";

    public static void LogInfo(string message)
    {
        Write(message);
    }

    public static void LogStartProcess(string message)
    {
        if (Console.OutputEncoding != System.Text.Encoding.UTF8)
            Console.OutputEncoding = System.Text.Encoding.UTF8;

        Write(ProcessMessagePadding + message, ConsoleColor.Yellow);
    }

    public static void LogFinishProcess(string message)
    {
        var width = ProcessMessagePadding.Length / 2;
        var iconText = $"{SuccessIcon}{ProcessMessagePadding.Substring(0, width)}";

        ResetLine();
        Write(iconText, ConsoleColor.Green);
        Write(message, ConsoleColor.Yellow);
        Write("\n");
    }

    public static void LogFailProcess(string processMessage, string errorMessage)
    {
        var width = ProcessMessagePadding.Length / 2;
        var iconText = $"{FailureIcon}{ProcessMessagePadding.Substring(0, width)}";

        ResetLine();
        Write(iconText, ConsoleColor.Red);
        Write(processMessage, ConsoleColor.Yellow);
        Write("\n");
        LogError(errorMessage);
    }

    public static void LogError(string message)
    {
        Write(message, ConsoleColor.Red);
    }

    public static void ResetLine()
    {
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth - 1));
        Console.SetCursorPosition(0, Console.CursorTop);
    }

    private static void Write(string message)
    {
        Console.Write(message);
    }

    private static void Write(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Write(message);
        Console.ResetColor();
    }
}