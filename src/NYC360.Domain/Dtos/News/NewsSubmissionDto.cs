using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Dtos.Topics;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Enums.Posts;

namespace NYC360.Domain.Dtos.News;

public record NewsSubmissionDto(
    int Id,
    string? Title,
    string Content,
    UserMinimalInfoDto? Author,
    LocationDto? Location,
    TopicDto? Topic,
    DateTime CreatedAt,
    DateTime LastUpdated,
    PostModerationStatus ModerationStatus,
    string? ModerationNote
);
