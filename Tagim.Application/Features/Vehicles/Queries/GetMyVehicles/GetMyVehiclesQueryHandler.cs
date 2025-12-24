using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.DTOs;
using Tagim.Application.Extensions;
using Tagim.Application.Interfaces;

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
            .Where(v => v.UserId == userId && !v.IsDeleted)
            .OrderByDescending(v => v.CreatedAt)
            .Select(v => new VehicleDto(
                v.Id,
                v.LicensePlate,
                v.Make,
                v.Model,
                v.Color,
                v.ContactNumber,
                v.VehicleImageUrl
            )).ToListAsync(cancellationToken);
        
        return vehicles;
    }
}