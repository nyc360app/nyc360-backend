using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.Communities;
using NYC360.Application.Features.Communities.Dashboard.ReviewLeaderApplication;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Communities.Dashboard;

public class ReviewLeaderApplicationEndpoint(IMediator mediator)
    : Endpoint<ReviewCommunityLeaderApplicationRequest, StandardResponse<CommunityLeaderApplicationAdminDetailsDto>>
{
    public override void Configure()
    {
        Put("/communities-dashboard/leader-applications/{ApplicationId}/review");
        Roles("Admin", "SuccessAdmin", "SuperAdmin");
    }

    public override async Task HandleAsync(ReviewCommunityLeaderApplicationRequest req, CancellationToken ct)
    {
        var adminUserId = User.GetId();
        if (adminUserId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var applicationId = Route<int>("ApplicationId");
        var notes = !string.IsNullOrWhiteSpace(req.AdminNotes) ? req.AdminNotes : req.AdminComment;

        var command = new ReviewCommunityLeaderApplicationCommand(
            applicationId,
            adminUserId.Value,
            req.Approved,
            notes);

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}
