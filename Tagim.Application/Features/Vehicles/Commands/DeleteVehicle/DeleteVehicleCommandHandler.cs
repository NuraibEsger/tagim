using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.Exceptions;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Vehicles.Commands.DeleteVehicle;

public class DeleteVehicleCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<DeleteVehicleCommand, bool>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<bool> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.PublicId == request.PublicId && v.UserId == userId, cancellationToken);

        if (vehicle == null) throw new NotFoundException("Maşın tapılmadı.");

        var associatedTag = await _context.Tags
            .FirstOrDefaultAsync(t => t.VehicleId == vehicle.Id, cancellationToken);

        if (associatedTag != null)
        {
            associatedTag.VehicleId = null; 
            associatedTag.IsActive = false; 
        }

        _context.Vehicles.Remove(vehicle);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}