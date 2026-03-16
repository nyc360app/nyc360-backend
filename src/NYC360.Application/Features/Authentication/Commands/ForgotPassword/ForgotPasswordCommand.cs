using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.ForgotPassword;

public record ForgotPasswordCommand(string Email) : IRequest<StandardResponse>;