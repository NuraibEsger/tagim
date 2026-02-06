using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler(IApplicationDbContext context, IEmailService emailService)
    : IRequestHandler<ForgotPasswordCommand, string>
{
    public async Task<string> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken: cancellationToken);

        if (user == null)
        {
            return "Əgər bu email sistemdə varsa, bərpa kodu göndərildi.";
        }

        var token = new Random().Next(100000, 999999).ToString();

        user.PasswordResetToken = user.PasswordHash + token;
        user.PasswordResetTokenExpires = DateTime.UtcNow.AddMinutes(15);
        await context.SaveChangesAsync(cancellationToken);

        string emailBody = $@"
                <h3>Salam, {user.FullName}!</h3>
                <p>Şifrənizi yeniləmək üçün aşağıdakı kodu istifadə edin:</p>
                <h1 style='color:blue'>{token}</h1>
                <p>Bu kod 15 dəqiqə ərzində etibarlıdır.</p>
                <br/>
                <p>Hörmətlə, <b>Tagim Komandası</b></p>
        ";
        
        await emailService.SendEmailAsync(user.Email, "Şifrənin Bərpası", emailBody);
        
        return "Əgər bu email sistemdə varsa, bərpa kodu göndərildi.";
    }
}