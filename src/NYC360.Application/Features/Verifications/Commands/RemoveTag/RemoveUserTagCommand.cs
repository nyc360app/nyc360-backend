using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Verifications.Commands.RemoveTag;

public record RemoveUserTagCommand(
    int UserId,
    int TagId,
    int AdminId
) : IRequest<StandardResponse>;