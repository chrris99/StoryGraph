namespace StoryGraph.Domain.Abstractions;

public interface IAuditableEntity
{
    string CreatedBy { get; set; }
    DateTime CreatedOnUtc { get; set; }
    DateTime? ModifiedOnUtc { get; set; }
}