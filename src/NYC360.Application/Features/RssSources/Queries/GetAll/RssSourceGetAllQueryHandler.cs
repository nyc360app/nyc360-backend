using NYC360.Application.Contracts.Persistence;
using MediatR;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.RssSources.Queries.GetAll;

public class RssSourceGetAllQueryHandler(
    IRssSourceRepository rssRepo)
    : IRequestHandler<RssSourceGetAllQuery, StandardResponse<List<RssSourceDto>>>
{
    public async Task<StandardResponse<List<RssSourceDto>>> Handle(RssSourceGetAllQuery request, CancellationToken ct)
    {
        var list = await rssRepo.GetAllAsync(ct);
        var dtos = list.Select(RssSourceDto.Map).ToList();
        return StandardResponse<List<RssSourceDto>>.Success(dtos);
    }
}