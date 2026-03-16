namespace Tagim.Application.DTOs.Tags;

public class TagsDto
{
    public int Id { get; set; }
    public TagScanStatus Status { get; init; }
    public string UniqueCode { get; init; }
    public bool IsActive { get; set; }
    public Guid? VehiclePublicId { get; init; }
    public ScanResultDto? VehicleData { get; init; }
}