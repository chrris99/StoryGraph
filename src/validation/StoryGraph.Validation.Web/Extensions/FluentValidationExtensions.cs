using FluentValidation;
using StoryGraph.Validation.Domain.Abstractions;

namespace StoryGraph.Validation.Web.Extensions;

/// <summary>
/// Contains extension methods for fluent validation.
/// </summary>
public static class FluentValidationExtensions
{
    /// <summary>
    /// Specifies a custom domain error to use if the validation fails.
    /// </summary>
    /// <param name="rule">The current validation rule.</param>
    /// <param name="error">The domain error to use.</param>
    /// <typeparam name="T">The type being validated..</typeparam>
    /// <typeparam name="TProperty">The property being validated.</typeparam>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule,
        Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return rule.WithErrorCode(error.Code).WithMessage(error.Message);
    }
}