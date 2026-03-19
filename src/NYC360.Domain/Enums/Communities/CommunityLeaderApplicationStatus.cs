using System.Text.Json.Serialization;

namespace NYC360.Domain.Enums.Communities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommunityLeaderApplicationStatus : byte
{
    Pending = 1,
    Approved,
    Rejected
}
