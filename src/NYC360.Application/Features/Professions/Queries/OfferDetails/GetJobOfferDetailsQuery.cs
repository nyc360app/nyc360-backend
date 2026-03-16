using NYC360.Domain.Dtos.Professions;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Queries.OfferDetails;

public record GetJobOfferDetailsQuery(
    int? UserId,
    int Id
) : IRequest<StandardResponse<JobOfferProfileDto>>;