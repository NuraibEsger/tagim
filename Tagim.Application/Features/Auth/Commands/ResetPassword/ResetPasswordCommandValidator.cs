using FluentValidation;

namespace Tagim.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Token).NotEmpty().Length(6).WithMessage("Kod 6 rəqəmli olmalıdır.");
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6).WithMessage("Yeni şifrə ən azı 6 simvol olmalıdır.");
    }
}