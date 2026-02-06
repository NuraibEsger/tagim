using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.DTOs;
using Tagim.Application.DTOs.Vehicle;
using Tagim.Application.Exceptions;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Vehicles.Queries.GetPublicVehicle;

public class GetPublicVehicleQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetPublicVehicleQuery, PublicVehicleDto>
{
    public async Task<PublicVehicleDto> Handle(GetPublicVehicleQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await context.Vehicles
            .AsNoTracking()
            .Include(v => v.User)
            .ThenInclude(v => v.SocialMediaLinks)
            .ProjectTo<PublicVehicleDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(v => v.PublicId == request.PublicId, cancellationToken);
        
        if (vehicle == null) throw new NotFoundException("Avtomobil tapılmadı.");
        
        return vehicle;
    }
}