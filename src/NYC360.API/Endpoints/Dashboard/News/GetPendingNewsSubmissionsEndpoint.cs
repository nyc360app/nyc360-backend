using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.News;
using NYC360.Application.Contracts.Services;
using NYC360.Application.Features.News.Queries.GetPendingSubmissions;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.News;

public class GetPendingNewsSubmissionsEndpoint(
    IMediator mediator,
    INewsAuthorizationService newsAuthorizationService)
    : Endpoint<GetPendingNewsSubmissionsRequest, PagedResponse<NewsSubmissionDto>>
{
    public override void Configure()
    {
        Get("/news-dashboard/submissions/pending");
    }

    public override async Task HandleAsync(GetPendingNewsSubmissionsRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var access = await newsAuthorizationService.GetAccessAsync(userId.Value, ct);
        if (access == null || !access.CanModerateContent)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        var result = await mediator.Send(
            new GetPendingNewsSubmissionsQuery(userId.Value, req.Page, req.PageSize, req.Search), ct);

        await Send.OkAsync(result, ct);
    }
}
