namespace NYC360.Domain.Dtos.Posts;

public sealed record PostDetailsDto(
    PostDto Post,
    List<PostCommentDto> Comments,
    List<PostMinimalDto> RelatedPosts
);