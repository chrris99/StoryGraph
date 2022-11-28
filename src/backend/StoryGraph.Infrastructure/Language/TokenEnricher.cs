using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json.Serialization;
using StoryGraph.Application.Services;
using StoryGraph.Domain.Models;
using StoryGraph.Infrastructure.Contracts;

namespace StoryGraph.Infrastructure.Language;

public sealed class TokenEnricher : ITokenEnricher
{
    private const string Endpoint = "/enrich_tokens";
    
    private readonly IHttpClientFactory _httpClientFactory;
    
    public TokenEnricher(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<IEnumerable<Token>> GetTokensAsync(string sentence)
    {
        var httpClient = _httpClientFactory.CreateClient(nameof(TokenEnricher));

        var serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            Formatting = Formatting.Indented
        };
        
        var payload = new StringContent(JsonConvert.SerializeObject(new TokenEnricherRequest
        {
            Sentence = sentence
        }, serializerSettings), Encoding.UTF8, "application/json");

        using var response = await httpClient.PostAsync(Endpoint, payload);

        response.EnsureSuccessStatusCode();

        var tokens = await response
            .Content
            .ReadFromJsonAsync<IEnumerable<Token>>();

        return tokens;
    }
}