namespace StoryGraph.Domain.Models;

public sealed class Token
{
    public string Text { get; set; }
    public string UniversalPos { get; set; }
    public string Lemma { get; set; }
}