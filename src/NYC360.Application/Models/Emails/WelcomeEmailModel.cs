namespace NYC360.Application.Models.Emails;

public sealed record WelcomeEmailModel(string FullName, string Email, string? Token);