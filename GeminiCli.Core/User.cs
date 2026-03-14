
namespace BenScr.GeminiCli.Core;

public class User
{
    public string Name { get; set; }
    public string Role { get; set; }

    public User(string name, string role = "user")
    {
        Name = name;
        Role = role;
    }

    public override string ToString()
    {
        return Name;
    }
}

