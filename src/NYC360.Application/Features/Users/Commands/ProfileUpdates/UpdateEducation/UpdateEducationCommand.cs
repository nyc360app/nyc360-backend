using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.UpdateEducation;

public record UpdateEducationCommand(
    int UserId,
    int EducationId,
    string School,
    string Degree,
    string FieldOfStudy,
    DateTime StartDate,
    DateTime? EndDate
) : IRequest<StandardResponse>;