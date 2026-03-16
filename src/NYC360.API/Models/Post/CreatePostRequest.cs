using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Enums;

namespace NYC360.API.Models.Post;

public sealed record CreatePostRequest(
    string Title, 
    string Content, 
    Category Category, 
    int? TopicId,
    PostType Type,
    int? LocationId,
    List<int>? Tags, 
    List<IFormFile>? Attachments
);