using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json.Serialization;
using StoryGraph.Application.Services;
using StoryGraph.Domain.Models;
using StoryGraph.Infrastructure.Contracts;

namespace StoryGraph.Infrastructure.Language;

public sealed class NamedEntityRecognizer : INamedEntityRecognizer
{
    private const string Endpoint = "/extract_person_entities";
    
    private readonly IHttpClientFactory _httpClientFactory;
    
    public NamedEntityRecognizer(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<IEnumerable<string>> GetEntitiesAsync(IEnumerable<Token> tokens)
    {
        var httpClient = _httpClientFactory.CreateClient(nameof(NamedEntityRecognizer));

        var serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            Formatting = Formatting.Indented
        };

        var payload = new StringContent(JsonConvert.SerializeObject(new NamedEntityRecognizerRequest
        {
            Tokens = tokens
        }, serializerSettings), Encoding.UTF8, "application/json");
        
        using var response = await httpClient.PostAsync(Endpoint, payload);

        response.EnsureSuccessStatusCode();

        var entities = await response
            .Content
            .ReadFromJsonAsync<IEnumerable<string>>();

        return entities;
    }
}