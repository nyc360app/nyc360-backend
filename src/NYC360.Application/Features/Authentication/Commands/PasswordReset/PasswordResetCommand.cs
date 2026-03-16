using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.PasswordReset;

public record PasswordResetCommand(string Email, string NewPassword, string Token) : IRequest<StandardResponse>;