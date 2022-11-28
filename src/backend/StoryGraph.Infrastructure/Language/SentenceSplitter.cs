using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StoryGraph.Application.Services;
using StoryGraph.Infrastructure.Contracts;

namespace StoryGraph.Infrastructure.Language;

public sealed class SentenceSplitter : ISentenceSplitter
{
    private const string Endpoint = "/split_sentences";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<SentenceSplitter> _logger;

    public SentenceSplitter(IHttpClientFactory httpClientFactory, ILogger<SentenceSplitter> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
    
    public async Task<IEnumerable<string>> SplitAsync(string text)
    {
        var httpClient = _httpClientFactory.CreateClient(nameof(SentenceSplitter));
        
        var serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            Formatting = Formatting.Indented
        };

        var payload = new StringContent(JsonConvert.SerializeObject(new SentenceSplitterRequest
        {
            Document = text
        }, serializerSettings), Encoding.UTF8, "application/json");

        _logger.LogInformation("Send request to sentence splitter with {payload}", payload.ToString());
        
        using var response = await httpClient.PostAsync(Endpoint, payload);

        response.EnsureSuccessStatusCode();

        var sentences = await response
            .Content
            .ReadFromJsonAsync<IEnumerable<string>>();

        return sentences;
    }
}