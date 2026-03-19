using NYC360.Domain.Dtos.Tags;

namespace NYC360.Domain.Dtos.News;

public record NewsAccessDto(
    bool IsStaff,
    bool IsOrganizationAccount,
    bool CanSubmitContent,
    bool CanModerateContent,
    bool CanPublishContent,
    bool CanConnectRss,
    bool CanReviewRssRequests,
    bool CanListNewsOrganizationInSpace,
    List<string> GrantedKeys,
    List<TagDto> GrantedNewsTags
);
