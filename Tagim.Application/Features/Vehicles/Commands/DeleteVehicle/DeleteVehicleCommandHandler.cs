using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.Exceptions;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Vehicles.Commands.DeleteVehicle;

public class DeleteVehicleCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<DeleteVehicleCommand, bool>
{
    public async Task<bool> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        var vehicle = await context.Vehicles
            .FirstOrDefaultAsync(v => v.PublicId == request.PublicId && v.UserId == userId, cancellationToken);

        if (vehicle == null) throw new NotFoundException("Maşın tapılmadı.");

        var associatedTag = await context.Tags
            .FirstOrDefaultAsync(t => t.VehicleId == vehicle.Id, cancellationToken);

        if (associatedTag != null)
        {
            associatedTag.VehicleId = null; 
            associatedTag.IsActive = false; 
        }

        context.Vehicles.Remove(vehicle);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}