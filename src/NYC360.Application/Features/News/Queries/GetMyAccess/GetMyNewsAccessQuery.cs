using MediatR;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.News.Queries.GetMyAccess;

public record GetMyNewsAccessQuery(int UserId) : IRequest<StandardResponse<NewsAccessDto>>;
