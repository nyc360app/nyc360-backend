using MediatR;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Users.Queries.GetProfile;

public record GetProfileQuery(string Username)
    : IRequest<StandardResponse<UserProfileDto>>;