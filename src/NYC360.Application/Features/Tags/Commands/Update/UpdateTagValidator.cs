using NYC360.Domain.Enums.Tags;
using FluentValidation;

namespace NYC360.Application.Features.Tags.Commands.Update;

public class UpdateTagValidator : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tag name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Type).IsInEnum();

        // 1. Business Rule: Interest tags must have a Division assigned
        // Housing (4) and Transportation (11) replacements are handled here
        RuleFor(x => x.Division)
            .NotNull()
            .When(x => x.Type == TagType.Interest)
            .WithMessage("Interest tags must be assigned to a Division (e.g., Housing or Transportation).");

        // 2. Hierarchy Rule: A tag cannot be its own parent
        RuleFor(x => x.ParentTagId)
            .NotEqual(x => x.Id)
            .WithMessage("A tag cannot be its own parent.")
            .When(x => x.ParentTagId.HasValue);
    }
}