using FluentValidation;

namespace Tagim.Application.Features.Auth.Commands.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Email daxil edilməlidir.")
            .EmailAddress().WithMessage("Düzgün email formatı deyil.");

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("Şifrə daxil edilməlidir.");
    }   
}