using System.Net.Http.Json;

using StoryGraph.Application.Services;
using StoryGraph.Domain.Models;

namespace StoryGraph.Infrastructure.Language;

public sealed class NamedEntityRecognizer : INamedEntityRecognizer
{
    private readonly HttpClient _http;
    
    public NamedEntityRecognizer(HttpClient http)
    {
        _http = http;
    }
    
    public async Task<IEnumerable<string>> GetEntitiesAsync(IEnumerable<Token> tokens)
    {
        const string endpoint = "/extract_person_entities";

        using var response = await _http.PostAsJsonAsync(endpoint, new
        {
            tokens
        });

        response.EnsureSuccessStatusCode();

        var entities = await response
            .Content
            .ReadFromJsonAsync<IEnumerable<string>>();

        return entities;
    }
}