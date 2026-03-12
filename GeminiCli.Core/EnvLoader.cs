using DotNetEnv;

namespace BenScr.GeminiCli.Core;

public class EnvLoader
{
    public string Variable;

    public EnvLoader(string name)
    {
        Env.Load();
        Variable = Environment.GetEnvironmentVariable(name) ?? throw new Exception($"Environment variable not found ({name})");
    }
}