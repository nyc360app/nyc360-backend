using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Forums.Queries.GetQuestions;
using NYC360.Domain.Dtos.Forums;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Forums;

public record GetQuestionsRequest(string ForumSlug, int Page = 1, int PageSize = 10);

public class GetQuestionsEndpoint(IMediator mediator) : Endpoint<GetQuestionsRequest, StandardResponse<ForumWithQuestionsDto>>
{
    public override void Configure()
    {
        Get("/forums/{ForumSlug}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetQuestionsRequest request, CancellationToken ct)
    {
        var query = new GetQuestionsQuery(request.ForumSlug, request.Page, request.PageSize);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}
