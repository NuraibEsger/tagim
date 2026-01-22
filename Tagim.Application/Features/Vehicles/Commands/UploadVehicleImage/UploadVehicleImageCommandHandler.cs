using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.Exceptions;
using Tagim.Application.Extensions;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Vehicles.Commands.UploadVehicleImage;

public class UploadVehicleImageCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IFileStorageService fileStorageService)
    : IRequestHandler<UploadVehicleImageCommand, string>
{
    public async Task<string> Handle(UploadVehicleImageCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserIdOrThrow();
        var vehicle = await context.Vehicles.FirstOrDefaultAsync(
            v => v.Id == request.VehicleId && v.UserId == userId, cancellationToken);

        if (vehicle == null) throw new NotFoundException("Maşın tapılmadı");
        
        if (!string.IsNullOrEmpty(vehicle.VehicleImageUrl))
        {
            fileStorageService.DeleteFile(vehicle.VehicleImageUrl);
        }
        
        var imageUrl = await fileStorageService.SaveFileAsync(request.File, "vehicles");
        
        vehicle.VehicleImageUrl = imageUrl;
        await context.SaveChangesAsync(cancellationToken);
        
        return imageUrl;
    }
}