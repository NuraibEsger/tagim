using MediatR;

namespace Tagim.Application.Features.Profile.Commands.AddSocialLink;

public class AddSocialLinkCommand : IRequest<int>
{
    public string PlatformName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}