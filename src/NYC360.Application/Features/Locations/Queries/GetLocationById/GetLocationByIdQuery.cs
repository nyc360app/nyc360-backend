using MediatR;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Locations.Queries.GetLocationById;

public record GetLocationByIdQuery(int Id) : IRequest<StandardResponse<LocationDto>>;
