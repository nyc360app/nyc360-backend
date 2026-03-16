namespace NYC360.API.Models.Forums;

public record UpdateForumModeratorsRequest(
    int ForumId,
    List<int> ModeratorIds
);
