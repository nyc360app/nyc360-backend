namespace NYC360.Domain.Enums;

public enum FlagStatus : byte
{
    Pending = 1,
    UnderReview,
    Rejected,        // Admin reviewed, no action needed
    ActionTaken      // Admin reviewed, post removed or actioned
}