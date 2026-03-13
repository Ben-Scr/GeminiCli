using BenScr.GeminiCli.Core;
using Google.GenAI.Types;

namespace BenScr.GeminiCli.Core
{
    public static class GeminiUtility
    {
        public static async Task<GenerateContentResponse> RequestResponseAsync(this Chat chat, string input, string model = null)
        {
            chat.AddContent(input, true);
            GenerateContentResponse response = await Gemini.RequestAsync(chat.History, model);
            chat.AddContent(response.Text, false);
            return response;
        }

        public static List<Content> ToContentList(this string input, string role = "user")
        {
            return new List<Content>
            {
                new Content
                {
                    Role = role,
                    Parts = new List<Part>
                    {
                        new Part
                        {
                            Text = input
                        }
                    }
                }
            };
        }
    }
}
