// using NYC360.Application.Features.Communities.Commands.TransferOwnership;
// using NYC360.API.Models.Communities;
// using NYC360.API.Extensions;
// using NYC360.Domain.Wrappers;
// using FastEndpoints;
// using MediatR;
//
// namespace NYC360.API.Endpoints.Public.Communities;
//
// public class TransferOwnershipEndpoint(IMediator mediator) : Endpoint<TransferOwnershipRequest, StandardResponse<string>>
// {
//     public override void Configure()
//     {
//         Post("/communities/{CommunityId}/transfer-ownership");
//     }
//
//     public override async Task HandleAsync(TransferOwnershipRequest req, CancellationToken ct)
//     {
//         var userId = User.GetId();
//         if (userId == null)
//         {
//             await Send.OkAsync(StandardResponse<string>.Failure(
//                 new ApiError("auth.unauthorized", "User not authenticated.")
//             ), ct);
//             return;
//         }
//
//         var command = new TransferOwnershipCommand(
//             userId.Value,
//             req.CommunityId,
//             req.NewOwnerId
//         );
//
//         var result = await mediator.Send(command, ct);
//         await Send.OkAsync(result, ct);
//     }
// }
