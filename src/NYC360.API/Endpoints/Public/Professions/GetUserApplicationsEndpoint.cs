using NYC360.Application.Features.Professions.Queries.UserApplications;
using NYC360.Domain.Dtos.Professions;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Professions;

public class GetUserApplicationsEndpoint(IMediator mediator) 
    : Endpoint<PagedRequest, PagedResponse<JobApplicationDto>>
{
    public override void Configure()
    {
        Get("/professions/applications/my-applications");
    }

    public override async Task HandleAsync(PagedRequest request, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
    
        var query = new GetUserApplicationsQuery(userId.Value, request.Page, request.PageSize);
        var result = await mediator.Send(query, ct);
        
        await Send.OkAsync(result, ct);
    }
}