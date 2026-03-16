using MediatR;
using NYC360.API.Models.Tags;
using NYC360.Application.Features.Verifications.Commands.RemoveTag;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;

namespace NYC360.API.Endpoints.Dashboard.Users;

public class RemoveUserTagEndpoint(IMediator mediator) : Endpoint<RemoveUserTagRequest, StandardResponse>
{
    public override void Configure()
    {
        Delete("/user-dashboard/tags/remove");
        Roles("Admin"); // Restrict to High-level Admins
    }

    public override async Task HandleAsync(RemoveUserTagRequest req, CancellationToken ct)
    {
        var adminId = User.GetId();
        if (adminId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new RemoveUserTagCommand(
            req.UserId, 
            req.TagId, 
            adminId.Value);

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}