using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Professions;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Queries.UserApplications;

public class GetUserApplicationsQueryHandler(IProfessionsRepository repo) 
    : IRequestHandler<GetUserApplicationsQuery, PagedResponse<JobApplicationDto>>
{
    public async Task<PagedResponse<JobApplicationDto>> Handle(GetUserApplicationsQuery request, CancellationToken ct)
    {
        var (items, total) = await repo.GetUserApplicationsAsync(
            request.UserId, 
            request.Page, 
            request.PageSize, 
            ct
        );
        
        var dtos = items.Select(JobApplicationDto.Map).ToList();

        return PagedResponse<JobApplicationDto>.Create(
            dtos, 
            request.Page, 
            request.PageSize, 
            total
        );
    }
}