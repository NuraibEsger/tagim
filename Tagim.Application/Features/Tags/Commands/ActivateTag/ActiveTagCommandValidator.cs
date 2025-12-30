using FluentValidation;

namespace Tagim.Application.Features.Tags.Commands.ActivateTag;

public class ActiveTagCommandValidator : AbstractValidator<ActivateTagCommand>
{
    public ActiveTagCommandValidator()
    {
        RuleFor(x => x.UniqueCode)
            .NotEmpty().WithMessage("Stiker kodu boş ola bilməz.")
            .MaximumLength(50).WithMessage("Stiker kodu çox uzundur.");
        
        RuleFor(x => x.VehiclePublicId)
            .NotEmpty().WithMessage("Avtomobil seçilməlidir.")
            .NotEqual(Guid.Empty).WithMessage("Yanlış avtomobil ID-si.");
    }
}