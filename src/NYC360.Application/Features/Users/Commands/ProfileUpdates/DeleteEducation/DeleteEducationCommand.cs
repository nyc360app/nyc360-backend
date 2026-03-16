using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.DeleteEducation;

public record DeleteEducationCommand(
    int UserId, 
    int EducationId
) : IRequest<StandardResponse>;