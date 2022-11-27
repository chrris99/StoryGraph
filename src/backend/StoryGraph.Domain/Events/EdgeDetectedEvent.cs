namespace StoryGraph.Domain.Events;

public sealed class EdgeDetectedEvent
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string Source { get; set; }
    public string Target { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeDetectedEvent"/> class.
    /// </summary>
    public EdgeDetectedEvent()
    {
        Id = string.Empty;
        Label = string.Empty;
        Source = string.Empty;
        Target = string.Empty;
    }
}