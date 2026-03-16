using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.Login;

public record LoginCommand(string Email, string Password) 
    : IRequest<StandardResponse<LoginResponse>>;
