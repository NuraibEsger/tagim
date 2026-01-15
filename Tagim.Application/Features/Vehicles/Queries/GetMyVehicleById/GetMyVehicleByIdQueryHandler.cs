using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.DTOs;
using Tagim.Application.Exceptions;
using Tagim.Application.Extensions;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Vehicles.Queries.GetMyVehicleById;

public class GetMyVehicleByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<GetMyVehicleByIdQuery, VehicleDto>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<VehicleDto> Handle(GetMyVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserIdOrThrow();
        
        var vehicle = await _context.Vehicles
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == request.Id && v.UserId == userId,  cancellationToken);
        
        if (vehicle == null)
            throw new NotFoundException("Avtomobil tapılmadı.");

        var socialMedialLinks = vehicle.User.SocialMediaLinks?
            .Where(s => s.IsVisible)
            .Select(s => new SocialMediaDto(s.PlatformName, s.Url, true)).ToList() ?? new List<SocialMediaDto>();
        
        return new VehicleDto(
            vehicle.Id,
            vehicle.PublicId,
            vehicle.LicensePlate,
            vehicle.Make,
            vehicle.Model,
            vehicle.Color,
            vehicle.ContactNumber,
            vehicle.VehicleImageUrl,
            vehicle.UserId,
            socialMedialLinks
        );
    }
}