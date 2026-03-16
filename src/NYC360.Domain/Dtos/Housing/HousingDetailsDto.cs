namespace NYC360.Domain.Dtos.Housing;

public record HousingDetailsDto(
    HousingDto Info,
    List<AvailabilitySlotResponseDto> Availabilities,
    HousingRequestDto? Request,
    List<HousingMinimalDto> Similar
);