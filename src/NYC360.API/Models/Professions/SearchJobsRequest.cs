using NYC360.Domain.Enums.Professions;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Models.Professions;

public record SearchJobsRequest(
    string? Search,
    int? LocationId,
    WorkArrangement? Arrangement,
    EmploymentType? Type,
    EmploymentLevel? Level,
    decimal? MinSalary,
    bool IsActive
) : PagedRequest;