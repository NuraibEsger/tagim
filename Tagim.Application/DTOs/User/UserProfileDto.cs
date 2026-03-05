namespace Tagim.Application.DTOs.User;

public record UserProfileDto
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ProfileImageUrl { get; set; }
    public List<SocialMediaDto> SocialLinks { get; set; } = new();
}