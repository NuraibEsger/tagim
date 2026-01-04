using FluentValidation;

namespace Tagim.Application.Features.Profile.Commands.UpdateProfile;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Ad və Soyad boş ola bilməz.")
            .MinimumLength(3).WithMessage("Ad çox qısadır.")
            .MaximumLength(50).WithMessage("Ad 50 simvoldan çox ola bilməz.");

        RuleFor(x => x.PhoneNumber.Trim())
            .NotEmpty().WithMessage("Telefon nömrəsi daxil edilməlidir.")
            .Matches(@"^(\+994|994|0)(50|51|55|60|70|77|99)[0-9]{7}$")
            .WithMessage("Düzgün Azərbaycan nömrəsi daxil edin (məs: 0501234567).");
    }
}