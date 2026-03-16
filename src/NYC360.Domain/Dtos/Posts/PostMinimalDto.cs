namespace NYC360.Domain.Dtos.Posts;

public record PostMinimalDto(int Id, string Title, string? ImageUrl, int CommentsCount);