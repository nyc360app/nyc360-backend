using NYC360.Domain.Wrappers;
using MediatR;
using NYC360.Domain.Dtos.Professions;

namespace NYC360.Application.Features.Professions.Queries.UserApplications;

public record GetUserApplicationsQuery(
    int UserId, 
    int Page = 1, 
    int PageSize = 10
) : IRequest<PagedResponse<JobApplicationDto>>;