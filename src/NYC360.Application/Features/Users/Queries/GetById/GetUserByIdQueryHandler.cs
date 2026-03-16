using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities;
using MediatR;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Users.Queries.GetById;

public class GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<GetUserByIdQuery, StandardResponse<UserDashboardDetailDto>>
{
    public async Task<StandardResponse<UserDashboardDetailDto>> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());

        if (user is null)
            return StandardResponse<UserDashboardDetailDto>.Failure(new ApiError("users.notfound", "User not found."));

        var roles = await userManager.GetRolesAsync(user);
        var dto = UserDashboardDetailDto.Map(user, roles);

        return StandardResponse<UserDashboardDetailDto>.Success(dto);
    }
}