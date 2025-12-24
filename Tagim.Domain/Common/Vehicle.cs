namespace Tagim.Domain.Common;

public class Vehicle : BaseEntity
{
    public string LicensePlate { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string? VehicleImageUrl { get; set; }
    public Guid PublicId { get; set; } =  Guid.NewGuid();
    // This is special number for car
    public string ContactNumber { get; set; } = string.Empty;
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}