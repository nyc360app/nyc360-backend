using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Tags.Commands.Delete;

public record DeleteTagCommand(int Id) : IRequest<StandardResponse>;