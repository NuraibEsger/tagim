using MediatR;
using Tagim.Application.Exceptions;
using Tagim.Application.Extensions;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Profile.Commands.UploadProfileImage;

public class UploadProfileImageCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService,
    IFileStorageService fileStorage)
    : IRequestHandler<UploadProfileImageCommand, string>
{
    public async Task<string> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserIdOrThrow();
        
        var user = await context.Users.FindAsync([userId], cancellationToken);

        if (user == null) throw new NotFoundException("User not found");

        if (!string.IsNullOrEmpty(user.ProfileImageUrl))
        {
            fileStorage.DeleteFile(user.ProfileImageUrl);
        }
        
        var imageUrl = await fileStorage.SaveFileAsync(request.File, "users");
        
        user.ProfileImageUrl = imageUrl;
        await context.SaveChangesAsync(cancellationToken);
        
        return imageUrl;
    }
}