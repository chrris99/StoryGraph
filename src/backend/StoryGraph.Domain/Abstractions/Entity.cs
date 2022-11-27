namespace StoryGraph.Domain.Abstractions;

/// <summary>
/// Abstract base class for entities.
/// </summary>
public abstract class Entity : IEquatable<Entity>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Entity"/> class.
    /// </summary>
    /// <param name="id">The entity identifier.</param>
    protected Entity(Guid id)
    {
        Id = id;
    }
    
    /// <summary>
    /// Gets the entity identifier.
    /// </summary>
    public Guid Id { get; private init; }

    public static bool operator ==(Entity? first, Entity? second)
    {
        if (ReferenceEquals(null, first)) return false;
        if (ReferenceEquals(null, second)) return false;

        return first.Equals(second);
    }

    public static bool operator !=(Entity? first, Entity? second)
    {
        return !(first == second);
    }

    /// <inheritdoc />
    public bool Equals(Entity? other)
    {
        if (ReferenceEquals(null, other)) return false;
        
        return Id.Equals(other.Id);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (obj.GetType() != GetType()) return false;
        if (obj.GetType() != typeof(Entity)) return false;
        
        return Equals(obj as Entity);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}