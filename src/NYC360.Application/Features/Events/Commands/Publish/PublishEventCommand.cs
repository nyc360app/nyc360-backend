using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Events.Commands.Publish;

public record PublishEventCommand(
    int UserId, 
    int EventId
) : IRequest<StandardResponse>;