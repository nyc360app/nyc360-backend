using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Topics.Queries.GetPostsByTopic;

public class GetPostsByTopicQueryHandler(IPostRepository postRepository)
    : IRequestHandler<GetPostsByTopicQuery, PagedResponse<PostDto>>
{
    public async Task<PagedResponse<PostDto>> Handle(GetPostsByTopicQuery request, CancellationToken cancellationToken)
    {
        var (posts, total) = await postRepository.GetPostsByTopicPaginatedAsync(
            request.TopicId, 
            request.Page, 
            request.PageSize, 
            request.UserId, 
            cancellationToken);

        return PagedResponse<PostDto>.Create(posts, total, request.Page, request.PageSize);
    }
}
