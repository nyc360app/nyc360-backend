namespace NYC360.API.Models.Communities;

public class ReviewCommunityLeaderApplicationRequest
{
    public bool Approved { get; set; }
    public string? AdminNotes { get; set; }
    public string? AdminComment { get; set; }
}
