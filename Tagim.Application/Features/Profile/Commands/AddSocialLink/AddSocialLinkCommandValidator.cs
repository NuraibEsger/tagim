using FluentValidation;

namespace Tagim.Application.Features.Profile.Commands.AddSocialLink;

public class AddSocialLinkCommandValidator :  AbstractValidator<AddSocialLinkCommand>
{
    public AddSocialLinkCommandValidator()
    {
        RuleFor(p => p.Url)
            .Must(BeAValidUrl).WithMessage("'{PropertyValue}' is not a valid URL.");
    }

    private bool BeAValidUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return true;
        }

        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}