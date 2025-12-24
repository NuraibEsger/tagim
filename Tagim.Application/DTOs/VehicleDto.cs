namespace Tagim.Application.DTOs;

public record VehicleDto(
    int Id,
    string LicensePlate,
    string Make,
    string Model,
    string Color,
    string ContactNumber,
    string? VehicleImageUrl
);