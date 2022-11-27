namespace StoryGraph.Infrastructure.Authentication;

public sealed class JwtOptions
{
    public static readonly string Key = "Jwt";
    
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string Secret { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtOptions"/> class.
    /// </summary>
    public JwtOptions()
    {
        Audience = string.Empty;
        Issuer = string.Empty;
        Secret = string.Empty;
    }
}