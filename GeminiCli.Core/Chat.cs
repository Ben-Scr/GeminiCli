using Google.GenAI;
using Google.GenAI.Types;
using System.Reflection.Metadata;

namespace BenScr.GeminiCli.Core;

public class Chat
{
    public string Topic { get; set; }
    public User User { get; set; }

    public List<Content> History { get; private set; } = new List<Content>();

    public Chat(User user)
    {
        User = user;
    }

    public void AddContent(string input, bool isUser = false)
    {
        History.Add(new Content
        {
            Role = isUser ? User.Role : "model",
            Parts = new List<Part>
                {
                  new Part { Text = input }
                }
        });
    }
}
