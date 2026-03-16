using Tagim.Application.DTOs.SocialMedias;

namespace Tagim.Application.DTOs.User;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string? ProfileImageUrl { get; set; }
    public ICollection<SocialMediaDto> SocialMedias { get; set; } = new List<SocialMediaDto>();
}