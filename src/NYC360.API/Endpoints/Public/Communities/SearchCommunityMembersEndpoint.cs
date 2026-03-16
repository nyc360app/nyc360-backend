using NYC360.Application.Features.Communities.Queries.SearchMembers;
using NYC360.Domain.Dtos.Communities;
using NYC360.API.Models.Communities;
using NYC360.API.Extensions;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Communities;

public class SearchCommunityMembersEndpoint(IMediator mediator) 
    : Endpoint<SearchMembersRequest, PagedResponse<CommunityMemberDto>>
{
    public override void Configure()
    {
        Get("/communities/{CommunityId}/members/search");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SearchMembersRequest req, CancellationToken ct)
    {
        var query = new SearchCommunityMembersQuery(
            User.GetId(),
            req.CommunityId, 
            req.SearchTerm, 
            req.Page, 
            req.PageSize
        );
        
        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}
