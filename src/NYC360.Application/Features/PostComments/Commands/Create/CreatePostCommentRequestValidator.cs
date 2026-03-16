using FluentValidation;

namespace NYC360.Application.Features.PostComments.Commands.Create;

public class CreatePostCommentRequestValidator : AbstractValidator<CreatePostCommentCommand>
{
    public CreatePostCommentRequestValidator()
    {
        RuleFor(x => x.PostId).GreaterThan(0);
        RuleFor(x => x.Content).NotEmpty();
    }
}