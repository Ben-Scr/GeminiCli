using Google.GenAI.Types;
using System.Linq.Expressions;

namespace BenScr.GeminiCli.Core;

public class Chat
{
    public string Topic { get; set; }
    public User User;

    public List<Content> History = new List<Content>();

    public Chat(User user, string topic = "No topic")
    {
        User = user;
        Topic = topic;
    }

    public void AddContent(string input, bool isUser)
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

    public void RemoveLastContent()
    {
        if (History.Count == 0) throw new Exception("History is empty");

        History.RemoveAt(History.Count - 1);
    }

    public async Task GenerateTopic()
    {
        RangeInt rangeInt = new RangeInt(2, 5);

        GenerateContentResponse response = await this.RequestResponseAsync($"Write a short topic about this chat within {rangeInt} words.");
        Topic = response.Text;
    }

    public override string ToString()
    {
        return Topic;
    }
}
