using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.Exceptions;
using Tagim.Application.Extensions;
using Tagim.Application.Interfaces;
using Tagim.Domain.Common;

namespace Tagim.Application.Features.Profile.Commands.UpdateProfile;

public class UpdateProfileCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<UpdateProfileCommand, bool>
{
    public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserIdOrThrow();

        var user = await context.Users
            .Include(s => s.SocialMediaLinks)
            .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("İstifadəçi", userId);
        }

        user.FullName = request.FullName;
        user.PhoneNumber = request.PhoneNumber.Trim();
        user.UpdatedAt = DateTime.UtcNow;

        if (user.SocialMediaLinks != null)
        {
            var existingLinks = user.SocialMediaLinks.ToList();

            var requestLinkIds = request.SocialMedia
                .Where(x => x.Id.HasValue)
                .Select(x => x.Id!.Value)
                .ToList();

            var linksToDelete = existingLinks
                .Where(dbLink => !requestLinkIds.Contains(dbLink.Id))
                .ToList();

            foreach (var linkToDelete in linksToDelete)
            {
                user.SocialMediaLinks.Remove(linkToDelete);
            }


            foreach (var linkDto in request.SocialMedia)
            {
                if (linkDto.Id is > 0)
                {
                    var existingLink = existingLinks.FirstOrDefault(x => x.Id == linkDto.Id);

                    if (existingLink != null)
                    {
                        existingLink.PlatformName = linkDto.PlatformName;
                        existingLink.Url = linkDto.Url;
                        existingLink.IsVisible = linkDto.IsVisible;
                    }
                }
                else
                {
                    user.SocialMediaLinks.Add(new SocialMediaLink
                    {
                        PlatformName = linkDto.PlatformName,
                        Url = linkDto.Url,
                        IsVisible = linkDto.IsVisible,
                        UserId = userId
                    });
                }
            }
        }

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}