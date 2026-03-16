using MediatR;
using NYC360.Application.Features.Authentication.Commands.Login;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Test.GetUserTokenById;

public record GetTokenByIdCommand(string Id) 
    : IRequest<StandardResponse<LoginResponse>>;