using Microsoft.AspNetCore.Http;
using NYC360.Domain.Enums;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.RssSources.Commands.Update;

public record RssSourceUpdateCommand(
    int Id, 
    string RssUrl, 
    Category Category, 
    string? Name, 
    string? Description, 
    IFormFile? Image,
    bool IsActive)
    : IRequest<StandardResponse>;