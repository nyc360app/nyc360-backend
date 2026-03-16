using NYC360.Application.Contracts.Persistence;
using MediatR;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Users.Queries.List;

public class GetUsersListQueryHandler(IUserRepository userRepository) 
    : IRequestHandler<GetUsersListQuery, PagedResponse<UserDashboardDetailDto>>
{
    public async Task<PagedResponse<UserDashboardDetailDto>> Handle(GetUsersListQuery request, CancellationToken ct)
    {
        var totalCount = await userRepository.CountAsync(ct);
        var users = await userRepository.GetPagedUsersAsync(request.Page, request.PageSize, ct);

        var dtos = new List<UserDashboardDetailDto>();

        foreach (var user in users)
        {
            var roles = await userRepository.GetUserRolesAsync(user, ct);
            dtos.Add(UserDashboardDetailDto.Map(user, roles));
        }

        return PagedResponse<UserDashboardDetailDto>.Create(
            data: dtos,
            page: request.Page,
            pageSize: request.PageSize,
            totalCount: totalCount
        );
    }
}