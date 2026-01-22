using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.Exceptions;
using Tagim.Application.Extensions;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Vehicles.Commands.UpdateVehicle;

public class UpdateVehicleCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<UpdateVehicleCommand, Unit>
{
    public async Task<Unit> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserIdOrThrow();
        
        var vehicle = await context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == request.Id && v.UserId == userId, cancellationToken);
        
        if (vehicle == null) throw new NotFoundException("Avtomobil",  request.Id);
        
        vehicle.LicensePlate = request.LicensePlate.ToUpper();
        vehicle.Make = request.Make;
        vehicle.Model = request.Model;
        vehicle.Color = request.Color;
        vehicle.ContactNumber = request.ContactNumber;
        vehicle.UpdatedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}