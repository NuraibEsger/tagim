using FluentValidation;

namespace Tagim.Application.Features.Auth.Commands.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        // 1. Email yoxlanışı
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Email daxil edilməlidir.")
            .EmailAddress().WithMessage("Düzgün email formatı daxil edin.");

        // 2. Şifrə yoxlanışı (Sərt qaydalar YOXDUR, sadəcə boş olmaması yoxlanılır)
        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("Şifrə daxil edilməlidir.")
            .MaximumLength(100).WithMessage("Şifrə çox uzundur."); 
    }
}