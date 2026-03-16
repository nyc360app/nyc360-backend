using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Common;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Common.Queries.GlobalSearch;
public class GlobalSearchQueryHandler(
    IPostRepository postRepo,
    IUserRepository userRepo,
    ICommunityRepository communityRepo,
    ITagRepository tagRepo,
    IHouseInfoRepository housingRepo) 
    : IRequestHandler<GlobalSearchQuery, StandardResponse<GlobalSearchDto>>
{
    public async Task<StandardResponse<GlobalSearchDto>> Handle(GlobalSearchQuery request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Term) || request.Term.Length < 2)
            return StandardResponse<GlobalSearchDto>.Success(new GlobalSearchDto());

        // Sequential execution to avoid DbContext concurrency issues
        var posts = await postRepo.SearchPostsAsync(request.Term, request.Division, request.UserId, request.Limit, ct);
        var users = await userRepo.SearchUsersAsync(request.Term, request.Limit, ct);
        var communities = await communityRepo.SearchCommunitiesAsync(request.Term, request.Limit, ct);
        var tags = await tagRepo.SearchTagsAsync(request.Term, request.Division, request.Limit, ct);
        
        List<HousingMinimalDto> housing = [];
        if (request.Division == null || request.Division == Category.Housing)
        {
            var housingEntities = await housingRepo.SearchHousingAsync(request.Term, request.Limit, ct);
            housing = housingEntities.Select(HousingMinimalDto.Map).ToList();
        }
        
        return StandardResponse<GlobalSearchDto>.Success(new GlobalSearchDto(
            posts,
            users,
            communities,
            tags,
            housing
        ));
    }
}