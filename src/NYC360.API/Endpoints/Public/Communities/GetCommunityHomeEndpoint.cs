using NYC360.Application.Features.Communities.Queries.GetHome;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Communities;

public class GetCommunityHomeEndpoint(IMediator mediator) : Endpoint<PagedRequest, StandardResponse<CommunityHomeDto>>
{
    public override void Configure()
    {
        Get("/communities/home");
    }

    public override async Task HandleAsync(PagedRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse<CommunityHomeDto>.Failure(new ApiError("auth.unauthorized", "Please login")), ct);
            return;
        }

        var query = new GetCommunityHomeQuery(userId.Value, req.Page, req.PageSize);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}