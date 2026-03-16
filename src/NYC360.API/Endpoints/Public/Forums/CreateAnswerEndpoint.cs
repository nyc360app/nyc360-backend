using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.Forums;
using NYC360.Application.Features.Forums.Commands.CreateAnswer;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Forums;

public class CreateAnswerEndpoint(IMediator mediator) : Endpoint<CreateAnswerRequest, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/forums/answers/create");
    }

    public override async Task HandleAsync(CreateAnswerRequest request, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new CreateAnswerCommand(
            request.QuestionId,
            request.Content,
            userId.Value
        );

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}
