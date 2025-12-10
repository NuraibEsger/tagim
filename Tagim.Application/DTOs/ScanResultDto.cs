namespace Tagim.Application.DTOs;

public record ScanResultDto(
    string LicensePlate,
    string CarDescription,
    string OwnerName,
    string ContactNumber,
    IEnumerable<SocialMediaDto>? SocialLinks); 