using Microsoft.AspNetCore.SignalR;

using StoryGraph.Application.Hubs;
using StoryGraph.Application.Services;
using StoryGraph.Domain.Abstractions;
using StoryGraph.Domain.Events;
using StoryGraph.Domain.Utilities;

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
        var entityList = entities
            .Distinct()
            .ToList();
        
        if (!entityList.Any()) return;

        foreach (var entity in entityList)
        {
            await _hubContext.Clients.All.NodeDetected(new NodeDetectedEvent
            {
                Id = entity,
                Label = entity
            });
        }

        var combinations = entityList
            .SelectMany(_ => entityList, Tuple.Create)
            .Where(tuple => tuple.Item1 != tuple.Item2)
            .Distinct(new TupleComparer<string>())
            .ToList();

        foreach (var edge in combinations)
        {
            await _hubContext.Clients.All.EdgeDetected(new EdgeDetectedEvent
            {
                Source = edge.Item1,
                Target = edge.Item2
            });
        }
    }
}