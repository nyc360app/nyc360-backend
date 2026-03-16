using NYC360.Domain.Enums.Housing;

namespace NYC360.API.Models.Housing;

public record UpdateHousingRequestStatusRequest(
    int RequestId,
    HousingRequestStatus Status
);
