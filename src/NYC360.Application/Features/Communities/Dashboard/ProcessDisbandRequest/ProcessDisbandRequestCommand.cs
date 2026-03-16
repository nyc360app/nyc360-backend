using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.ProcessDisbandRequest;

public record ProcessDisbandRequestCommand(
    int RequestId,
    int AdminUserId,
    bool Approved,
    string? AdminNotes
) : IRequest<StandardResponse<string>>;
