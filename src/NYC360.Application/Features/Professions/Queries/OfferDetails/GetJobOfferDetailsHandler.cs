using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Professions;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Queries.OfferDetails;

public class GetJobOfferDetailsHandler(IProfessionsRepository repo) 
    : IRequestHandler<GetJobOfferDetailsQuery, StandardResponse<JobOfferProfileDto>>
{
    public async Task<StandardResponse<JobOfferProfileDto>> Handle(GetJobOfferDetailsQuery request, CancellationToken ct)
    {
        var job = await repo.GetJobOfferByIdAsync(request.Id, ct);

        if (job == null)
            return StandardResponse<JobOfferProfileDto>.Failure(new ApiError("job.not_found", "Job offer not found."));
        
        var isApplied = request.UserId != null && await repo.HasAlreadyAppliedAsync(job.Id, request.UserId.Value, ct);
        var applicationsCount = await repo.GetApplicationCountAsync(job.Id, ct);
        var dto = JobOfferDetailsDto.Map(job, applicationsCount, isApplied);
        
        var locationId = job.Address?.LocationId ?? 0; 
        var relatedJobs = locationId > 0 ? await repo.GetRelatedJobsAsync(job.Id, locationId, job.EmploymentType, 4, ct) : [];
        
        var result = new JobOfferProfileDto(dto, relatedJobs);
        return StandardResponse<JobOfferProfileDto>.Success(result);
    }
}