using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Forums;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Queries.GetForums;

public class GetForumsQueryHandler(IForumRepository forumRepository) 
    : IRequestHandler<GetForumsQuery, StandardResponse<List<ForumDto>>>
{
    public async Task<StandardResponse<List<ForumDto>>> Handle(GetForumsQuery request, CancellationToken cancellationToken)
    {
        var forums = await forumRepository.GetForumsAsync(cancellationToken);
        
        var dtos = forums.Select(f => ForumDto.Map(f)).ToList();

        return StandardResponse<List<ForumDto>>.Success(dtos);
    }
}
