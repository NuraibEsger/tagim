using Tagim.Domain.Common;

namespace Tagim.Application.DTOs.Vehicle;

public record PublicVehicleDto
{
    public Guid PublicId { get; set; }
    public string Make {get; set;}
    public string Model {get; set;}
    public string LicensePlate {get; set;}
    public string Color {get; set;}
    public string ContactNumber {get; set; }
    public string? VehicleImageUrl { get; set; }
    public string UserFullName {get; set;}
    public string? UserProfileImageUrl { get; set;}
    public List<SocialMediaDto> SocialMediaLinks { get; init; }
};