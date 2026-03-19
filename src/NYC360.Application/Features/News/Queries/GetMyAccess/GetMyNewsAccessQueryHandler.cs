using MediatR;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.News.Queries.GetMyAccess;

public class GetMyNewsAccessQueryHandler(INewsAuthorizationService newsAuthorizationService)
    : IRequestHandler<GetMyNewsAccessQuery, StandardResponse<NewsAccessDto>>
{
    public async Task<StandardResponse<NewsAccessDto>> Handle(GetMyNewsAccessQuery request, CancellationToken ct)
    {
        var access = await newsAuthorizationService.GetAccessAsync(request.UserId, ct);
        if (access == null)
            return StandardResponse<NewsAccessDto>.Failure(new ApiError("news.access_not_found", "User profile not found."));

        return StandardResponse<NewsAccessDto>.Success(access);
    }
}
