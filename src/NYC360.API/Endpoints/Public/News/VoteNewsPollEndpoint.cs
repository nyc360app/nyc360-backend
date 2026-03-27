using FastEndpoints;
using NYC360.API.Extensions;
using NYC360.API.Models.News;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.News;

public class VoteNewsPollEndpoint(INewsPollService newsPollService)
    : Endpoint<VoteNewsPollRequest, StandardResponse<NewsPollVoteResultDto>>
{
    public override void Configure()
    {
        Post("/news/polls/{PollId:int}/vote");
    }

    public override async Task HandleAsync(VoteNewsPollRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var pollId = Route<int>("PollId");
        var result = await newsPollService.VoteAsync(userId.Value, pollId, req.OptionIds ?? [], ct);
        await Send.OkAsync(result, ct);
    }
}
