namespace Tagim.Domain.Common;

public class Tag : BaseEntity
{
    public string UniqueCode { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int? VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; } = null!;
}