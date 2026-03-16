using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Professions;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Professions.Queries.GetFeed;

public class GetProfessionsFeedHandler(
    IPostRepository postRepo,
    IProfessionsRepository jobRepo,
    ITagRepository tagRepository) 
    : IRequestHandler<GetProfessionsFeedQuery, StandardResponse<ProfessionsFeedDto>>
{
    public async Task<StandardResponse<ProfessionsFeedDto>> Handle(GetProfessionsFeedQuery request, CancellationToken ct)
    {
        // 1. Fetch Professions Posts (Let's get 7 items: 1 Hero + 6 Grid)
        // We reuse your existing repo logic: Filter by Category.Profession
        var (posts, _) = await postRepo.GetAllPaginatedAsync(
            1, 
            7, 
            request.UserId,
            Category.Professions, 
            null, 
            null,
            null, // sourceType
            null, // authorId
            ct);

        // 2. Separate Hero from Grid
        var hero = posts.FirstOrDefault();
        var grid = posts.Skip(1).ToList();

        // 3. Fetch "Hiring News" (The 2 jobs at the bottom)
        var jobs = await jobRepo.GetFeaturedJobsAsync(2, ct);

        // 4. Fetch Tags for Professions
        var tags = await tagRepository.GetTagsByDivisionAsync(Category.Professions, ct);

        // 5. Construct Response
        var result = new ProfessionsFeedDto(
            hero,
            grid,
            jobs,
            tags.Select(t => t.Map()).ToList()
        );

        return StandardResponse<ProfessionsFeedDto>.Success(result);
    }
}