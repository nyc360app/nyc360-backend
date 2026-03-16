using NYC360.Domain.Dtos.User;
using NYC360.Domain.Entities.Forums;

namespace NYC360.Domain.Dtos.Forums;

public record ForumDto(
    int Id,
    string Title,
    string Description,
    string Slug,
    string? IconUrl,
    int QuestionsCount,
    bool IsActive,
    List<UserMinimalInfoDto>? Moderators
);

public static class ForumDtoExtension
{
    extension(ForumDto)
    {
        public static ForumDto Map(Forum forum, int? questionsCount = null) => new(
            forum.Id,
            forum.Title,
            forum.Description,
            forum.Slug,
            forum.IconUrl,
            questionsCount ?? forum.Questions.Count,
            forum.IsActive,
            forum.Moderators.Select(m => UserMinimalInfoDto.Map(m.Moderator)).ToList()
        );
    }
}
