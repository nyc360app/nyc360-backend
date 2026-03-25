using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.News;
using NYC360.Application.Features.RssSources.Commands.ConnectRequest;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.News;

public class CreateNewsRssConnectionRequestEndpoint(IMediator mediator)
    : Endpoint<NewsRssConnectionRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/news/rss/connect");
        AllowFileUploads();
    }

    public override async Task HandleAsync(NewsRssConnectionRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(
            new RssFeedConnectionRequestCreateCommand(
                req.Url,
                Category.News,
                req.Name,
                req.Description,
                req.ImageUrl,
                req.Language,
                req.SourceWebsite,
                req.SourceCredibility,
                req.AgreementAccepted,
                req.DivisionTag,
                req.LogoImage,
                userId.Value),
            ct);

        await Send.OkAsync(result, ct);
    }
}
