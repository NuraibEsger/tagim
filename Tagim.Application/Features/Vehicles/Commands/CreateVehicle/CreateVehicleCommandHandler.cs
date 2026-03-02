using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.Extensions;
using Tagim.Application.Interfaces;
using Tagim.Domain.Common;

namespace Tagim.Application.Features.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandHandler(
    IMapper mapper,
    IApplicationDbContext context, 
    ICurrentUserService currentUserService) : IRequestHandler<CreateVehicleCommand, int>
{
    public async Task<int> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserIdOrThrow();
        
        var exists = await context.Vehicles
            .AnyAsync(v => v.LicensePlate.ToLower() == request.LicensePlate.ToLower(), cancellationToken);

        if (exists)
        {
            throw new Exception($"'{request.LicensePlate}' nömrəli avtomobil artıq sistemdə mövcuddur.");
        }
        
        var vehicle = mapper.Map<Vehicle>(request);
        vehicle.UserId = userId;
        
        context.Vehicles.Add(vehicle);
        await context.SaveChangesAsync(cancellationToken);
        
        return vehicle.Id;
    }
}