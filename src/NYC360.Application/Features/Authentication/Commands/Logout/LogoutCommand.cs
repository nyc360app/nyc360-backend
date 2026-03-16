using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.Logout;

public record LogoutCommand(int UserId)
    : IRequest<StandardResponse>;