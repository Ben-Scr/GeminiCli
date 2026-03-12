using BenScr.GeminiCli.Core;
using Google.GenAI;

public static class Program
{
    private static EnvLoader envLoader;

    public static async Task Main(string[] args)
    {
        envLoader = new EnvLoader("GEMINI_API_KEY");
        Console.WriteLine("Gemini Api Key: " + envLoader.Variable);
    }

    public static async Task RequestAI(string input)
    {
        Client client = new Client(apiKey: envLoader.Variable); 
    }
}