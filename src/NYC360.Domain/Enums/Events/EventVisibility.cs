namespace NYC360.Domain.Enums.Events;

public enum EventVisibility : byte
{
    Public,
    PrivateNoSecurity,
    PrivateSpecialUrl,
    PrivatePassword,
    PrivateApproval
}