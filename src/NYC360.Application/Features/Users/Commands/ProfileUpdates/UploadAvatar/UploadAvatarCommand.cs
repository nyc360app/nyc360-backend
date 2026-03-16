using Microsoft.AspNetCore.Http;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.UploadAvatar;

public record UploadAvatarCommand(
    int UserId, 
    IFormFile File
) : IRequest<StandardResponse<string>>;