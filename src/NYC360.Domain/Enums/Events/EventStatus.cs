namespace NYC360.Domain.Enums.Events;

public enum EventStatus : byte
{
    Draft,
    Published,
    Cancelled,
    Completed,
    Archived,
    Hidden
}