using FluentValidation;
using Tagim.Application.Extensions;

namespace Tagim.Application.Features.Profile.Commands.UploadProfileImage;

public class UploadProfileImageCommandValidator : AbstractValidator<UploadProfileImageCommand>
{
    public UploadProfileImageCommandValidator()
    {
        RuleFor(x => x.File).IsValidImage();
    }
}