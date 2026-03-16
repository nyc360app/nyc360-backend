using NYC360.Application.Contracts.Persistence;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.Logout;

public class LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepo)
    : IRequestHandler<LogoutCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await refreshTokenRepo.RemoveAsync(request.UserId, cancellationToken);
        await refreshTokenRepo.SaveChangesAsync(cancellationToken);
        return StandardResponse.Success();
    }
}