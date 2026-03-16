namespace NYC360.Domain.Enums.Events;

public enum EventTicketStatus : byte
{
    Active = 1,
    Used,
    Refunded,
    Cancelled
}