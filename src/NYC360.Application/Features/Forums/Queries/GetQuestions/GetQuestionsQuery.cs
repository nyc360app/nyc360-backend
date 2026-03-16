using NYC360.Domain.Dtos.Forums;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Forums.Queries.GetQuestions;

public record GetQuestionsQuery(
    string ForumSlug,
    int Page = 1,
    int PageSize = 10
) : IRequest<StandardResponse<ForumWithQuestionsDto>>;