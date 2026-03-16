using NYC360.Domain.Enums.Professions;
using NYC360.Domain.Wrappers;
using MediatR;
using NYC360.Domain.Dtos.Location;

namespace NYC360.Application.Features.Professions.Commands.UpdateOffer;

public record UpdateJobOfferCommand(
    int UserId,
    int Id,
    string Title,
    string Description,
    string? Requirements,
    string? Benefits,
    string? Responsibilities,
    decimal? SalaryMin,
    decimal? SalaryMax,
    WorkArrangement WorkArrangement,
    EmploymentType EmploymentType,
    EmploymentLevel EmploymentLevel,
    AddressInputDto? Address
) : IRequest<StandardResponse>;