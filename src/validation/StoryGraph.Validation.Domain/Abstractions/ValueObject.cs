namespace StoryGraph.Validation.Domain.Abstractions;

/// <summary>
/// Abstract base class for value objects.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// Gets the atomic values of the value object.
    /// </summary>
    /// <returns>The collection of objects representing the value object values.</returns>
    protected abstract IEnumerable<object> GetValues();

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        
        return obj is ValueObject valueObject && GetValues().SequenceEqual(valueObject.GetValues());
    }
    
    /// <inheritdoc />
    public bool Equals(ValueObject? other)
    {
        return other is not null && GetValues().SequenceEqual(other.GetValues());
    }

    public static bool operator ==(ValueObject? a, ValueObject? b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject? a, ValueObject? b) => !(a == b);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        HashCode hashCode = default;
        
        foreach (var obj in GetValues())
        {
            hashCode.Add(obj);
        }

        return hashCode.ToHashCode();
    }
}