using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Locations.Commands.UpdateLocation;

public record UpdateLocationCommand(
    int Id,
    string Borough,
    string Code,
    string NeighborhoodNet,
    string Neighborhood,
    int ZipCode
) : IRequest<StandardResponse>;
