namespace NYC360.Application.Models.Emails;

public record ResetPasswordEmailModel(string Fullname, string Email, string Token);