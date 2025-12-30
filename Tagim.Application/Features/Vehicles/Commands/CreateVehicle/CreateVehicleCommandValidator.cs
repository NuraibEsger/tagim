using FluentValidation;

namespace Tagim.Application.Features.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandValidator  : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("Dövlət nömrə nişanı vacibdir.")
            .Matches(@"^[0-9]{2}[- ]?[A-Za-z]{2}[- ]?[0-9]{3}$")
            .WithMessage("Nömrə formatı düzgün deyil (Məs: 90-AZ-100).");

        RuleFor(x => x.Make).NotEmpty().WithMessage("Marka qeyd olunmalıdır.");
        RuleFor(x => x.Model).NotEmpty().WithMessage("Model qeyd olunmalıdır.");
        RuleFor(x => x.Color).NotEmpty().WithMessage("Rəng qeyd olunmalıdır.");
            
        RuleFor(x => x.ContactNumber)
            .NotEmpty()
            .Matches(@"^(\+994|994|0)(50|51|55|60|70|77|99)[0-9]{7}$")
            .WithMessage("Düzgün Azərbaycan nömrəsi daxil edin.");
    }
}