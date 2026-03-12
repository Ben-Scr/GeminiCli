using BenScr.GeminiCli.Core;
using Google.GenAI;

public static class Program
{
    private static EnvLoader envLoader;

    private static readonly string[] Models =
{
    "gemini-2.0-flash",
    "gemini-2.0-flash-001",
    "gemini-2.0-flash-lite",
    "gemini-2.0-flash-lite-001",
    "gemini-2.5-flash",
    "gemini-2.5-flash-lite",
    "gemini-2.5-pro",
    "gemini-3-flash-preview",
    "gemini-3.1-flash-lite-preview",
    "gemini-3.1-pro-preview",
};

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