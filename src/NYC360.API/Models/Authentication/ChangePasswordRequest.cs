namespace NYC360.API.Models.Authentication;

public record ChangePasswordRequest(string CurrentPassword, string NewPassword);