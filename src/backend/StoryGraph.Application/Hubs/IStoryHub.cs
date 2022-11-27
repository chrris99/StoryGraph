using StoryGraph.Domain.Events;

namespace StoryGraph.Application.Hubs;

public interface IStoryHub
{
    Task NodeDetected(NodeDetectedEvent @event);

    Task EdgeDetected(EdgeDetectedEvent @event);
}