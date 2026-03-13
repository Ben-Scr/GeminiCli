
namespace BenScr.GeminiCli.Core;

public class User
{
    public string Name { get; private set; }
    public string Role { get; private set; }

    public User(string name, string role = "user")
    {
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }
}

