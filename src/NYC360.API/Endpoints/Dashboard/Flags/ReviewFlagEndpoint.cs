using NYC360.Application.Features.Flags.Commands.ReviewFlag;
using NYC360.API.Models.Flags;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Flags;

public class ReviewFlagEndpoint(IMediator mediator) : Endpoint<ReviewFlagRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/flags-dashboard/posts/{FlagId:int}/review");
        Permissions(Domain.Constants.Permissions.PostFlags.TakeAction); 
    }

    public override async Task HandleAsync(ReviewFlagRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse.Failure(
                new ApiError("auth.invalidId", "Id not found")
            ), ct);
            return;
        }

        var command = new ReviewFlagCommand(userId.Value, req.FlagId, req.NewStatus, req.AdminNote);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct); 
    }
}