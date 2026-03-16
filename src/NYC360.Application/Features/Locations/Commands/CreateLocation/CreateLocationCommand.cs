using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Locations.Commands.CreateLocation;

public record CreateLocationCommand(
    string Borough,
    string Code,
    string NeighborhoodNet,
    string Neighborhood,
    int ZipCode
) : IRequest<StandardResponse<int>>;
