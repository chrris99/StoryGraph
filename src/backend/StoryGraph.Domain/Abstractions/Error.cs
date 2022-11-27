namespace StoryGraph.Domain.Abstractions;

/// <summary>
/// Represents a domain error object.
/// </summary>
public class Error : ValueObject
{
    /// <summary>
    /// Gets the error code.
    /// </summary>
    public string Code { get; }
    
    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    /// <summary>
    /// Empty error instance.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    public static implicit operator string(Error error) => error.Code;
    
    /// <inheritdoc />
    protected override IEnumerable<object> GetValues()
    {
        yield return Code;
        yield return Message;
    }
}