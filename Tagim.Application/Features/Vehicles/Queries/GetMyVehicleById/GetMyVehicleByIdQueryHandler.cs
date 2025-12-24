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
            .FirstOrDefaultAsync(v => v.Id == request.Id && v.UserId == userId && !v.IsDeleted,  cancellationToken);
        
        if (vehicle == null)
            throw new NotFoundException("Avtomobil tapılmadı.");     
        
        return new VehicleDto(
            vehicle.Id,
            vehicle.LicensePlate,
            vehicle.Make,
            vehicle.Model,
            vehicle.Color,
            vehicle.ContactNumber,
            vehicle.VehicleImageUrl
        );
    }
}