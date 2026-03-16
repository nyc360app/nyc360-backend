using NYC360.Domain.Enums;

namespace NYC360.API.Models.Post;

public record SubmitPostFlagRequest(
    int PostId,
    FlagReasonType Reason,
    string? Details
);