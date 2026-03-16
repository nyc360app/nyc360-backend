using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Queries.GetProfile;

public class GetProfileQueryHandler(
    IUserRepository userRepository, 
    IPostRepository postRepository)
    : IRequestHandler<GetProfileQuery, StandardResponse<UserProfileDto>>
{
    public async Task<StandardResponse<UserProfileDto>> Handle(GetProfileQuery request, CancellationToken ct)
    {
        var user = await userRepository.GetProfileAsync(request.Username, ct);
        if (user is null)
            return StandardResponse<UserProfileDto>.Failure(new ApiError("users.notfound", "User not found."));

        var recentPosts = await postRepository.GetRecentUserPostsAsync(user.UserId, 5, ct);
        var dto = UserProfileDto.Map(user, recentPosts); 

        return StandardResponse<UserProfileDto>.Success(dto);
    }
}