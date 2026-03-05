using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.Exceptions;
using Tagim.Application.Extensions;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Profile.Commands.UploadSocialLink;

public class UploadSocialLinkCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService) 
    : IRequestHandler<UpdateSocialLinkCommand, int>
{
    public async Task<int> Handle(UpdateSocialLinkCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserIdOrThrow();
        
        var exists = await context.SocialMediaLinks
            .FirstOrDefaultAsync(s => s.Id == request.SocialLinkId, cancellationToken);

        if (exists is null)
        {
            throw new NotFoundException("Sosial link tapılmadı.");
        }
        
        exists.PlatformName = request.PlatformName;
        exists.Url = request.Url;
        exists.IsVisible = request.IsVisible;


        await context.SaveChangesAsync(cancellationToken);

        return request.SocialLinkId;
    }
}