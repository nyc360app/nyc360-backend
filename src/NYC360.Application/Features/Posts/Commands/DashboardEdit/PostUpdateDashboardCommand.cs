using Microsoft.AspNetCore.Http;
using NYC360.Domain.Enums;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Commands.DashboardEdit;

public sealed record PostUpdateDashboardCommand(
    int Id,
    string? Title,
    string Content,
    Category Category,
    int? TopicId,
    List<IFormFile>? AddedAttachments,
    List<int>? RemovedAttachments
) : IRequest<StandardResponse>;