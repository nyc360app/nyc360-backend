using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Commands.Disband;

public record DisbandCommunityCommand(
    int UserId,
    int CommunityId,
    string Reason
) : IRequest<StandardResponse<string>>;
