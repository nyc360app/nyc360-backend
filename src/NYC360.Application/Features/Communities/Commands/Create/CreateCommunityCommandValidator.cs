using Microsoft.AspNetCore.Http;
using FluentValidation;

namespace NYC360.Application.Features.Communities.Commands.Create;

public class CreateCommunityCommandValidator : AbstractValidator<CreateCommunityCommand>
{
    public CreateCommunityCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(1000);

        RuleFor(x => x.Rules)
            .MaximumLength(4000)
            .When(x => !string.IsNullOrWhiteSpace(x.Rules));

        RuleFor(x => x.Slug)
            .Matches("^[a-z0-9-]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.Slug))
            .WithMessage("Slug can only contain lowercase letters, numbers and hyphens.");

        RuleFor(x => x.CategoryCode)
            .Must(c => new[] { "A", "B", "C", "D", "E" }.Contains(c))
            .When(x => !string.IsNullOrWhiteSpace(x.CategoryCode))
            .WithMessage("CategoryCode must be one of: A, B, C, D, E.");

        RuleFor(x => x.DivisionTag)
            .Must(d => new[] { "community", "culture", "education", "health", "housing", "lifestyle", "legal", "news", "professions", "social", "transportation", "tv" }.Contains(d))
            .When(x => !string.IsNullOrWhiteSpace(x.DivisionTag))
            .WithMessage("DivisionTag must be one of the allowed values.");

        RuleFor(x => x.Borough)
            .NotEmpty()
            .Must(b => new[] { "Citywide", "Manhattan", "Brooklyn", "Queens", "Bronx", "Staten Island" }.Contains(b))
            .WithMessage("Borough must be one of: Citywide, Manhattan, Brooklyn, Queens, Bronx, Staten Island.");

        RuleFor(x => x.Neighborhood)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .Matches(@"^\d{5}$")
            .WithMessage("ZipCode must be a 5-digit US zip code.");

        RuleFor(x => x.AvatarImage)
            .Must(BeValidImage!)
            .When(x => x.AvatarImage != null);

        RuleFor(x => x.CoverImage)
            .Must(BeValidImage!)
            .When(x => x.CoverImage != null);
    }

    private static bool BeValidImage(IFormFile file)
    {
        var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
        return allowed.Contains(file.ContentType) && file.Length <= 5_000_000;
    }
}
