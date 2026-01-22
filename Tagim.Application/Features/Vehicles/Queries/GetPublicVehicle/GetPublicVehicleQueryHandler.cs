using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.DTOs;
using Tagim.Application.Exceptions;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Vehicles.Queries.GetPublicVehicle;

public class GetPublicVehicleQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetPublicVehicleQuery, PublicVehicleDto>
{
    public async Task<PublicVehicleDto> Handle(GetPublicVehicleQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await context.Vehicles
            .AsNoTracking()
            .Include(v => v.User)  
            .ThenInclude(v => v.SocialMediaLinks)
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.PublicId == request.PublicId, cancellationToken);
        
        if (vehicle == null) throw new NotFoundException("Avtomobil tapılmadı.");
        
        var socialMediaLinks = vehicle.User.SocialMediaLinks?
            .Where(s => s.IsVisible)
            .Select(s => new SocialMediaDto(s.PlatformName, s.Url, true)).ToList() ?? new List<SocialMediaDto>();
        
        return new PublicVehicleDto(
            vehicle.Make,
            vehicle.Model,
            vehicle.LicensePlate,
            vehicle.Color,
            vehicle.ContactNumber,
            vehicle.VehicleImageUrl,
            vehicle.User.FullName,
            vehicle.User.ProfileImageUrl,
            socialMediaLinks
        );
    }
}