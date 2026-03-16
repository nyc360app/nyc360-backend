using NYC360.Domain.Enums;

namespace NYC360.API.Models.Post;

public record PostUpdateDashboardRequest(
    int PostId,
    string? Title,
    string Content,
    Category Category,
    int? TopicId,
    List<IFormFile>? AddedAttachments,
    List<int>? RemovedAttachments
);