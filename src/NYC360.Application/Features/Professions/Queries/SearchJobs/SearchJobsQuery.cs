using NYC360.Domain.Dtos.Professions;
using NYC360.Domain.Enums.Professions;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Queries.SearchJobs;

public record SearchJobsQuery(
    string? Search,
    int? LocationId,
    WorkArrangement? Arrangement,
    EmploymentType? Type,
    EmploymentLevel? Level,
    decimal? MinSalary,
    bool IsActive,
    int Page = 1,
    int PageSize = 20
) : IRequest<PagedResponse<JobOfferMinimalDto>>;