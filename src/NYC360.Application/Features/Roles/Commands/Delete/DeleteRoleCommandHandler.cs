using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NYC360.Domain.Entities;
using MediatR;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Roles.Commands.Delete;

public class DeleteRoleCommandHandler(
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager,
    ILogger<DeleteRoleCommandHandler> logger
) : IRequestHandler<DeleteRoleCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(DeleteRoleCommand request, CancellationToken ct)
    {
        var role = await roleManager.FindByIdAsync(request.RoleId.ToString());
        if (role == null)
            return StandardResponse.Failure(new("role.notfound", "Role not found."));

        // Check if any users are assigned to this role
        var usersInRole = await userManager.GetUsersInRoleAsync(role.Name!);
        if (usersInRole.Any())
            return StandardResponse.Failure(new("role.assigned", "Cannot delete role. Users are assigned to this role."));

        var result = await roleManager.DeleteAsync(role);
        if (!result.Succeeded)
            return StandardResponse.Failure(new("role.delete.failed",
                string.Join(", ", result.Errors.Select(e => e.Description))));

        logger.LogInformation("Deleted role {RoleId} ({RoleName})", role.Id, role.Name);
        return StandardResponse.Success();
    }
}