namespace StoryGraph.Domain.Events;

public sealed class NodeDetectedEvent
{
    public string Id { get; set; }
    public string Label { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NodeDetectedEvent"/> class.
    /// </summary>
    public NodeDetectedEvent()
    {
        Id = string.Empty;
        Label = string.Empty;
    }
}