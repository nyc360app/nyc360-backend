namespace NYC360.Domain.Enums.Events;

public enum ModerationActionType : byte
{
    Hide = 1,              // Event hidden from discovery
    Unhide,               // Event restored
    Cancel,               // Event cancelled by admin
    Delete,               // Event hard-deleted
    OrganizerStrike,      // Strike issued to organizer
    VisibilityChange      // Visibility modified (public/private/etc)
}