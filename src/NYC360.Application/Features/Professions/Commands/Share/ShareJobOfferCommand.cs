using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Professions.Commands.Share;

public record ShareJobOfferCommand(
    int UserId,
    int JobOfferId,
    string Content,
    List<string>? Tags,
    int? CommunityId
) : IRequest<StandardResponse<int>>;