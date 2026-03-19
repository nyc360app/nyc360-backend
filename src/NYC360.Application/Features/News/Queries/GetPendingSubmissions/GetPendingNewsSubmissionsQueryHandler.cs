using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.News.Queries.GetPendingSubmissions;

public class GetPendingNewsSubmissionsQueryHandler(
    IPostRepository postRepository,
    INewsAuthorizationService newsAuthorizationService)
    : IRequestHandler<GetPendingNewsSubmissionsQuery, PagedResponse<NewsSubmissionDto>>
{
    public async Task<PagedResponse<NewsSubmissionDto>> Handle(GetPendingNewsSubmissionsQuery request, CancellationToken ct)
    {
        var access = await newsAuthorizationService.GetAccessAsync(request.UserId, ct);
        if (access == null || !access.CanModerateContent)
            return PagedResponse<NewsSubmissionDto>.Create([], request.Page, request.PageSize, 0);

        var (items, total) = await postRepository.GetPendingNewsSubmissionsAsync(request.Page, request.PageSize, request.Search, ct);
        return PagedResponse<NewsSubmissionDto>.Create(items, request.Page, request.PageSize, total);
    }
}
