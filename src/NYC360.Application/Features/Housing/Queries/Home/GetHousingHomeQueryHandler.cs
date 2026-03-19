using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Housing.Queries.Home;

public class GetHousingHomeQueryHandler(
    IHouseInfoRepository houseInfoRepository,
    IPostRepository postRepository,
    ITagRepository tagRepository) 
    : IRequestHandler<GetHousingHomeQuery, StandardResponse<HousingHomeDto>>
{
    public async Task<StandardResponse<HousingHomeDto>> Handle(GetHousingHomeQuery request, CancellationToken ct)
    {
        // 1. Fetch the Hero (Latest Featured or General Housing Post)
        var heroResult = await postRepository.GetAllPaginatedAsync(
            page: 1, 
            pageSize: 5, 
            userId: request.UserId, 
            category: Category.Housing, 
            search: null, 
            postType: null,
            sourceType: PostSource.Rss,
            authorId: null,
            includeUnapproved: false,
            ct: ct);
            
        var hero = heroResult.Item1.FirstOrDefault();

        // 2. Fetch specialized Housing Info (Directly from HouseInfos table)
        var housingData = await houseInfoRepository.GetRecentHousingInfoAsync(40, ct);

        // 3. Split into ForSale and ForRenting using the boolean column
        var forRenting = housingData
            .Where(x => x.IsRenting)
            .Take(10)
            .Select(HousingMinimalDto.Map)
            .ToList();

        var forSale = housingData
            .Where(x => !x.IsRenting)
            .Take(10)
            .Select(HousingMinimalDto.Map)
            .ToList();
        
        // 4. Fetch RSS Feed (News/Updates)
        var rss = await postRepository.GetAllPaginatedAsync(
            1, 10, request.UserId, Category.Housing, null, null, PostSource.Rss, null, false, ct
        );
        
        // 5. Fetch Discussions (Standard text posts in Housing category)
        var discussions = await postRepository.GetAllPaginatedAsync(
            1, 5, request.UserId, Category.Housing, null, PostType.Normal, PostSource.User, null, false, ct
        );
        
        // 6. "All Posts" - a mix of the latest activity
        var excludedIds = forRenting.Select(x => x.Id)
            .Concat(forSale.Select(x => x.Id))
            .ToHashSet();
        
        var allPosts = housingData
            .Where(x => !excludedIds.Contains(x.Id))
            .OrderByDescending(x => x.Id)
            .Take(10)
            .Select(HousingMinimalDto.Map)
            .ToList();
            
        // 7. Fetch Tags for Housing
        var tags = await tagRepository.GetTagsByDivisionAsync(Category.Housing, ct);
        
        var result = new HousingHomeDto(
            hero!,
            forSale,
            forRenting,
            rss.Item1.ToList(),
            discussions.Item1.ToList(),
            allPosts,
            tags.Select(t => t.Map()).ToList()
        );

        return StandardResponse<HousingHomeDto>.Success(result);
    }
}
