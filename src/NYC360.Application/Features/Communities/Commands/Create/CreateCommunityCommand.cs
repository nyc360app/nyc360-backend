using MediatR;
using Microsoft.AspNetCore.Http;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Commands.Create;

public record CreateCommunityCommand(
    int UserId,
    string Name,
    string Description,
    string? Slug,
    CommunityType Type,
    int? LocationId,
    bool IsPrivate,
    IFormFile? AvatarImage,
    IFormFile? CoverImage
) : IRequest<StandardResponse<string>>;