
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
}

