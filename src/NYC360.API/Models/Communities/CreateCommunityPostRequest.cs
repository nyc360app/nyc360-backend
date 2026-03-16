namespace NYC360.API.Models.Communities;

public record CreateCommunityPostRequest(
    int CommunityId,
    string Title, 
    string Content,
    List<string>? Tags, 
    List<IFormFile>? Attachments
);