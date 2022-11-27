using StoryGraph.Domain.Models;

namespace StoryGraph.Application.Services;

public interface INamedEntityRecognizer
{
    Task<IEnumerable<string>> GetEntitiesAsync(IEnumerable<Token> tokens);
}