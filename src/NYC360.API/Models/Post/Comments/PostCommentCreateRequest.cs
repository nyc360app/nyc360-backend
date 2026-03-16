namespace NYC360.API.Models.Post.Comments;

public sealed record PostCommentCreateRequest(
    int PostId,
    string Content,
    int? ParentCommentId = null
);