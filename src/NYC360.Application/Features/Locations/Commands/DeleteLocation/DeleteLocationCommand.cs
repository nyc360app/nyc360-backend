using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Locations.Commands.DeleteLocation;

public record DeleteLocationCommand(int Id) : IRequest<StandardResponse>;
