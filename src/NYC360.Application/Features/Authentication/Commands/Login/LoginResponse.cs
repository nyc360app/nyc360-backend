namespace NYC360.Application.Features.Authentication.Commands.Login;

public record LoginResponse(string? AccessToken, string? RefreshToken = null, bool TwoFactorRequired = false);