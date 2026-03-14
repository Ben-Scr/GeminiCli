using BenScr.AI.Gemini;
using Google.GenAI.Types;
using System.Text;

namespace BenScr.GeminiCli.Program;

using static Utility;

public static class Program
{
    private static GeminiClient geminiClient;
    private static readonly bool tryoutEveryModel = true;
    private static string username = System.Environment.UserName;
    private static int selectedModelIndex = 0;

    private static int attempts;
    private static List<Chat> chats = new();
    private static Chat currentChat;

   // public static async Task Main(string[] args) => await Run();

    static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        using var cts = new CancellationTokenSource();

        var spinnerTask = ShowSpinner(cts.Token);

        await Task.Delay(3000); // Simulierte Arbeit

        cts.Cancel();
        await spinnerTask;

        Console.WriteLine("\rFertig!   ");
    }

    static async Task ShowSpinner(CancellationToken token)
    {
        char[] spinner = { '⠁', '⠂', '⠄', '⠂' };
        int counter = 0;

        while (!token.IsCancellationRequested)
        {
            Console.Write($"\rLädt... {spinner[counter % spinner.Length]}");
            counter++;
            await Task.Delay(100, token).ContinueWith(_ => { });
        }
    }

    private static async Task Run()
    {
        Console.InputEncoding = Encoding.UTF8;
        char[] spinner = { '⠁', '⠂', '⠄', '⠂' };

        for (int i = 0; i < 20; i++)
        {
            Console.Write($"\rLädt... {spinner[i % spinner.Length]}");
            Thread.Sleep(100);
        }

        Console.WriteLine("\rFertig!    ");

        OnLoad();
        GeminiApiKey geminiApiKey = GeminiApiKey.LoadFromEnvironment();
        geminiClient = new GeminiClient(geminiApiKey.Key, GeminiUtility.Models[selectedModelIndex]);
        await ShowOptions();
    }

    private static void OnExit()
    {
        SaveHandler.SaveChatHistory(chats);
    }

    private static void OnLoad()
    {
        chats = SaveHandler.LoadChatHistory();
    }

    private static async Task ShowOptions()
    {
        Console.Clear();
        Console.WriteLine("Choose one of the following options:");

        bool chatsAvailable = chats.Count > 0;

        Console.WriteLine("1. Create a new Chat");

        if (!chatsAvailable)
            Console.ForegroundColor = ConsoleColor.DarkGray;

        Console.WriteLine("2. Open a Chat");

        if (!chatsAvailable)
            Console.ForegroundColor = ConsoleColor.Gray;

        Console.WriteLine("3. Exit");

        for (int attempt = 0; attempt < 5; attempt++)
        {
            if (!int.TryParse(ReadUserColored(username), out int index))
                continue;

            switch (index)
            {
                case 1:
                    await EnterChat(new Chat(geminiClient));
                    return;
                case 2 when chatsAvailable:
                    await OpenChats();
                    return;
                case 3:
                    await Exit();
                    return;
                default:
                    Console.WriteLine("Invalid option!");
                    break;
            }
        }

        await Exit();
    }

    private static async Task Exit()
    {
        Console.WriteLine("Are you sure that you want to exit? (y/n)");

        if (EnteredYes(username))
        {
            OnExit();
            System.Environment.Exit(0);
            return;
        }

        await ShowOptions();
    }

    private static async Task OpenChats()
    {
        Console.Clear();
        Console.WriteLine($"Choose a chat from 1-{chats.Count}");

        for (int i = 0; i < chats.Count; i++)
            Console.WriteLine($"{i + 1}. {chats[i]}");

        for (int attempt = 0; attempt < 5; attempt++)
        {
            if (!int.TryParse(ReadUserColored(username), out int index))
                return;

            if (index < 1 || index > chats.Count)
            {
                Console.WriteLine("Invalid option!");
                continue;
            }

            await EnterChat(chats[index - 1]);
            return;
        }

        await ShowOptions();
    }

    private static async Task EnterChat(Chat chat)
    {
        Console.Clear();
        Console.WriteLine("Enter your message - Enter \"End\" in order to go back to the options");

        if (chat.History.Count > 0)
        {
            Console.WriteLine("Would you like to load the history of the chat? (y/n)");

            if (EnteredYes(username))
            {
                Console.Clear();
                Console.WriteLine("Enter your message - Enter \"End\" in order to go back to the options");

                foreach (var history in chat.History)
                {
                    string role = history?.Role ?? "no role";
                    ConsoleColor color = role == "model" ? ConsoleColor.Gray : ConsoleColor.White;

                    WriteUserColored(role, color);
                    Console.WriteLine(history?.Parts[0].Text);

                    await Task.Delay(100);
                }
            }
        }

        while (true)
        {
            string input = ReadUserColored(username);

            if (input.Contains("end", StringComparison.OrdinalIgnoreCase))
                break;

            var response = await TryRequestAsync(() =>  chat.RequestResponseAsync(input));

            if (response != null)
                Console.WriteLine(response.Text);
        }

        Console.WriteLine("Would you like to generate a topic for this chat? (y/n)");

        if (EnteredYes(username))
        {
            bool topicGenerated = await TryRunAsync(chat.GenerateTopic);

            if (topicGenerated)
                Console.WriteLine(chat.Topic);
        }

        await Task.Delay(500);

        if (!chats.Contains(chat))
            chats.Add(chat);

        await ShowOptions();
    }

    private static async Task<GenerateContentResponse?> TryRequestAsync(Func<Task<GenerateContentResponse>> action)
    {
        int modelLength = GeminiUtility.Models.Length;

        while (true)
        {
            Console.Write($"{geminiClient.Model}> ");

            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                currentChat.RemoveLastContent();

                if (!tryoutEveryModel)
                {
                    Console.WriteLine(ex);
                    return null;
                }

                Console.WriteLine("Error occured");
                selectedModelIndex += selectedModelIndex + 1 % modelLength;
                await geminiClient.SetModelAsync(GeminiUtility.Models[selectedModelIndex]);

                attempts++;

                if (attempts >= modelLength - 1)
                {
                    Console.WriteLine("No working model found");
                    return null;
                }
            }
        }
    }

    private static async Task<bool> TryRunAsync(Func<Task> action)
    {
        while (true)
        {
            Console.Write($"{geminiClient.Model}> ");

            try
            {
                await action();
                return true;
            }
            catch (Exception ex)
            {
                currentChat.RemoveLastContent();

                if (!tryoutEveryModel)
                {
                    Console.WriteLine(ex);
                    return false;
                }

                Console.WriteLine("Error occured!");
                
                attempts++;


                if (attempts >= GeminiUtility.Models.Length - 1)
                {
                    Console.WriteLine("No working model found!");
                    return false;
                }
            }
        }
    }
}