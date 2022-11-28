namespace StoryGraph.Domain.Utilities;

public class TupleComparer<T> : IEqualityComparer<Tuple<T, T>>
{
    private readonly IEqualityComparer<T> _comparer;
    
    public TupleComparer(IEqualityComparer<T>? comparer = default)
    {
        _comparer = comparer ?? EqualityComparer<T>.Default;
    }

    public bool Equals(Tuple<T, T>? x, Tuple<T, T>? y)
    {
        return _comparer.Equals(x.Item1, y.Item1) && _comparer.Equals(x.Item2, y.Item2) ||
               _comparer.Equals(x.Item1, y.Item2) && _comparer.Equals(x.Item2, y.Item1);
    }

    public int GetHashCode(Tuple<T, T>? obj)
    {
        return _comparer.GetHashCode(obj.Item1) ^ _comparer.GetHashCode(obj.Item2);
    }
}