namespace NYC360.API.Models.Tags;

public record ResolveTagVerificationRequest(
    int RequestId, 
    bool Approved, 
    string? AdminComment
);