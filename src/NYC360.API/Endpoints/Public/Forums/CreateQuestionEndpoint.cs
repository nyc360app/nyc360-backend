using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.Forums;
using NYC360.Application.Features.Forums.Commands.CreateQuestion;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Forums;

public class CreateQuestionEndpoint(IMediator mediator) : Endpoint<CreateQuestionRequest, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/forums/questions/create");
    }

    public override async Task HandleAsync(CreateQuestionRequest request, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new CreateQuestionCommand(
            request.ForumId,
            request.Title,
            request.Content,
            userId.Value
        );

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}
