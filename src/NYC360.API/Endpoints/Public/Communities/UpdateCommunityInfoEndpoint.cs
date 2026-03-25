using NYC360.Application.Features.Communities.Commands.UpdateInfo;
using NYC360.API.Models.Communities;
using NYC360.Domain.Dtos.Communities;
using NYC360.API.Extensions;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Communities;

public class UpdateCommunityInfoEndpoint(IMediator mediator) : Endpoint<UpdateCommunityInfoRequest, StandardResponse<CommunityDto>>
{
    public override void Configure()
    {
        Put("/communities/{CommunityId}/update");
        AllowFileUploads();
    }

    public override async Task HandleAsync(UpdateCommunityInfoRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse<CommunityDto>.Failure(
                new ApiError("auth.unauthorized", "User not authenticated.")
            ), ct);
            return;
        }

        var command = new UpdateCommunityInfoCommand(
            userId.Value,
            req.CommunityId,
            req.Name,
            req.Description,
            req.Rules,
            req.Type,
            req.LocationId,
            req.IsPrivate,
            req.AnyoneCanPost,
            req.RequiresApproval,
            req.AvatarImage,
            req.CoverImage
        );

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}
