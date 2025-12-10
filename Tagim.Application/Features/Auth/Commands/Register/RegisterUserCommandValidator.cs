using FluentValidation;

namespace Tagim.Application.Features.Auth.Commands.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(p => p.FullName)
            .NotEmpty().WithMessage("Ad və Soyad boş ola bilməz.")
            .MaximumLength(50).WithMessage("Ad çox uzundur (maks 50 simvol).");

        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Email vacibdir.")
            .EmailAddress().WithMessage("Düzgün email formatı daxil edin.");

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("Şifrə vacibdir.")
            .MinimumLength(6).WithMessage("Şifrə ən azı 6 simvol olmalıdır.");

        RuleFor(p => p.PhoneNumber)
            .NotEmpty().WithMessage("Nömrə vacibdir.");
    }
}