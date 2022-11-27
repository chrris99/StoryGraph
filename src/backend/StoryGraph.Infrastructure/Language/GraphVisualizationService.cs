using Microsoft.AspNetCore.SignalR;

using StoryGraph.Application.Hubs;
using StoryGraph.Application.Services;
using StoryGraph.Domain.Abstractions;
using StoryGraph.Domain.Events;

namespace StoryGraph.Infrastructure.Language;

public sealed class GraphVisualizationService : IVisualizationService
{
    private readonly ISentenceSplitter _sentenceSplitter;
    private readonly ITokenEnricher _tokenEnricher;
    private readonly INamedEntityRecognizer _namedEntityRecognizer;

    private readonly IHubContext<StoryHub, IStoryHub> _hubContext;

    public GraphVisualizationService(
        ISentenceSplitter sentenceSplitter,
        ITokenEnricher tokenEnricher,
        INamedEntityRecognizer namedEntityRecognizer,
        IHubContext<StoryHub, IStoryHub> hubContext)
    {
        _sentenceSplitter = sentenceSplitter;
        _tokenEnricher = tokenEnricher;
        _namedEntityRecognizer = namedEntityRecognizer;

        _hubContext = hubContext;
    }

    public async Task<Result> VisualizeAsync(string text)
    {
        var sentences = await _sentenceSplitter.SplitAsync(text);

        foreach (var sentence in sentences)
        {
            var tokens = await _tokenEnricher.GetTokensAsync(sentence);
            var entities = await _namedEntityRecognizer.GetEntitiesAsync(tokens);

            PublishEvents(entities);
        }

        return Result.Success();
    }

    private async void PublishEvents(IEnumerable<string> entities)
    {
        var entityList = entities.ToList();
        
        if (!entityList.Any()) return;
        
        // First publish all detected nodes
        foreach (var entity in entityList)
        {
            await _hubContext.Clients.All.NodeDetected(new NodeDetectedEvent
            {
                Id = new Guid().ToString(),
                Label = entity
            });
        }
        
        // TODO Then publish a detected edge between every detected node
    }
}