using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Users.Commands.Dashboard.Delete;

public record DeleteUserCommand(int UserId) : IRequest<StandardResponse>;