using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Professions;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Queries.SearchJobs;

public class SearchJobsHandler(IProfessionsRepository repo) 
    : IRequestHandler<SearchJobsQuery, PagedResponse<JobOfferMinimalDto>>
{
    public async Task<PagedResponse<JobOfferMinimalDto>> Handle(SearchJobsQuery request, CancellationToken ct)
    {
        var (items, total) = await repo.SearchJobOffersAsync(
            request.Search,
            request.LocationId,
            request.Arrangement,
            request.Type,
            request.Level,
            request.MinSalary,
            request.IsActive,
            request.Page,
            request.PageSize,
            ct);

        return PagedResponse<JobOfferMinimalDto>.Create(items, request.Page, request.PageSize, total);
    }
}