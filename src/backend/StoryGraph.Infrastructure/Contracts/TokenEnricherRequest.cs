namespace StoryGraph.Infrastructure.Contracts;

public sealed class TokenEnricherRequest
{
    public string Sentence { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenEnricherRequest"/> class.
    /// </summary>
    public TokenEnricherRequest()
    {
        Sentence = string.Empty;
    }
}