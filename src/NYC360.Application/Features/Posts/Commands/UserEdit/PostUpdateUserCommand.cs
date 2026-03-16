using Microsoft.AspNetCore.Http;
using NYC360.Domain.Enums;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Commands.UserEdit;

public sealed record PostUpdateUserCommand(
    int UserId,
    string UserRole,
    int PostId,
    string Title,
    string Content,
    Category Category,
    int? TopicId,
    List<int>? TagIds,
    List<IFormFile>? AddedAttachments,
    List<int>? RemovedAttachments
) : IRequest<StandardResponse>;