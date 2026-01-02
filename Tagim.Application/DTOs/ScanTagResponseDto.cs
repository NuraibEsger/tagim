namespace Tagim.Application.DTOs;

public record ScanTagResponseDto()
{
    public TagScanStatus Status { get; init; }
    public string UniqueCode { get; init; }
    public Guid? VehiclePublicId { get; init; }
    public ScanResultDto? VehicleData { get; init; }
}