using NYC360.Domain.Dtos.Posts;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.PostComments.Commands.Create;

public record CreatePostCommentCommand(
    int UserId,
    int PostId,
    int? ParentCommentId,
    string Content
) : IRequest<StandardResponse<PostCommentDto>>;