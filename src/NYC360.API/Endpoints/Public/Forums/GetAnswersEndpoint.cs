using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Forums.Queries.GetAnswers;
using NYC360.Domain.Dtos.Forums;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Forums;

public record GetAnswersRequest(int QuestionId);

public class GetAnswersEndpoint(IMediator mediator) : Endpoint<GetAnswersRequest, StandardResponse<IEnumerable<ForumAnswerDto>>>
{
    public override void Configure()
    {
        Get("/forums/questions/{QuestionId}/answers");
    }

    public override async Task HandleAsync(GetAnswersRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new GetAnswersQuery(request.QuestionId), ct);
        await Send.OkAsync(result, ct);
    }
}
