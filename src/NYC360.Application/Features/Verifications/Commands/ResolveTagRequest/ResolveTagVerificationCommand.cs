using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Verifications.Commands.ResolveTagRequest;

public record ResolveTagVerificationCommand(
    int UserId,
    int RequestId,
    bool Approved,
    string? AdminComment
) : IRequest<StandardResponse>;