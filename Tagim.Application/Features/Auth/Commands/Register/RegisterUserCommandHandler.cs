using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.Interfaces;
using Tagim.Domain.Common;

namespace Tagim.Application.Features.Auth.Commands.Register;

public class RegisterUserCommandHandler(IApplicationDbContext context) : IRequestHandler<RegisterUserCommand, int>
{
    public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var exists = await context.Users.AnyAsync(u => 
            u.Email == request.Email || u.PhoneNumber == request.PhoneNumber, cancellationToken);
        
        if (exists)
            throw new Exception("Bu istifadəçi artıq mövcuddur.");
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            PasswordHash = passwordHash,
        };
        
        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);
        
        return user.Id;
    }
}