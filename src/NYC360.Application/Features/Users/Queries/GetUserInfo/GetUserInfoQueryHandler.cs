using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Queries.GetUserInfo;

public class GetUserInfoQueryHandler(
    IUserRepository userRepository)
    : IRequestHandler<GetUserInfoQuery, StandardResponse<UserInfoDto>>
{
    public async Task<StandardResponse<UserInfoDto>> Handle(GetUserInfoQuery request, CancellationToken ct)
    {
        // 1. Fetch User with all nested profile details
        var user = await userRepository.GetProfileInfoByUserIdAsync(request.UserId, ct);

        if (user == null) 
            return StandardResponse<UserInfoDto>.Failure(new ApiError("user.not_found", "User profile not found."));

        // 2. Map to UserInfoDto
        var dto = UserInfoDto.Map(user);

        return StandardResponse<UserInfoDto>.Success(dto);
    }
}