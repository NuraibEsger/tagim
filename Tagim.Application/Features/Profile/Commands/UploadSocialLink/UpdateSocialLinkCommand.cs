using MediatR;
using Tagim.Application.DTOs;

namespace Tagim.Application.Features.Profile.Commands.UploadSocialLink;

public class UploadSocialLinkCommand : IRequest<int>
{
    public int SocialLinkId { get; set; }
    public string PlatformName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool IsVisible  { get; set; }
}