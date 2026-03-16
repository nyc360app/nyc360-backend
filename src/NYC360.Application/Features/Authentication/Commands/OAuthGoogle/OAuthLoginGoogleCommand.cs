using NYC360.Application.Features.Authentication.Commands.Login;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.OAuthGoogle;

public record OAuthLoginGoogleCommand(string IdToken)
    : IRequest<StandardResponse<LoginResponse>>;