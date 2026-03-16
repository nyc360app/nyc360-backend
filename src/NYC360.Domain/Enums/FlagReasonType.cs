namespace NYC360.Domain.Enums;

public enum FlagReasonType : byte
{
    Spam = 1,
    HateSpeech,
    Harassment,
    InappropriateContent,
    ScamOrFraud,
    ViolationOfPolicy,
    Other
}