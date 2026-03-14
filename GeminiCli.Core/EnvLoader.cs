using DotNetEnv;

namespace BenScr.GeminiCli.Core;

public class EnvLoader
{
    public string Variable;

    public static void Load()
    {
        Env.Load();
    }

    public EnvLoader(string name)
    {
        Variable = Environment.GetEnvironmentVariable(name) ?? throw new Exception($"Environment variable not found ({name})");
    }
}