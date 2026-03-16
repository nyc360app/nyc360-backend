using NYC360.Application.Features.Verifications.Queries;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Dashboard.Verifications;

public class GetPendingTagVerificationsEndpoint(IMediator mediator) 
    : Endpoint<PagedRequest, PagedResponse<PendingTagVerificationDto>>
{
    public override void Configure()
    {
        Get("/verifications/tags/pending");
        Permissions(Domain.Constants.Permissions.Tags.Verify); 
    }

    public override async Task HandleAsync(PagedRequest req, CancellationToken ct)
    {
        var query = new GetPendingVerificationsQuery(req.Page, req.PageSize);
        var result = await mediator.Send(query, ct);
        
        await Send.OkAsync(result, ct);
    }
}