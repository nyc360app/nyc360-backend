using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Posts.Commands.SavePost;

public record SavePostCommand(
    int UserId, 
    int PostId
) : IRequest<StandardResponse<int>>;
