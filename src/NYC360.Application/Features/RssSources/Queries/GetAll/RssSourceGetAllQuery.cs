using MediatR;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.RssSources.Queries.GetAll;

public record RssSourceGetAllQuery(Category? Category = null)
    : IRequest<StandardResponse<List<RssSourceDto>>>;
