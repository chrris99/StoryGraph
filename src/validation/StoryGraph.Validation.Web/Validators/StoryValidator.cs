using FluentValidation;
using StoryGraph.Validation.Domain;
using StoryGraph.Validation.Domain.Errors;
using StoryGraph.Validation.Web.Extensions;

namespace StoryGraph.Validation.Web.Validators;

public sealed class StoryValidator : AbstractValidator<Story>
{
    public StoryValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithError(DomainErrors.Title.Empty);
            
        RuleFor(x => x.Title)
            .MaximumLength(50)
            .WithError(DomainErrors.Title.MaxLengthExceeded);

        RuleFor(x => x.Text)
            .NotEmpty()
            .WithError(DomainErrors.Text.Empty);

        RuleFor(x => x.Text)
            .MaximumLength(1000)
            .WithError(DomainErrors.Text.MaxLengthExceeded);
    }
}