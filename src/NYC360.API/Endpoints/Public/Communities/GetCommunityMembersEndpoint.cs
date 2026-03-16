using NYC360.Application.Features.Communities.Queries.GetMembers;
using NYC360.Domain.Dtos.Communities;
using NYC360.API.Models.Communities;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Communities;

public class GetCommunityMembersEndpoint(IMediator mediator) 
    : Endpoint<GetMembersRequest, PagedResponse<CommunityMemberDto>>
{
    public override void Configure()
    {
        Get("/communities/{CommunityId}/members");
    }

    public override async Task HandleAsync(GetMembersRequest req, CancellationToken ct)
    {
        var query = new GetCommunityMembersQuery(req.CommunityId, req.Page, req.PageSize);
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}