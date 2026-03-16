using NYC360.Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace NYC360.Application.Features.Professions.Commands.Apply;

public record ApplyToJobCommand(
    int UserId,
    int JobOfferId,
    string? CoverLetter,
    IFormFile? Attachment
) : IRequest<StandardResponse<int>>;