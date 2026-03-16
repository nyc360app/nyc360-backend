using Microsoft.AspNetCore.Http;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.UploadCover;

public record UploadCoverCommand(
    int UserId, 
    IFormFile File
) : IRequest<StandardResponse<string>>;