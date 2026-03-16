using Microsoft.AspNetCore.Http;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Verifications.Commands.TagRequest;

public record SubmitTagVerificationCommand(
    int UserId,
    int TagId,
    string Reason,
    DocumentType DocumentType,
    IFormFile File
) : IRequest<StandardResponse>;