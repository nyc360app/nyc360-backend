namespace NYC360.API.Models.Authentication;

public sealed record PasswordResetRequest(string Email, string NewPassword, string Token);