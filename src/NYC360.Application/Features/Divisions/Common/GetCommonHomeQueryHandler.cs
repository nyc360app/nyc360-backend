using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Pages;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Divisions.Common;

public class GetCommonHomeQueryHandler(
    IPostRepository postRepo,
    ITagRepository tagRepository) 
    : IRequestHandler<GetCommonHomeQuery, StandardResponse<DivisionHomeDto>>
{
    public async Task<StandardResponse<DivisionHomeDto>> Handle(GetCommonHomeQuery request, CancellationToken ct)
    {
        if (request.Division is null)
            return StandardResponse<DivisionHomeDto>.Failure(
                new ApiError("division.required", "Division is required."));

        // These repositories share the same scoped DbContext, so these reads must stay sequential.
        // Running them in parallel causes EF Core to throw "A second operation was started on this context..."
        var featured = await postRepo.GetFeaturedPostsAsync(request.Division, request.UserId, request.Limit, ct);
        var latest = await postRepo.GetLatestPostsAsync(request.Division, request.UserId, request.Limit, ct);
        var trending = await postRepo.GetTrendingPostsAsync(request.Division, request.UserId, request.Limit, ct);
        var tags = await tagRepository.GetTagsByDivisionAsync(request.Division.Value, ct);

        return StandardResponse<DivisionHomeDto>.Success(new DivisionHomeDto(
            featured,
            latest,
            trending,
            tags.Select(t => t.Map()).ToList()
        ));
    }
}
