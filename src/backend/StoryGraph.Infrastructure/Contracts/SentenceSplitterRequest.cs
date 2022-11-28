namespace StoryGraph.Infrastructure.Contracts;

public sealed class SentenceSplitterRequest
{
    public string Document { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SentenceSplitterRequest"/> class.
    /// </summary>
    public SentenceSplitterRequest()
    {
        Document = string.Empty;
    }
}