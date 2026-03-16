using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.Dashboard.Delete;

public class DeleteUserHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<DeleteUserCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            return StandardResponse.Failure(new("user.notFound", "User not found."));

        if (await userManager.IsInRoleAsync(user, "SuperAdmin"))
            return StandardResponse.Failure(new("user.protected", "You cannot delete this account."));
        
        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            var error = result.Errors.FirstOrDefault();
            return StandardResponse.Failure(new("user.deleteFailed", error?.Description ?? "Failed to delete user."));
        }

        return StandardResponse.Success();
    }
}