using BenScr.GeminiCli.Core;
using Google.GenAI;
using Google.GenAI.Types;

public static class Program
{
    private static User user = new User(System.Environment.UserName);

    public static async Task Main(string[] args)
    {
        EnvLoader envLoader = new EnvLoader("GEMINI_API_KEY");
        Gemini.Init(envLoader.Variable);

        while (true)
        {
            Console.Write($"{user.Name}> ");
            Console.WriteLine(await Gemini.GetAnswer(Console.ReadLine()));
        }
    }
}