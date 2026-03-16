    using NYC360.Application.Features.Communities.Queries.GetBySlug;
    using NYC360.API.Models.Communities;
    using NYC360.Domain.Wrappers;
    using NYC360.API.Extensions;
    using FastEndpoints;
    using MediatR;
    using NYC360.Domain.Dtos.Communities;

    namespace NYC360.API.Endpoints.Public.Communities;

    public class GetCommunityBySlugEndpoint(IMediator mediator) : Endpoint<GetCommunityBySlugRequest, StandardResponse<CommunityHomePageDto>>
    {
        public override void Configure()
        {
            Get("/communities/{Slug}");
        }
        
        public override async Task HandleAsync(GetCommunityBySlugRequest req, CancellationToken ct)
        {
            var userId = User.GetId();
            if (userId == null)
            {
                await Send.OkAsync(StandardResponse<CommunityHomePageDto>.Failure(
                    new ApiError("auth.invalidEmail", "Email not found")
                ), ct);
                return;
            }
            
            var query = new GetCommunityBySlugQuery(
                userId.Value, 
                req.Slug,
                req.Page,
                req.PageSize
            );
            var result = await mediator.Send(query, ct);

            await Send.OkAsync(result, ct);
        }
    }