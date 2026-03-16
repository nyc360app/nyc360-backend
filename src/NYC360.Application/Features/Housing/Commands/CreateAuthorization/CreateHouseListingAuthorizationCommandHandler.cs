using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Entities.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Commands.CreateAuthorization;

public class CreateHouseListingAuthorizationCommandHandler(
    IHouseInfoRepository housingRepository,
    IUnitOfWork unitOfWork,
    ILocalStorageService storageService)
    : IRequestHandler<CreateHouseListingAuthorizationCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(CreateHouseListingAuthorizationCommand request, CancellationToken cancellationToken)
    {
        var housing = await housingRepository.GetHouseInfoByIdAsync(request.HouseListingId, cancellationToken);
        if (housing == null)
        {
            return StandardResponse<int>.Failure(new ApiError("housing.not-found", "House listing not found."));
        }

        if (housing.UserId != request.UserId)
        {
            return StandardResponse<int>.Failure(new ApiError("auth.forbidden", "You do not own this listing."));
        }

        if (housing.HouseListingAuthorization != null)
        {
            return StandardResponse<int>.Failure(new ApiError("housing.authorization.already-exists", "House listing authorization already exists."));
        }

        var auth = housing.HouseListingAuthorization ?? new HouseListingAuthorization();
        
        auth.UserId = request.UserId;
        auth.HouseInfoId = request.HouseListingId;
        auth.FullName = request.FullName;
        auth.OrganizationName = request.OrganizationName;
        auth.Email = request.Email;
        auth.PhoneNumber = request.PhoneNumber;
        
        auth.Availabilities.Clear();
        if (request.Availabilities != null)
        {
            foreach (var slot in request.Availabilities)
            {
                auth.Availabilities.Add(new HouseListingAuthorizationAvailability
                {
                    AvailabilityType = slot.AvailabilityType,
                    Dates = slot.Dates ?? new List<DateOnly>(),
                    TimeFrom = slot.TimeFrom,
                    TimeTo = slot.TimeTo
                });
            }
        }

        auth.AuthorizationType = request.AuthorizationType;
        auth.ListingAuthorizationDocument = request.ListingAuthorizationDocument;
        auth.AuthorizationValidationDate = request.AuthorizationValidationDate;
        auth.SaveThisAuthorizationForFutureListings = request.SaveThisAuthorizationForFutureListings;

        if (request.Attachments != null && request.Attachments.Count != 0)
        {
            auth.Attachments.Clear();
            foreach (var attachment in request.Attachments)
            {
                var fileName = await storageService.SaveFileAsync(attachment, "housing/authorizations", cancellationToken);
                auth.Attachments.Add(new HouseListingAuthorizationAttachment { Url = "@local://" + fileName });
            }
        }

        housing.HouseListingAuthorization = auth;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return StandardResponse<int>.Success(auth.Id);
    }
}
