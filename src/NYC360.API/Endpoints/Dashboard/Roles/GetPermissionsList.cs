using FastEndpoints;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Roles;

public class GetPermissionsList : EndpointWithoutRequest<StandardResponse<List<string>>>
{
    public override void Configure()
    {
        Get("/roles-dashboard/all-permissions");
        Permissions(Domain.Constants.Permissions.Roles.Create, Domain.Constants.Permissions.Roles.Edit);
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var list = Domain.Constants.Permissions.GetAllPermissions();
        var result = StandardResponse<List<string>>.Success(list);
        await Send.OkAsync(result, ct);
    }
}