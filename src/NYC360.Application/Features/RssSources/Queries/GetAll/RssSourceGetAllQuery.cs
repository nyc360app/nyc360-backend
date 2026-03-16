using MediatR;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.RssSources.Queries.GetAll;

public record RssSourceGetAllQuery()
    : IRequest<StandardResponse<List<RssSourceDto>>>;