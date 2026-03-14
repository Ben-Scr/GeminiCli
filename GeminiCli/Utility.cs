
namespace BenScr.GeminiCli.Program;

public static class Utility
{
    public static void WriteUserColored(string name, ConsoleColor consoleColor = ConsoleColor.White)
    {
        Console.ForegroundColor = consoleColor;
        Console.Write($"{name}> ");
        Console.ForegroundColor = ConsoleColor.Gray;
    }
    public static string ReadUserColored(string name, ConsoleColor consoleColor = ConsoleColor.White)
    {
        Console.ForegroundColor = consoleColor;
        Console.Write($"{name}> ");
        string? input = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        return input ?? "";
    }
    public static bool EnteredYes(string name)
    {
        string input = ReadUserColored(name).ToLower();
        while (input != "y" && input != "n")
            input = ReadUserColored(name).ToLower();

        return input == "y";
    }

    public static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, currentLineCursor);
        Console.Write(new string(' ', Console.BufferWidth - 1));
        Console.SetCursorPosition(0, currentLineCursor);
    }

    public static async Task ShowSpinner(string prefix, CancellationToken token)
    {
        string[] frames = { "|", "/", "-", "\\" };
        int i = 0;

        while (!token.IsCancellationRequested)
        {
            string text = $"\r{prefix} {frames[i++ % frames.Length]}";
            Console.Write(text);

            try
            {
                await Task.Delay(100, token);
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }
    }
}

