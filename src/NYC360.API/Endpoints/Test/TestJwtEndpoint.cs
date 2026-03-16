// using System.Security.Claims;
// using FastEndpoints;
//
// namespace NYC360.API.Endpoints.Test;
//
// public class TestJwtEndpoint : EndpointWithoutRequest<ClaimsPrincipal>
// {
//     public override void Configure()
//     {
//         Get("/#test/jwt-permission");
//     }
//
//     public override async Task HandleAsync(CancellationToken ct)
//     {
//         var perms = User;
//         await Send.OkAsync(perms, ct);
//     }
// }