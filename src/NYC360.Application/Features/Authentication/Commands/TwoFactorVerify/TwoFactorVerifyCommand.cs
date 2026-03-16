using NYC360.Application.Features.Authentication.Commands.Login;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.TwoFactorVerify;

public record TwoFactorVerifyCommand(string Email, string Code) : IRequest<StandardResponse<LoginResponse>>;
