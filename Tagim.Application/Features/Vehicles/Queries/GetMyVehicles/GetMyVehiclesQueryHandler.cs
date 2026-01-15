using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.DTOs;
using Tagim.Application.Extensions;
using Tagim.Application.Interfaces;
using Tagim.Domain.Common;

namespace Tagim.Application.Features.Vehicles.Queries.GetMyVehicles;

public class GetMyVehiclesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<GetMyVehiclesQuery, List<VehicleDto>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<List<VehicleDto>> Handle(GetMyVehiclesQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserIdOrThrow();

        var vehicles = await _context.Vehicles
            .AsNoTracking()
            .Where(v => v.UserId == userId)
            .OrderByDescending(v => v.CreatedAt)
            .Select(v => new VehicleDto(
                v.Id,
                v.PublicId,
                v.LicensePlate,
                v.Make,
                v.Model,
                v.Color,
                v.ContactNumber,
                v.VehicleImageUrl,
                v.UserId,
                (v.User.SocialMediaLinks ?? Enumerable.Empty<SocialMediaLink>())
                    .Where(sm => sm.IsVisible)
                    .Select(sm => new SocialMediaDto(
                        sm.PlatformName, 
                        sm.Url, 
                        sm.IsVisible
                    )).ToList()
            )).ToListAsync(cancellationToken);
        
        return vehicles;
    }
}