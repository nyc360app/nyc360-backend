using NYC360.Domain.Dtos.Posts;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Queries.PostDetails;

public record PostGetDetailsQuery(int Id, int? UserId) 
    : IRequest<StandardResponse<PostDetailsDto>>;