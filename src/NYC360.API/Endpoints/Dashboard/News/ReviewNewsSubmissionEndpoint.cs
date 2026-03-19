using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.News;
using NYC360.Application.Features.News.Commands.ReviewSubmission;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.News;

public class ReviewNewsSubmissionEndpoint(IMediator mediator)
    : Endpoint<ReviewNewsSubmissionRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/news-dashboard/submissions/review");
    }

    public override async Task HandleAsync(ReviewNewsSubmissionRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(
            new ReviewNewsSubmissionCommand(userId.Value, req.PostId, req.Approved, req.ModerationNote), ct);

        await Send.OkAsync(result, ct);
    }
}
