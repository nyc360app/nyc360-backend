using NYC360.Domain.Enums;
using MediatR;
using NYC360.Domain.Wrappers;
using Microsoft.AspNetCore.Http;

namespace NYC360.Application.Features.RssSources.Commands.Create;

public record RssSourceCreateCommand(
    string Url, 
    Category Category, 
    string Name, 
    string? Description, 
    string? ImageUrl,
    IFormFile? Image)
    : IRequest<StandardResponse>;