using FluentValidation;
using Tagim.Application.Features.Profile.Commands.UploadSocialLink;

namespace Tagim.Application.Features.Profile.Commands.UpdateProfile;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(p => p.FullName)
            .NotEmpty().WithMessage("Ad və Soyad boş ola bilməz.")
            .MinimumLength(3).WithMessage("Ad və Soyad ən azı 3 simvol olmalıdır.")
            .MaximumLength(50).WithMessage("Ad çox uzundur (maksimum 50 simvol).")
            .Matches(@"^[a-zA-ZəöğüçşƏÖĞÜÇŞ\s]+$").WithMessage("Ad və Soyad yalnız hərflərdən ibarət olmalıdır.");

        RuleFor(x => x.PhoneNumber.Trim())
            .NotEmpty().WithMessage("Telefon nömrəsi daxil edilməlidir.")
            .Matches(@"^(\+994|994|0)(50|51|55|60|70|77|99)[0-9]{7}$")
            .WithMessage("Düzgün Azərbaycan nömrəsi daxil edin (məs: 0501234567).");

        RuleForEach(x => x.SocialMedia)
            .SetValidator(new SocialMediaDtoValidator());
    }
}