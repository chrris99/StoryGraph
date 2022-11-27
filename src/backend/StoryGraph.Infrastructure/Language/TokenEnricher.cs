using System.Net.Http.Json;
using StoryGraph.Application.Services;
using StoryGraph.Domain.Models;

namespace StoryGraph.Infrastructure.Language;

public sealed class TokenEnricher : ITokenEnricher
{
    private readonly HttpClient _http;
    
    public TokenEnricher(HttpClient http)
    {
        _http = http;
    }
    
    public async Task<IEnumerable<Token>> GetTokensAsync(string sentence)
    {
        const string endpoint = "/enrich_tokens";

        using var response = await _http.PostAsJsonAsync(endpoint, new
        {
            sentence
        });

        response.EnsureSuccessStatusCode();

        var tokens = await response
            .Content
            .ReadFromJsonAsync<IEnumerable<Token>>();

        return tokens;
    }
}