using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Queries.GetMyAuthorizations;

public record GetMyAuthorizationsQuery(int UserId) : IRequest<StandardResponse<List<HouseListingAuthorizationDto>>>;
