using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Commands.UserDelete;

public sealed record PostDeleteUserCommand(int UserId, int PostId) : IRequest<StandardResponse>;