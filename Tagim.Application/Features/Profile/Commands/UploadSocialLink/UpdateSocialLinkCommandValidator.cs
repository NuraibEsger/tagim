using FluentValidation;

namespace Tagim.Application.Features.Profile.Commands.UploadSocialLink;

public class UpdateSocialLinkCommandValidator  : AbstractValidator<UpdateSocialLinkCommand>
{
    public UpdateSocialLinkCommandValidator()
    {
        // 1. ID yoxlanışı
        RuleFor(x => x.SocialLinkId)
            .GreaterThan(0)
            .WithMessage("Keçərli bir link ID-si göndərilməlidir.");

        // 2. Platforma adı yoxlanışı
        RuleFor(x => x.PlatformName)
            .NotEmpty().WithMessage("Platforma adı boş ola bilməz.")
            .MaximumLength(50).WithMessage("Platforma adı ən çox 50 simvol ola bilər.");

        // 3. URL yoxlanışı
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("URL boş ola bilməz.")
            .MaximumLength(500).WithMessage("URL çox uzundur (Maksimum 500 simvol).")
            .Must(BeAValidUrl).WithMessage("Zəhmət olmasa, düzgün bir veb ünvanı daxil edin (məs: https://instagram.com/adiniz).");
    }
    
    // URL-in həqiqətən keçərli olub-olmadığını yoxlayan xüsusi metod
    private bool BeAValidUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return false;

        // 1. Uri kimi formatı düzgündürmü?
        bool isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out var outUri);
        
        if (!isValidUrl) return false;
        
        // 2. HTTP/HTTPS dirmi?
        bool isHttpOrHttps = outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps;
        
        // 3. ƏN VACİBİ: Host adında nöqtə varmı? (Məsələn, "example" -> false, "example.com" -> true)
        bool hasDomainExtension = outUri.Host.Contains('.'); 
        
        return  isHttpOrHttps && hasDomainExtension;
    }
}