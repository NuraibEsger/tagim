using FluentValidation;
using Tagim.Application.Extensions;

namespace Tagim.Application.Features.Vehicles.Commands.UploadVehicleImage;

public class UploadVehicleImageCommandValidator : AbstractValidator<UploadVehicleImageCommand>
{
    public UploadVehicleImageCommandValidator()
    {
        RuleFor(x => x.VehicleId).GreaterThan(0).WithMessage("Yanlış maşın ID-si.");
        RuleFor(x => x.File).IsValidImage();
    }
}