using StoryGraph.Domain.Models;

namespace StoryGraph.Application.Services;

public interface ITokenEnricher
{
    Task<IEnumerable<Token>> GetTokensAsync(string sentence);
}