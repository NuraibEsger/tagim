using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Auth.Commands.Login;

public class LoginUserCommandHandler(IApplicationDbContext context, ITokenService token) : IRequestHandler<LoginUserCommand, string>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ITokenService _tokenService = token;
    
    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        
        if (user == null) throw new UnauthorizedAccessException("Email or password is incorrect");
        
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        
        if (!isPasswordValid) throw new UnauthorizedAccessException("Password is incorrect");
        
        return _tokenService.GenerateToken(user);
    }
}