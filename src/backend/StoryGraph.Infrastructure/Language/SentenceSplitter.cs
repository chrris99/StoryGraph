using System.Net.Http.Json;
using StoryGraph.Application.Services;

namespace StoryGraph.Infrastructure.Language;

public sealed class SentenceSplitter : ISentenceSplitter
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    public SentenceSplitter(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<IEnumerable<string>> SplitAsync(string text)
    {
        var httpClient = _httpClientFactory.CreateClient();
        
        const string endpoint = "http://localhost:5002/split_sentences";

        using var response = await httpClient.PostAsJsonAsync(endpoint, new
        {
            document = text
        });

        response.EnsureSuccessStatusCode();

        var sentences = await response
            .Content
            .ReadFromJsonAsync<IEnumerable<string>>();

        return sentences;
    }
}