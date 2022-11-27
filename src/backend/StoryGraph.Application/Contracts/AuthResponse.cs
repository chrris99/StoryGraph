namespace StoryGraph.Application.Contracts;

public sealed class AuthResponse
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Token { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthResponse"/> class.
    /// </summary>
    public AuthResponse()
    {
        Email = string.Empty;
        Name = string.Empty;
        Token = string.Empty;
    }
}