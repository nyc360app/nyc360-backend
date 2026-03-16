using NYC360.Domain.Dtos.Professions;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Queries.GetJobApplicants;

public record GetJobApplicantsQuery(
    int JobOfferId,
    int OwnerId,
    int Page,
    int PageSize
) : IRequest<PagedResponse<JobApplicationDetailsDto>>;