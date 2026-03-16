using NYC360.Domain.Dtos.Common;
using NYC360.Domain.Dtos.User;

namespace NYC360.Domain.Dtos.Tags;

public record PendingTagVerificationDto(
    int RequestId,
    string Reason,
    DateTime SubmittedAt,
    TagMinimalDto Tag,
    UserMinimalInfoDto Requester,
    List<VerificationDocDto> Documents
);