using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler(IApplicationDbContext context) : IRequestHandler<ResetPasswordCommand, string>
{
    public async Task<string> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (user == null) 
            throw new Exception("İstifadəçi tapılmadı.");

        if (request.Token != user.PasswordResetToken)
            throw new Exception("Kod yanlışdır.");
        
        if (user.PasswordResetTokenExpires < DateTime.UtcNow) 
            throw new Exception("Kodun vaxtı bitib. Zəhmət olmasa yenidən cəhd edin.");
        
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.PasswordHash = passwordHash;
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpires = null;
        
        await context.SaveChangesAsync(cancellationToken);
        
        return "Şifrəniz uğurla yeniləndi! İndi giriş edə bilərsiniz.";
    }
}