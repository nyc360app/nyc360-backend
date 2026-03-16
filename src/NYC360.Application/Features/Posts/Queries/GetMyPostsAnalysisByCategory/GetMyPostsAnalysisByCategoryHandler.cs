using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Posts.Queries.GetMyPostsAnalysisByCategory;

public class GetMyPostsAnalysisByCategoryHandler(IPostRepository postRepository)
    : IRequestHandler<GetMyPostsAnalysisByCategoryQuery, StandardResponse<PostAnalysisDto>>
{
    public async Task<StandardResponse<PostAnalysisDto>> Handle(GetMyPostsAnalysisByCategoryQuery request, CancellationToken ct)
    {
        var analysis = await postRepository.GetMyPostsAnalysisByCategoryAsync(request.UserId, request.Category, ct);
        return StandardResponse<PostAnalysisDto>.Success(analysis);
    }
}
