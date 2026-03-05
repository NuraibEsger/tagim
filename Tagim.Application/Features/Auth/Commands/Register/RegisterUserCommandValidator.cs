using FluentValidation;

namespace Tagim.Application.Features.Auth.Commands.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        // 1. Ad və Soyad (Yalnız hərflər və boşluq)
        RuleFor(p => p.FullName)
            .NotEmpty().WithMessage("Ad və Soyad boş ola bilməz.")
            .MinimumLength(3).WithMessage("Ad və Soyad ən azı 3 simvol olmalıdır.")
            .MaximumLength(50).WithMessage("Ad çox uzundur (maksimum 50 simvol).")
            // Azərbaycan hərflərini də dəstəkləyən Regex
            .Matches(@"^[a-zA-ZəöğüçşƏÖĞÜÇŞ\s]+$").WithMessage("Ad və Soyad yalnız hərflərdən ibarət olmalıdır.");

        // 2. Email
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Email vacibdir.")
            .EmailAddress().WithMessage("Düzgün email formatı daxil edin.");

        // 3. Şifrə (Güclü şifrə siyasəti)
        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("Şifrə vacibdir.")
            .MinimumLength(8).WithMessage("Şifrə ən azı 8 simvol olmalıdır.")
            .MaximumLength(100).WithMessage("Şifrə çox uzundur.")
            .Matches("[A-Z]").WithMessage("Şifrədə ən azı bir böyük hərf olmalıdır.")
            .Matches("[a-z]").WithMessage("Şifrədə ən azı bir kiçik hərf olmalıdır.")
            .Matches("[0-9]").WithMessage("Şifrədə ən azı bir rəqəm olmalıdır.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Şifrədə ən azı bir xüsusi simvol (@, $, !, %, *, ?, &) olmalıdır.");

        // 4. Telefon Nömrəsi (Azərbaycan formatı)
        RuleFor(p => p.PhoneNumber)
            .NotEmpty().WithMessage("Telefon nömrəsi vacibdir.")
            .Matches(@"^(\+994|994|0)(50|51|55|60|70|77|99)[0-9]{7}$")
            .WithMessage("Düzgün Azərbaycan nömrəsi daxil edin (məs: 0501234567).");
    }
}