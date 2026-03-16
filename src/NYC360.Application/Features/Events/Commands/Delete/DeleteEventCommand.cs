using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Events.Commands.Delete;

public record DeleteEventCommand(int Id, int UserId) : IRequest<StandardResponse<bool>>;
