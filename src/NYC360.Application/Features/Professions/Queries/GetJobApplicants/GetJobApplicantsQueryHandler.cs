using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Professions;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Queries.GetJobApplicants;

public class GetJobApplicantsQueryHandler(IProfessionsRepository repo) 
    : IRequestHandler<GetJobApplicantsQuery, PagedResponse<JobApplicationDetailsDto>>
{
    public async Task<PagedResponse<JobApplicationDetailsDto>> Handle(GetJobApplicantsQuery request, CancellationToken ct)
    {
        var offer = await repo.GetJobOfferByIdAsync(request.JobOfferId, ct);
        if (offer == null)
        {
            return PagedResponse<JobApplicationDetailsDto>.Failure(new ApiError("job.notfound", "Job offer not found."));
        }

        if (offer.AuthorId != request.OwnerId)
        {
            return PagedResponse<JobApplicationDetailsDto>.Failure(new ApiError("job.forbidden", "You are not authorized to view this job."));
        }
        
        var (applications, total) = await repo.GetApplicationsByJobIdAsync(
            request.JobOfferId, 
            request.OwnerId, 
            request.Page, 
            request.PageSize, 
            ct);

        var dtos = applications.Select(JobApplicationDetailsDto.Map).ToList();

        return PagedResponse<JobApplicationDetailsDto>.Create(dtos, request.Page, request.PageSize, total);
    }
}