using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NYC360.Domain.Constants;
using NYC360.Domain.Entities;
using System.Security.Claims;
using MediatR;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Roles.Commands.Create;

public class CreateRoleCommandHandler(
    RoleManager<ApplicationRole> roleManager,
    ILogger<CreateRoleCommandHandler> logger) 
    : IRequestHandler<CreateRoleCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(CreateRoleCommand request, CancellationToken ct)
    {
        if (await roleManager.RoleExistsAsync(request.Name))
            return StandardResponse.Failure(new("role.exists", $"Role '{request.Name}' already exists."));

        var role = new ApplicationRole { Name = request.Name };
        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
            return StandardResponse.Failure(new("role.create.failed",
                string.Join(", ", result.Errors.Select(e => e.Description))));

        // Assign permissions as claims
        var allPermissions = Permissions.GetAllPermissions();
        foreach (var permission in request.Permissions.Intersect(allPermissions))
        {
            await roleManager.AddClaimAsync(role, new Claim(Permissions.PermissionClaimType, permission));
        }

        logger.LogInformation("Created role {Role} with {Count} permissions", request.Name, request.Permissions.Count);

        return StandardResponse.Success();
    }
}