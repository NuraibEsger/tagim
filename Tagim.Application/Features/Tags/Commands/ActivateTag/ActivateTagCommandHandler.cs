using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Tags.Commands.ActivateTag;

public class ActivateTagCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<ActivateTagCommand, bool>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<bool> Handle(ActivateTagCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        
        var tag = await _context.Tags
            .FirstOrDefaultAsync(t => t.UniqueCode == request.UniqueCode, cancellationToken);
 
        if (tag == null)
        {
            throw new Exception("Yanlış kod! Belə bir stiker mövcud deyil.");
        }

        if (tag.IsActive || tag.VehicleId != null)
        {
            throw new Exception("Bu stiker artıq istifadə olunub!");
        }
        
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == request.VehicleId && v.UserId == userId, cancellationToken);
        
        if (vehicle == null)
        {
            throw new Exception("Maşın tapılmadı və ya sizə aid deyil.");
        }
        
        tag.VehicleId = vehicle.Id;
        tag.IsActive = true;
        tag.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}