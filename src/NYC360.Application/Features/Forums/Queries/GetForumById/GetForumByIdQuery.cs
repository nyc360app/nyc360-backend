using MediatR;
using NYC360.Domain.Dtos.Forums;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Queries.GetForumById;

public record GetForumByIdQuery(int Id) : IRequest<StandardResponse<ForumDto>>;
