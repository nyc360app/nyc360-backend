using FastEndpoints;
using NYC360.API.Extensions;
using NYC360.API.Models.News;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.News;

public class GetNewsPollByIdEndpoint(INewsPollService newsPollService)
    : Endpoint<GetNewsPollByIdRequest, StandardResponse<NewsPollDetailsDto>>
{
    public override void Configure()
    {
        Get("/news/polls/{PollId:int}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetNewsPollByIdRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        var result = await newsPollService.GetByIdAsync(userId, req.PollId, ct);
        await Send.OkAsync(result, ct);
    }
}
