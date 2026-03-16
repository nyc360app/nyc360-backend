using NYC360.Application.Features.Authentication.Commands.Register.NewYorker;
using NYC360.API.Models.Authentication.Register;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Authentication.Register;

public class NewYorkerRegisterEndpoint(IMediator mediator) : Endpoint<NewYorkerRegisterRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/auth/register/newyorker");
        AllowAnonymous();
    }

    public override async Task HandleAsync(NewYorkerRegisterRequest req, CancellationToken ct)
    {
        var command = new RegisterNewYorkerCommand(
            req.FirstName, 
            req.LastName!, 
            req.Username, 
            req.Email, 
            req.Password,
            req.Address,
            req.Interests,
            req.IsInterestedInVolunteering,
            req.IsOpenToAttendingLocalEvents,
            req.FollowNeighborhoodUpdates,
            req.MakeProfilePublic,
            req.DisplayNeighborhood,
            req.AllowMessagesFromVerifiedOrganizations
        );
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}