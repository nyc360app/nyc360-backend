using MediatR;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.RssSources.Queries.Test;

public record TestRssSourceQuery(string Url) : IRequest<StandardResponse<RssSourceDto>>;
