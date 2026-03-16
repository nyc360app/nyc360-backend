using NYC360.Domain.Dtos.Pages;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Posts.Queries.GetHomeFeed;

public record GetHomeFeedQuery(int UserId) : IRequest<StandardResponse<HomeFeedDto>>;