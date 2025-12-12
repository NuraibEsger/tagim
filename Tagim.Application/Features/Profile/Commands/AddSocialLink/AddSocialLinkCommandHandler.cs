using MediatR;
using Tagim.Application.Extensions;
using Tagim.Application.Interfaces;
using Tagim.Domain.Common;

namespace Tagim.Application.Features.Profile.Commands.AddSocialLink;

public class AddSocialLinkCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<AddSocialLinkCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<int> Handle(AddSocialLinkCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserIdOrThrow();

        var link = new SocialMediaLink
        {
            UserId = userId,
            PlatformName = request.PlatformName,
            Url = request.Url,
            IsVisible = true
        };
        
        _context.SocialMediaLinks.Add(link);
        await _context.SaveChangesAsync(cancellationToken);
        
        return link.Id;
    }
}