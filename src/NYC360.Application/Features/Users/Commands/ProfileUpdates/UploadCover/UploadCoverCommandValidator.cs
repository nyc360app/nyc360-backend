using FluentValidation;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.UploadCover;

public class UploadCoverCommandValidator: AbstractValidator<UploadCoverCommand>
{
    public UploadCoverCommandValidator()
    {
        RuleFor(x => x.File).NotNull();
        
        RuleFor(x => x.File.Length)
            .LessThanOrEqualTo(10 * 1024 * 1024) // 10MB Limit
            .WithMessage("Cover image size must be less than 10MB.");

        RuleFor(x => x.File.ContentType)
            .Must(x => x.Equals("image/jpeg") || x.Equals("image/png") || x.Equals("image/webp"))
            .WithMessage("Only JPG, PNG and WebP images are allowed.");
    }
}