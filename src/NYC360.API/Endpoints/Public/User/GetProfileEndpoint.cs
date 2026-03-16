using NYC360.Application.Features.Users.Queries.GetProfile;
using NYC360.API.Models.Users;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.User;

public class GetProfileEndpoint(IMediator mediator)
    : Endpoint<GetUserProfileRequest, StandardResponse<UserProfileDto>>
{
    public override void Configure()
    {
        Get("/users/profile/{Username}");
    }
    
    public override async Task HandleAsync(GetUserProfileRequest req, CancellationToken ct)
    {
        var query = new GetProfileQuery(req.Username);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, cancellation: ct);
    }
}