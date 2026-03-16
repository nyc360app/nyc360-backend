using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Forums;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Forums.Queries.GetForumById;

public class GetForumByIdQueryHandler(IForumRepository forumRepository) 
    : IRequestHandler<GetForumByIdQuery, StandardResponse<ForumDto>>
{
    public async Task<StandardResponse<ForumDto>> Handle(GetForumByIdQuery request, CancellationToken cancellationToken)
    {
        var forum = await forumRepository.GetByIdAsync(request.Id, cancellationToken);
        if (forum == null)
        {
            return StandardResponse<ForumDto>.Failure(new ApiError("forum.not_found", "Forum not found."));
        }

        var dto = ForumDto.Map(forum);

        return StandardResponse<ForumDto>.Success(dto);
    }
}
