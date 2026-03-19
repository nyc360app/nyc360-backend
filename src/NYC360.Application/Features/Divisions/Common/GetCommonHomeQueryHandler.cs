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

        var featuredTask = postRepo.GetFeaturedPostsAsync(request.Division, request.UserId, request.Limit, ct);
        var latestTask = postRepo.GetLatestPostsAsync(request.Division, request.UserId, request.Limit, ct);
        var trendingTask = postRepo.GetTrendingPostsAsync(request.Division, request.UserId, request.Limit, ct);
        var tagsTask = tagRepository.GetTagsByDivisionAsync(request.Division.Value, ct);

        await Task.WhenAll(featuredTask, latestTask, trendingTask, tagsTask);

        return StandardResponse<DivisionHomeDto>.Success(new DivisionHomeDto(
            await featuredTask,
            await latestTask,
            await trendingTask,
            (await tagsTask).Select(t => t.Map()).ToList()
        ));
    }
}
