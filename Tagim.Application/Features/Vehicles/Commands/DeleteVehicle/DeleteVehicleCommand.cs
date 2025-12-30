using MediatR;

namespace Tagim.Application.Features.Vehicles.Commands.DeleteVehicle;

public record DeleteVehicleCommand(Guid PublicId) : IRequest<bool>;