using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Tags.Queries.GetById;

public record GetTagByIdQuery(int Id) : IRequest<StandardResponse<TagDto>>;
