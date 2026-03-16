using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.Application.Features.Communities.Dashboard.ProcessDisbandRequest;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Communities.Dashboard;

public class ProcessDisbandRequestRequest
{
    public bool Approved { get; set; }
    public string? AdminNotes { get; set; }
}

public class ProcessDisbandRequestEndpoint(IMediator mediator)
    : Endpoint<ProcessDisbandRequestRequest, StandardResponse<string>>
{
    public override void Configure()
    {
        Post("/communities-dashboard/disband-requests/{RequestId}/process");
        Roles("SuccessAdmin", "SuperAdmin");
    }

    public override async Task HandleAsync(ProcessDisbandRequestRequest req, CancellationToken ct)
    {
        var requestId = Route<int>("RequestId");
        var userId = User.GetId(); // Assuming extension method exists based on previous code usage
        
        var command = new ProcessDisbandRequestCommand(requestId, userId!.Value, req.Approved, req.AdminNotes);
        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result, ct);
    }
}
