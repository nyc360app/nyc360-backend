namespace NYC360.API.Models.Authentication;

public sealed record TwoFactorVerifyRequest(string Email, string Code);