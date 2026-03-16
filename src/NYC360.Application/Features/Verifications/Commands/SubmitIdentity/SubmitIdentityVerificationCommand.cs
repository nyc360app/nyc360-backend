using Microsoft.AspNetCore.Http;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Verifications.Commands.SubmitIdentity;

public record SubmitIdentityVerificationCommand(
    int UserId,
    DocumentType DocumentType,
    string Reason,
    IFormFile File
) : IRequest<StandardResponse>;