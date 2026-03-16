using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Queries.GetUserInfo;

public record GetUserInfoQuery(int UserId) : IRequest<StandardResponse<UserInfoDto>>;