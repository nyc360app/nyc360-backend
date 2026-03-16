using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Users.Commands.ToggleTwoFactor;

public record ToggleTwoFactorCommand(int UserId, bool Enable) 
    : IRequest<StandardResponse>;