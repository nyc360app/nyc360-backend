using NYC360.Application.Features.Authentication.Commands.Login;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(string Token) : IRequest<StandardResponse<LoginResponse>>;
