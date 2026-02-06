using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.DTOs;
using Tagim.Application.DTOs.Vehicle;
using Tagim.Application.Exceptions;
using Tagim.Application.Extensions;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Vehicles.Queries.GetMyVehicleById;

public class GetMyVehicleByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
    : IRequestHandler<GetMyVehicleByIdQuery, VehicleDto>
{
    public async Task<VehicleDto> Handle(GetMyVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserIdOrThrow();
        
        var vehicle = await context.Vehicles
            .AsNoTracking()
            .Include(v => v.User)
            .ThenInclude(u => u.SocialMediaLinks)
            .ProjectTo<VehicleDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(v => v.Id == request.Id && v.UserId == userId,  cancellationToken);
        
        if (vehicle == null)
            throw new NotFoundException("Avtomobil tapılmadı.");
        
        return  vehicle;
    }
}