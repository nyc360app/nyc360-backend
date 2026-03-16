using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Forums.Queries.GetQuestionDetails;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Forums;

public record GetQuestionDetailsRequest(int QuestionId);

public class GetQuestionDetailsEndpoint(IMediator mediator) : Endpoint<GetQuestionDetailsRequest, StandardResponse<ForumQuestionDetailsDto>>
{
    public override void Configure()
    {
        Get("/forums/questions/{QuestionId}");
    }

    public override async Task HandleAsync(GetQuestionDetailsRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new GetQuestionDetailsQuery(request.QuestionId), ct);
        await Send.OkAsync(result, ct);
    }
}
