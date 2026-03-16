using Microsoft.AspNetCore.Http;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Users.Commands.Dashboard.EditProfile;

public record DashboardEditUserProfileCommand(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string Bio,
    IFormFile? Avatar
) : IRequest<StandardResponse>;