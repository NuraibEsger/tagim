using FluentValidation;

namespace Tagim.Application.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email ünvanı daxil edilməlidir.")
            .EmailAddress().WithMessage("Düzgün email formatı daxil edin.");
    }
}