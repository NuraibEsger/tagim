using Tagim.Domain.Enums;

namespace Tagim.Domain.Common;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpires { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public ICollection<SocialMediaLink>?  SocialMediaLinks { get; set; } = new List<SocialMediaLink>();
}