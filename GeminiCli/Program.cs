using BenScr.GeminiCli.Core;
using Google.GenAI.Types;

public static class Program
{
    private static User user = new User(System.Environment.UserName);

    private static bool tryoutEveryModel = true;
    private static int tries = 0;


    public static async Task Main(string[] args)
    {
        EnvLoader envLoader = new EnvLoader("GEMINI_API_KEY");
        Gemini.Init(envLoader.Variable);

        Chat chat = new Chat(user);


        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{user.Name}> ");
            string input = Console.ReadLine();
            if (input.ToLower().Contains("end"))
                break;


            Console.ForegroundColor = ConsoleColor.Gray;

            GenerateContentResponse response = null;

            while (true)
            {
                Console.Write($"{Gemini.SelectedModel}> ");

                try
                {
                    response = await chat.RequestResponseAsync(input);
                    Console.WriteLine(response.Text);
                    break;
                }
                catch (Exception ex)
                {
                    if (!tryoutEveryModel)
                    {
                        Console.WriteLine(ex);
                        break;
                    }

                    Console.WriteLine("Error occured");
                    Gemini.NextModel();
                    ++tries;

                    if (tries >= Gemini.Models.Count - 1)
                    {
                        Console.WriteLine("No working model found");
                        break;
                    }
                }
            }
        }

        Console.WriteLine("Would you like to generate a topic for this chat? (y/n)");

        if (Console.ReadLine().ToLower() == "y")
        {
            await chat.GenerateTopic();
            Console.WriteLine($"{Gemini.SelectedModel}> " + chat.Topic);
        }
    }
}