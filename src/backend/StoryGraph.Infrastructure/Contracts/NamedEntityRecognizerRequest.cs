using StoryGraph.Domain.Models;

namespace StoryGraph.Infrastructure.Contracts;

public sealed class NamedEntityRecognizerRequest
{
    public IEnumerable<Token> Tokens { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NamedEntityRecognizerRequest"/> class.
    /// </summary>
    public NamedEntityRecognizerRequest()
    {
        Tokens = new List<Token>();
    }
}

