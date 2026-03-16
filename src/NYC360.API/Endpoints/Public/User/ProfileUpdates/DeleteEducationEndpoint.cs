using NYC360.Application.Features.Users.Commands.ProfileUpdates.DeleteEducation;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.User.ProfileUpdates;

public class DeleteEducationEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Delete("/users/profile/educations/{educationId}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(new DeleteEducationCommand(userId.Value, Route<int>("educationId")), ct);
        await Send.OkAsync(result, ct);
    }
}