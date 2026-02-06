using Tagim.Domain.Common;

namespace Tagim.Application.DTOs.Vehicle;

public record VehicleDto
{
    public int Id { get; init; }
    public Guid PublicId { get; init; }
    public string LicensePlate { get; init; }
    public string Make { get; init; }
    public string Model { get; init; }
    public string Color { get; init; }
    public string ContactNumber { get; init; }
    public string? VehicleImageUrl { get; init; }
    public int UserId { get; init; }
    public List<SocialMediaDto>? SocialMedia { get; init; }
}