using MediatR;
using Tagim.Application.Exceptions;
using Tagim.Application.Extensions;
using Tagim.Application.Interfaces;
using Tagim.Domain.Common;

namespace Tagim.Application.Features.Profile.Commands.UpdateProfile;

public class UpdateProfileCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<UpdateProfileCommand, bool>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserIdOrThrow();

        var user = await _context.Users
            .FindAsync([userId], cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("İstifadəçi", userId);
        }

        user.FullName = request.FullName;
        user.PhoneNumber = request.PhoneNumber.Trim();
        user.UpdatedAt = DateTime.UtcNow;
        user.SocialMediaLinks = request.SocialMedia
            .Select(dto => new SocialMediaLink
            {
                PlatformName = dto.PlatformName,
                Url = dto.Url,
                IsVisible = dto.IsVisible
            }).ToList();

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}