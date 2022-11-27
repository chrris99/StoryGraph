namespace StoryGraph.Application.Contracts;

public abstract class AuthRequest
{
    public string Email { get; set; }
    public string Password { get; set; }

    protected AuthRequest()
    {
        Email = string.Empty;
        Password = string.Empty;
    }
}