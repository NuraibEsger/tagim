namespace Tagim.Domain.Common;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    public ICollection<SocialMediaLink>?  SocialMediaLinks { get; set; } = new List<SocialMediaLink>();
}