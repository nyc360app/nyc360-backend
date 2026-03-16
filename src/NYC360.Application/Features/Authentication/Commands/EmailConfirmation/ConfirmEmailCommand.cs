using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.EmailConfirmation;

public record ConfirmEmailCommand(string Email, string Token) 
    : IRequest<StandardResponse>;