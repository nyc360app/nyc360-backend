using NYC360.Domain.Enums.Professions;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Commands.Create;

public record CreateJobOfferCommand(
    int UserId,
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
) : IRequest<StandardResponse<int>>;