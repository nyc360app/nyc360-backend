using NYC360.Application.Features.Tags.Queries.GetById;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Dashboard.Tags;

public class GetTagByIdEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse<TagDto>>
{
    public override void Configure()
    {
        Get("/tags-dashboard/{id}");
        Permissions(Domain.Constants.Permissions.Tags.View); 
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var query = new GetTagByIdQuery(id);
        var result = await mediator.Send(query, ct);
        
        await Send.OkAsync(result, ct);
    }
}
