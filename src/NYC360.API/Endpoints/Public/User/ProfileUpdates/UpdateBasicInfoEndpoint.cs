using NYC360.Application.Features.Users.Commands.ProfileUpdates.UpdateBasicInfo;
using NYC360.API.Models.Users.ProfileUpdate;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.User.ProfileUpdates;

public class UpdateBasicInfoEndpoint(IMediator mediator) : Endpoint<UpdateBasicInfoRequest, StandardResponse>
{
    public override void Configure()
    {
        Patch("/users/profile/basic");
    }

    public override async Task HandleAsync(UpdateBasicInfoRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse.Failure(
                new ApiError("auth.invalidEmail", "Email not found")
            ), ct);
            return;
        }
        // Create the command and inject the UserId from the JWT Token
        var command = new UpdateBasicInfoCommand(
            userId.Value, 
            req.FirstName, 
            req.LastName, 
            req.Headline, 
            req.Bio, 
            req.LocationId
        );

        // Send to MediatR (Validator runs automatically)
        var result = await mediator.Send(command, ct);
        
        await Send.OkAsync(result, ct);
    }
}