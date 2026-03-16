using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.AddEducation;

public record AddEducationCommand(
    int UserId,
    string School,
    string Degree,
    string FieldOfStudy,
    DateTime StartDate,
    DateTime? EndDate
) : IRequest<StandardResponse<int>>;