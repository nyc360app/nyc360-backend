using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Forums;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Queries.GetDashboardForums;

public class GetDashboardForumsQueryHandler(IForumRepository forumRepository) 
    : IRequestHandler<GetDashboardForumsQuery, StandardResponse<List<ForumDto>>>
{
    public async Task<StandardResponse<List<ForumDto>>> Handle(GetDashboardForumsQuery request, CancellationToken cancellationToken)
    {
        var forums = await forumRepository.GetAllForumsAsync(cancellationToken);
        
        var dtos = forums.Select(x => new ForumDto(
                x.Id,
                x.Title,
                x.Description,
                x.Slug,
                x.IconUrl,
                x.Questions.Count,
                x.IsActive,
                x.Moderators.Select(m => UserMinimalInfoDto.Map(m.Moderator)).ToList()
            )).ToList();

        return StandardResponse<List<ForumDto>>.Success(dtos);
    }
}
