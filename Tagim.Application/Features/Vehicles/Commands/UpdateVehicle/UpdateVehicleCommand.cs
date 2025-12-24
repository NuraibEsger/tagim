using MediatR;

namespace Tagim.Application.Features.Vehicles.Commands.UpdateVehicle;

public class UpdateVehicleCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
}