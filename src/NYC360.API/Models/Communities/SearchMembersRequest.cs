namespace NYC360.API.Models.Communities;

public record SearchMembersRequest(
    int CommunityId,
    string? SearchTerm,
    int Page = 1,
    int PageSize = 20
);
