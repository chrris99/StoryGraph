namespace StoryGraph.Validation.Domain.Abstractions;

/// <summary>
/// Represents the result of an operation with status information and possibly an error.
/// </summary>
public class Result
{
    /// <summary>
    /// Gets a value indicating whether the result is a success result.
    /// </summary>
    public bool IsSuccess { get; }
    
    /// <summary>
    /// Gets a value indicating whether the result is a failure result.
    /// </summary>
    public bool IsFailure => !IsSuccess;
    
    /// <summary>
    /// Gets the error.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="isSuccess">The flag indicating the status of the result.</param>
    /// <param name="error">The error.</param>
    /// <exception cref="InvalidOperationException">Thrown if there is a contradiction between the <paramref name="error"/>
    /// and the <paramref name="isSuccess"/> parameters.</exception>
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException();

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Returns a success <see cref="Result"/>.
    /// </summary>
    /// <returns>A new <see cref="Result"/> instance with the success flag set and an empty error.</returns>
    public static Result Success() => new(true, Error.None);
    
    /// <summary>
    /// Returns a success <see cref="Result{TValue}"/> with the specified value.
    /// </summary>
    /// <param name="value">The result value.</param>
    /// <typeparam name="TValue">The result type.</typeparam>
    /// <returns>A new <see cref="Result{TValue}"/> instance with the success flag set and an empty error.</returns>
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    
    /// <summary>
    /// Returns a failure <see cref="Result"/> with the specified error.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <returns>A new <see cref="Result"/> instance with the failure flag set and the specified error.</returns>
    public static Result Failure(Error error) => new(false, error);
    
    /// <summary>
    /// Returns a failure <see cref="Result{TValue}"/> with the specified error.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <typeparam name="TValue">The result type.</typeparam>
    /// <returns>A new <see cref="Result{TValue}"/> instance with the failure flag set and the specified error.</returns>
    public static Result<TValue> Failure<TValue>(Error error) => new(default!, false, error);
}

/// <summary>
/// Represents the result of an operation with a return value, status information and possibly an error.
/// </summary>
/// <typeparam name="TValue">The result value type.</typeparam>
public sealed class Result<TValue> : Result
{
    private readonly TValue _value;
    
    /// <summary>
    /// Gets the result value if the result is successful.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when <see cref="Result.IsFailure"/> is true.</exception>
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValue}"/> class.
    /// </summary>
    /// <param name="value">The result value.</param>
    /// <param name="isSuccess">The flag indicating the status of the result.</param>
    /// <param name="error">The error.</param>
    protected internal Result(TValue value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }
    
    public static implicit operator Result<TValue>(TValue value) => Success(value);
}