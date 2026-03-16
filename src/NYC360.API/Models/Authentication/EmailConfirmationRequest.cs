namespace NYC360.API.Models.Authentication;

public record EmailConfirmationRequest(string Email, string Token);