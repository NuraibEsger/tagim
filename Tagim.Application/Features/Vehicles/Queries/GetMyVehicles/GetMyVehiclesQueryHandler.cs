using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.DTOs.Vehicle;
using Tagim.Application.DTOs;
using Tagim.Application.Extensions;
using Tagim.Application.Interfaces;
using Tagim.Domain.Common;

namespace Tagim.Application.Features.Vehicles.Queries.GetMyVehicles;

public class GetMyVehiclesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
    : IRequestHandler<GetMyVehiclesQuery, List<VehicleDto>>
{
    public async Task<List<VehicleDto>> Handle(GetMyVehiclesQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserIdOrThrow();

        var vehicles = await context.Vehicles
            .AsNoTracking()
            .Where(v => v.UserId == userId)
            .OrderByDescending(v => v.CreatedAt)
            .ProjectTo<VehicleDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        return vehicles;
    }
}