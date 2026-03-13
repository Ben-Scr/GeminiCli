using Google.GenAI.Types;

namespace BenScr.GeminiCli.Core;

public class Chat
{
    public string Topic { get; private set; }
    public User User { get; private set; }

    public List<Content> History { get; private set; } = new List<Content>();

    public Chat(User user, string topic = "")
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

    public async Task GenerateTopic(RangeInt? rangeInt = null, string model = null)
    {
        rangeInt ??= new RangeInt(2, 5);

        GenerateContentResponse response = await this.RequestResponseAsync($"Write a short topic about this chat within {rangeInt.Value} words.", model);
        Topic = response.Text;
    }
}
