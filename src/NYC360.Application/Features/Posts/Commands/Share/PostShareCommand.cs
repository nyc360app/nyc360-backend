using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Posts.Commands.Share;

public record PostShareCommand(
    int UserId, 
    int ParentPostId, 
    string? Commentary
) : IRequest<StandardResponse<PostDto>>;