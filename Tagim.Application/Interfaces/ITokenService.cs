using Tagim.Domain.Common;

namespace Tagim.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}