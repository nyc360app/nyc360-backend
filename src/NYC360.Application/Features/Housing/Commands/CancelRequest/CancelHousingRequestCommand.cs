using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Commands.CancelRequest;

public record CancelHousingRequestCommand(
    int UserId,
    int RequestId
) : IRequest<StandardResponse>;
