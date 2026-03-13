using BenScr.GeminiCli.Core;
using Google.GenAI.Types;

public static class Program
{
    private static User user = new User(System.Environment.UserName);

    public static async Task Main(string[] args)
    {
        EnvLoader envLoader = new EnvLoader("GEMINI_API_KEY");
        Gemini.Init(envLoader.Variable);

        Chat chat = new Chat(user);

        Gemini.SetModelIndex(7);

        while (true)
        {
            string input = Console.ReadLine();
            if (input.ToLower().Contains("end"))
                break;

            GenerateContentResponse response = await chat.RequestResponseAsync(input);
            Console.WriteLine(response.Text);
        }

        await chat.GenerateTopic();
        Console.WriteLine(chat.Topic);
    }
}