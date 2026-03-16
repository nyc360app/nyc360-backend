using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.ChangePassword;

public record ChangePasswordCommand(string Email, string CurrentPassword, string NewPassword) : IRequest<StandardResponse>;
