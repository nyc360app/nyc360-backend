using NYC360.Domain.Enums.Tags;
using FluentValidation;

namespace NYC360.Application.Features.Tags.Commands.Create;

public class CreateTagValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tag name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Type).IsInEnum();

        // Business Rule: Interest tags must have a Division assigned
        RuleFor(x => x.Division)
            .NotNull()
            .When(x => x.Type == TagType.Interest)
            .WithMessage("Interest tags must be assigned to a Division (e.g., Housing or Transportation).");
    }
}