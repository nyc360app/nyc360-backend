using FastEndpoints;
using NYC360.API.Extensions;
using NYC360.API.Models.News;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.News;

public class UpdateNewsPollEndpoint(INewsPollService newsPollService)
    : Endpoint<UpdateNewsPollRequest, StandardResponse<NewsPollCreateResultDto>>
{
    public override void Configure()
    {
        Put("/news/polls/{PollId:int}/update");
        AllowFileUploads();
    }

    public override async Task HandleAsync(UpdateNewsPollRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var pollId = Route<int>("PollId");
        var result = await newsPollService.UpdateAsync(
            userId.Value,
            pollId,
            new NewsPollUpdateInput(
                req.Title,
                req.Question,
                req.Description,
                req.Options ?? [],
                req.CoverImage,
                req.ClosesAt,
                req.AllowMultipleAnswers,
                req.ShowResultsBeforeVoting,
                req.IsFeatured,
                req.Tags,
                req.LocationId),
            ct);

        await Send.OkAsync(result, ct);
    }
}
