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
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IFileStorageService _fileStorage = fileStorage;

    public async Task<string> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserIdOrThrow();
        
        var user = await _context.Users.FindAsync([userId], cancellationToken);

        if (user == null) throw new NotFoundException("User not found");

        if (!string.IsNullOrEmpty(user.ProfileImageUrl))
        {
            _fileStorage.DeleteFile(user.ProfileImageUrl);
        }
        
        var imageUrl = await _fileStorage.SaveFileAsync(request.File, "users");
        
        user.ProfileImageUrl = imageUrl;
        await _context.SaveChangesAsync(cancellationToken);
        
        return imageUrl;
    }
}