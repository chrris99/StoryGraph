using StoryGraph.Domain.Abstractions;

namespace StoryGraph.Domain.Entities;

public sealed class Story : Entity, IAuditableEntity
{
    public string Title { get; set; }
    public string Text { get; set; }
    
    public string CreatedBy { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Story"/> class.
    /// </summary>
    /// <param name="id"></param>
    public Story(Guid id) : base(id) { }
}