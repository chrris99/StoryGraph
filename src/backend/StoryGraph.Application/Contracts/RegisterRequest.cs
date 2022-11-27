namespace StoryGraph.Application.Contracts;

public sealed class RegisterRequest : AuthRequest
{
    public string Name { get; set; }

    public RegisterRequest()
    {
        Name = string.Empty;
    }
}