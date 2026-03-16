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

        RuleFor(x => x.Slug)
            .Matches("^[a-z0-9-]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.Slug))
            .WithMessage("Slug can only contain lowercase letters, numbers and hyphens.");

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