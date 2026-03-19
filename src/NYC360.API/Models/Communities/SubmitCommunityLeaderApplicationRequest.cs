namespace NYC360.API.Models.Communities;

public class SubmitCommunityLeaderApplicationRequest
{
    public string fullName { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string phoneNumber { get; set; } = string.Empty;
    public string communityName { get; set; } = string.Empty;
    public string location { get; set; } = string.Empty;
    public string? profileLink { get; set; }
    public string motivation { get; set; } = string.Empty;
    public string experience { get; set; } = string.Empty;
    public bool ledBefore { get; set; }
    public string weeklyAvailability { get; set; } = string.Empty;
    public bool agreedToGuidelines { get; set; }
    public IFormFile? verificationFile { get; set; }
}
