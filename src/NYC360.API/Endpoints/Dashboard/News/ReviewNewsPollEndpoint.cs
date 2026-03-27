using FastEndpoints;
using NYC360.API.Extensions;
using NYC360.API.Models.News;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.News;

public class ReviewNewsPollEndpoint(INewsPollService newsPollService)
    : Endpoint<ReviewNewsPollRequest, StandardResponse<NewsPollReviewResultDto>>
{
    public override void Configure()
    {
        Put("/news-dashboard/polls/{PollId:int}/review");
    }

    public override async Task HandleAsync(ReviewNewsPollRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var pollId = Route<int>("PollId");
        var result = await newsPollService.ReviewAsync(userId.Value, pollId, req.Approved, req.AdminComment, ct);
        await Send.OkAsync(result, ct);
    }
}
