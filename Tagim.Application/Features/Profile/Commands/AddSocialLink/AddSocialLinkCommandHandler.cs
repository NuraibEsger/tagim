using MediatR;
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
        var userId = _currentUserService.UserId;
        if (userId == null) throw new UnauthorizedAccessException();

        var link = new SocialMediaLink
        {
            UserId = userId.Value,
            PlatformName = request.PlatformName,
            Url = request.Url,
            IsVisible = true
        };
        
        _context.SocialMediaLinks.Add(link);
        await _context.SaveChangesAsync(cancellationToken);
        
        return link.Id;
    }
}