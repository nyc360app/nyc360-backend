using NYC360.Domain.Dtos.Professions;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Queries.UserOffers;

public record GetUserJobOffersQuery(
    int UserId, 
    bool? IsActive, 
    int Page, 
    int PageSize
) : IRequest<PagedResponse<JobOfferManageDto>>;