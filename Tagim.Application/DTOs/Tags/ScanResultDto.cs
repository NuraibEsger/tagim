using Tagim.Application.DTOs.SocialMedias;

namespace Tagim.Application.DTOs.Tags;

public record ScanResultDto(
    string LicensePlate,
    string CarDescription,
    string OwnerName,
    string ContactNumber,
    IEnumerable<SocialMediaDto>? SocialLinks); 