using FluentValidation;

namespace Tagim.Application.Features.Profile.Commands.UpdateProfile;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Ad və Soyad boş ola bilməz.")
            .MaximumLength(50).WithMessage("Ad çox uzundur.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Nömrə vacibdir.");
    }
}