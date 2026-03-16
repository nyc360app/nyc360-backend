using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Posts.Queries.GetMyPostsAnalysisByCategory;

public record GetMyPostsAnalysisByCategoryQuery(
    Category Category, 
    int UserId
) : IRequest<StandardResponse<PostAnalysisDto>>;
