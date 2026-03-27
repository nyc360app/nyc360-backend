using FastEndpoints;
using NYC360.API.Extensions;
using NYC360.API.Models.News;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.News;

public class GetPendingNewsPollsEndpoint(INewsPollService newsPollService)
    : Endpoint<GetPendingNewsPollsRequest, PagedResponse<NewsPollAdminPendingDto>>
{
    public override void Configure()
    {
        Get("/news-dashboard/polls/pending");
    }

    public override async Task HandleAsync(GetPendingNewsPollsRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var result = await newsPollService.GetPendingAsync(userId.Value, req.Page, req.PageSize, req.Search, ct);
        await Send.OkAsync(result, ct);
    }
}
