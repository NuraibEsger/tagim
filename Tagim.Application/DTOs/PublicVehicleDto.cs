using Tagim.Domain.Common;

namespace Tagim.Application.DTOs;

public record PublicVehicleDto(
    string Make,
    string Model,
    string LicensePlate,
    string Color,
    string ContactNumber,
    string? VehicleImageUrl,
    string UserFullName,
    string? UserProfileImageUrl,
    List<SocialMediaDto>  SocialMediaLinks
);