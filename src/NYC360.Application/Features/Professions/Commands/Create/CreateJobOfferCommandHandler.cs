using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Professions;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Commands.Create;

public class CreateJobOfferCommandHandler(
    IProfessionsRepository professionsRepository,
    ILocationRepository locationRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateJobOfferCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(CreateJobOfferCommand request, CancellationToken ct)
    {
        // 1. Create the Resource
        int? addressId = null;
        if (request.Address != null)
        {
            addressId = await locationRepository.GetOrCreateAddressIdAsync(request.Address, ct);
        }
        var jobOffer = new JobOffer
        {
            AuthorId = request.UserId,
            Title = request.Title,
            Description = request.Description,
            Requirements = request.Requirements,
            Benefits = request.Benefits,
            Responsablities = request.Responsibilities,
            SalaryMin = request.SalaryMin,
            SalaryMax = request.SalaryMax,
            WorkArrangement = request.WorkArrangement,
            EmploymentType = request.EmploymentType,
            EmploymentLevel = request.EmploymentLevel,
            AddressId = addressId,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // 2. Persist
        await professionsRepository.AddJobOfferAsync(jobOffer, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<int>.Success(jobOffer.Id);
    }
}