namespace Tagim.Domain.Common;

public class SocialMediaLink : BaseEntity
{
    public string PlatformName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}