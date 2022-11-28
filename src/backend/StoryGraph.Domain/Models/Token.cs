namespace StoryGraph.Domain.Models;

public sealed class Token
{
    public string Text { get; set; }
    public string UniversalPos { get; set; }
    public string Lemma { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Token"/> class.
    /// </summary>
    public Token()
    {
        Text = string.Empty;
        UniversalPos = string.Empty;
        Lemma = string.Empty;
    }
}