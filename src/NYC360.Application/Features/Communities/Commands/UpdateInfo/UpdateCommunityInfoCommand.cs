using MediatR;
using Microsoft.AspNetCore.Http;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Commands.UpdateInfo;

public record UpdateCommunityInfoCommand(
    int UserId,
    int CommunityId,
    string? Name,
    string? Description,
    string? Rules,
    CommunityType? Type,
    int? LocationId,
    bool? IsPrivate,
    bool? AnyoneCanPost,
    bool? RequiresApproval,
    IFormFile? AvatarImage,
    IFormFile? CoverImage
) : IRequest<StandardResponse<CommunityDto>>;
