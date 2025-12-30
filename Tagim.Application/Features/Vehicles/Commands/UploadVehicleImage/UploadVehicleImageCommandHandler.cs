using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.Exceptions;
using Tagim.Application.Extensions;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Vehicles.Commands.UploadVehicleImage;

public class UploadVehicleImageCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IFileStorageService fileStorageService)
    : IRequestHandler<UploadVehicleImageCommand, string>
{
    private readonly IApplicationDbContext  _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IFileStorageService _fileStorageService = fileStorageService;

    public async Task<string> Handle(UploadVehicleImageCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserIdOrThrow();
        var vehicle = await _context.Vehicles.FirstOrDefaultAsync(
            v => v.Id == request.VehicleId && v.UserId == userId, cancellationToken);

        if (vehicle == null) throw new NotFoundException("Maşın tapılmadı");
        
        if (!string.IsNullOrEmpty(vehicle.VehicleImageUrl))
        {
            _fileStorageService.DeleteFile(vehicle.VehicleImageUrl);
        }
        
        var imageUrl = await _fileStorageService.SaveFileAsync(request.File, "vehicles");
        
        vehicle.VehicleImageUrl = imageUrl;
        await _context.SaveChangesAsync(cancellationToken);
        
        return imageUrl;
    }
}