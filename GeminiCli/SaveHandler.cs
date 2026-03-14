using BenScr.AI.Gemini;
using BenScr.IO;
using BenScr.Serialization;

namespace BenScr.GeminiCli.Program;

public static class SaveHandler
{
    public static string MainDirPath { get; } = Path.Combine(FileManager.LocalAppFolder, "BenScr", "GeminiCli");
    public static string UserDirName { get; } = "UserData";
    public static string ChatFileName { get; } = "Chats.json";

    public static string ChatFilePath => Path.Combine(MainDirPath, UserDirName, ChatFileName);

    public static void SaveChatHistory(List<Chat> chats)
    {
        Json.SaveCompressed(ChatFilePath, chats);
    }

    public static List<Chat> LoadChatHistory()
    {
        return Json.LoadCompressed(ChatFilePath, new List<Chat>());
    }
}

