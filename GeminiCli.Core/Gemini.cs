using Google.GenAI;
using Google.GenAI.Types;
using Sprache;
using System.Threading.Tasks;

namespace BenScr.GeminiCli.Core
{
    public static class Gemini
    {
        public static string ApiKey;
        public static Client Client {  get; private set; }

        public static List<string> Models { get; private set; } = new List<string>()
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

        public static int selectedModelIndex = 0;
        private static string selectedModel => Gemini.Models[selectedModelIndex];

        public static void Init(string apiKey)
        {
            ApiKey = apiKey;
            Client = new Client(apiKey: ApiKey);
        }

        public static async Task<bool> AddModel(string model)
        {
            if(await ModelExistsAsync(Client, model))
            {
                return true;
            }
            return false;
        }

        private static async Task<bool> ModelExistsAsync(Client client, string modelName)
        {
            try
            {
                Model model = await client.Models.GetAsync(model: modelName);
                return model != null;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<GenerateContentResponse> RequestAsync(List<Content> contents)
        {
            if (contents == null || contents.Count == 0)
                throw new ArgumentNullException("Contents can't be null or empty");


            Console.WriteLine("Selected Model: " + selectedModel);

            GenerateContentResponse response = await Client.Models.GenerateContentAsync(model: selectedModel, contents: contents);
            return response;
        }

        public static async Task<GenerateContentResponse> RequestAsync(List<Content> contents, string model)
        {
            if (contents == null || contents.Count == 0)
                throw new ArgumentNullException("Contents can't be null or empty");
            if (string.IsNullOrEmpty(model))
                throw new ArgumentNullException("Model can't be null or empty");

            Client client = new Client(apiKey: ApiKey);

            if (!await ModelExistsAsync(client, model))
            {
                return null;
            }

            GenerateContentResponse response = await client.Models.GenerateContentAsync(model: selectedModel, contents: contents);
            return response;
        }

        public static async Task<string> GetAnswer(string question)
        {
            List<Content> contents = new List<Content>
            {
                new Content
                {
                Role = "user",
                Parts = new List<Part>
                {
                  new Part { Text = question }
                }
                }
            };
            GenerateContentResponse response = await RequestAsync(contents);
            return response?.Text ?? "Error occured";
        }
        public static async Task<string> GetAnswer(List<Content> contents)
        {
            GenerateContentResponse response = await RequestAsync(contents);
            return response.Text;
        }
    }
}
