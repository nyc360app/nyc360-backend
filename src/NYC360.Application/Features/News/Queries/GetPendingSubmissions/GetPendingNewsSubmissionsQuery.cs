using MediatR;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.News.Queries.GetPendingSubmissions;

public record GetPendingNewsSubmissionsQuery(
    int UserId,
    int Page,
    int PageSize,
    string? Search
) : IRequest<PagedResponse<NewsSubmissionDto>>;
