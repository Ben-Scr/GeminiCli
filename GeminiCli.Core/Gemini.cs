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

        public static int SelectedModelIndex { get; private set; } = 0;
        public static string SelectedModel => Gemini.Models[SelectedModelIndex];

        public static void Init(string apiKey)
        {
            ApiKey = apiKey;
            Client = new Client(apiKey: ApiKey);
        }

        public static bool SetModelIndex(int index)
        {
            SelectedModelIndex = Math.Clamp(index, 0, Models.Count);
            return index == SelectedModelIndex;
        }
        public static void NextModel()
        {
            SelectedModelIndex = (SelectedModelIndex + 1) % Models.Count;
        }
        public static void LastModel()
        {
            SelectedModelIndex = (SelectedModelIndex + SelectedModelIndex - 1) % Models.Count;
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

        public static async Task<GenerateContentResponse> RequestAsync(List<Content> contents, string model = null)
        {
            if (contents == null || contents.Count == 0)
                throw new ArgumentNullException("Contents can't be null or empty");

            model ??= SelectedModel;

            if (!await ModelExistsAsync(Client, model))
                throw new ArgumentException($"Model doesn't exist ({model})");

            GenerateContentResponse response = await Client.Models.GenerateContentAsync(model: model, contents: contents);
            return response;
        }
    }
}
