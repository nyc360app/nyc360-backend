using FastEndpoints;
using NYC360.API.Extensions;
using NYC360.API.Models.News;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Enums.News;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.News;

public class GetMyNewsPollsEndpoint(INewsPollService newsPollService)
    : Endpoint<GetMyNewsPollsRequest, PagedResponse<NewsPollListItemDto>>
{
    public override void Configure()
    {
        Get("/news/polls/mine");
    }

    public override async Task HandleAsync(GetMyNewsPollsRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        NewsPollStatus? status = null;
        if (!string.IsNullOrWhiteSpace(req.Status))
        {
            if (!Enum.TryParse<NewsPollStatus>(req.Status, true, out var parsed))
            {
                await Send.OkAsync(PagedResponse<NewsPollListItemDto>.Failure(
                    new ApiError("news.polls.invalid_status", "Invalid poll status filter.")), ct);
                return;
            }

            status = parsed;
        }

        var result = await newsPollService.GetMineAsync(userId.Value, status, req.Page, req.PageSize, ct);
        await Send.OkAsync(result, ct);
    }
}
