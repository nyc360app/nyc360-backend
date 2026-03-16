using MediatR;
using NYC360.Domain.Dtos.Events;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Events.Queries.GetById;

public record GetEventByIdQuery(int Id) : IRequest<StandardResponse<EventDto>>;
