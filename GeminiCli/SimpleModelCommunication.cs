
using BenScr.GeminiCli.Core;
using Google.GenAI.Types;

namespace BenScr.GeminiCli.Program;

public static class SimpleModelCommunication
{
    private static User user = new User(System.Environment.UserName);

    public static async Task Run(string[] args)
    {
        EnvLoader envLoader = new EnvLoader("GEMINI_API_KEY");
        Gemini.Init(envLoader.Variable);

        while (true)
        {
            Console.Write($"{user.Name}> ");
            GenerateContentResponse response;
            string input = Console.ReadLine();

            while (true)
            {
                Console.Write($"{Gemini.SelectedModel}> ");

                try
                {
                    response = await Gemini.RequestAsync(input.ToContentList());
                    break;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error occured");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Gemini.NextModel();
                }
            }


            Console.WriteLine(response.Text);
        }
    }
}
