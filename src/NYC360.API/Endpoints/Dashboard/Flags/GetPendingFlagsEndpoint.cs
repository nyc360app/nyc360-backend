using NYC360.Application.Features.Flags.Queries.GetPendingFlags;
using NYC360.Domain.Dtos.Posts;
using NYC360.API.Models.Flags;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Flags;

public class GetPendingFlagsEndpoint(IMediator mediator) : Endpoint<PendingFlagsGetRequest, PagedResponse<PostFlagAdminDto>>
{
    public override void Configure()
    {
        Get("/flags-dashboard/posts/pending");
        Permissions(Domain.Constants.Permissions.PostFlags.View); 
    }

    public override async Task HandleAsync(PendingFlagsGetRequest req, CancellationToken ct)
    {
        var query = new GetPendingFlagsQuery(
            req.Page,
            req.PageSize
        );

        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, cancellation: ct);
    }
}