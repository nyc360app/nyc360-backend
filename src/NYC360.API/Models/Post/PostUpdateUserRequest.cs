using NYC360.Domain.Enums;

namespace NYC360.API.Models.Post;

public sealed record PostUpdateUserRequest(
    int PostId,
    string Title,
    string Content,
    Category Category,
    int? TopicId,
    List<int>? TagIds,
    List<IFormFile>? AddedAttachments,
    List<int>? RemovedAttachments
);