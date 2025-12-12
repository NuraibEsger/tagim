using Tagim.Application.Interfaces;

namespace Tagim.Application.Extensions;

public static class CurrentUserServiceExtensions
{
    public static int GetUserIdOrThrow(this ICurrentUserService service)
    {
        if (service.UserId == null)
        {
            throw new UnauthorizedAccessException("Bu əməliyyat üçün giriş etməlisiniz.");
        }
        
        return service.UserId.Value;
    }
}