using MediatR;

namespace Tagim.Application.Features.Vehicles.Commands.DeleteVehicle;

public record DeleteVehicleCommand(int Id) : IRequest<bool>;