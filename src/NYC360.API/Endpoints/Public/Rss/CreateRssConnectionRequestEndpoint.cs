using NYC360.Domain.Enums;
using FastEndpoints;
using MediatR;
using NYC360.Application.Features.RssSources.Commands.ConnectRequest;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using NYC360.API.Models.RssSources;

namespace NYC360.API.Endpoints.Public.Rss;

public class CreateRssConnectionRequestEndpoint(IMediator mediator) : Endpoint<RssFeedConnectionRequestRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/rss/connect");
        AllowFileUploads();
    }
    
    public override async Task HandleAsync(RssFeedConnectionRequestRequest request, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var url = string.IsNullOrWhiteSpace(request.Url)
            ? Query<string>("url", false) ?? string.Empty
            : request.Url;

        var category = request.Category;
        var queryCategory = Query<string>("category", false);
        if (!string.IsNullOrWhiteSpace(queryCategory) && TryParseCategory(queryCategory, out var parsedCategory))
        {
            category = parsedCategory;
        }

        var command = new RssFeedConnectionRequestCreateCommand(
            url, 
            category, 
            request.Name, 
            request.Description, 
            request.ImageUrl,
            request.Image,
            request.Language,
            request.SourceWebsite,
            request.SourceCredibility,
            request.AgreementAccepted,
            request.DivisionTag,
            request.LogoImage,
            request.LogoFileName,
            userId.Value);
            
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }

    private static bool TryParseCategory(string rawValue, out Category category)
    {
        if (Enum.TryParse<Category>(rawValue, true, out var byName) && Enum.IsDefined(byName))
        {
            category = byName;
            return true;
        }

        if (int.TryParse(rawValue, out var numeric) && Enum.IsDefined(typeof(Category), numeric))
        {
            category = (Category)numeric;
            return true;
        }

        category = default;
        return false;
    }
}
