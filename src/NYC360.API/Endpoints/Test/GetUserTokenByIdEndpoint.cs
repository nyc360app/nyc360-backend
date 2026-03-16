using NYC360.Application.Features.Authentication.Commands.Login;
using NYC360.Application.Features.Test.GetUserTokenById;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Test;

public class GetUserTokenByIdEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse<LoginResponse>>
{
    public override void Configure()
    {
        Get("/-test/user/token/{id:int}");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var command = new GetTokenByIdCommand(id.ToString());
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}