using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Commands.UpdateOffer;

public class UpdateJobOfferCommandHandler(
    IProfessionsRepository professionsRepository, 
    ILocationRepository locationRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateJobOfferCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateJobOfferCommand request, CancellationToken ct)
    {
        // 1. Verify Ownership  
        var job = await professionsRepository.GetJobOfferForManagementAsync(request.Id, request.UserId, ct);
        
        if (job == null)
            return StandardResponse.Failure(new ApiError("job.not_found", "Job offer not found or access denied."));
        
        // 2. Update Fields
        job.Title = request.Title;
        job.Description = request.Description;
        job.Requirements = request.Requirements;
        job.Benefits = request.Benefits;
        job.Responsablities = request.Responsibilities;
        job.SalaryMin = request.SalaryMin;
        job.SalaryMax = request.SalaryMax;
        job.WorkArrangement = request.WorkArrangement;
        job.EmploymentType = request.EmploymentType;
        job.EmploymentLevel = request.EmploymentLevel;
        
        if (request.Address != null)
        {
            // Use the same idempotent method to get the ID
            var newAddressId = await locationRepository.GetOrCreateAddressIdAsync(request.Address, ct);
            job.AddressId = newAddressId;
        }
        else
        {
            // If the request sends a null address, decide if you want to keep the old one 
            // or clear it (if the job became fully remote/no fixed address)
            job.AddressId = null;
        }

        // 3. Persist
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}