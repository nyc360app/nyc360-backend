using MediatR;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.ReviewLeaderApplication;

public record ReviewCommunityLeaderApplicationCommand(
    int ApplicationId,
    int AdminUserId,
    bool Approved,
    string? AdminNotes)
    : IRequest<StandardResponse<CommunityLeaderApplicationAdminDetailsDto>>;
