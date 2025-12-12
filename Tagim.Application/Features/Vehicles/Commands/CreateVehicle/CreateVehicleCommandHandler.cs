using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.Extensions;
using Tagim.Application.Interfaces;
using Tagim.Domain.Common;

namespace Tagim.Application.Features.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandHandler(
    IApplicationDbContext context, 
    ICurrentUserService currentUserService) : IRequestHandler<CreateVehicleCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;


    public async Task<int> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserIdOrThrow();
        
        var exists = await _context.Vehicles
            .AnyAsync(v => v.LicensePlate == request.LicensePlate, cancellationToken);

        if (exists)
        {
            throw new Exception($"'{request.LicensePlate}' nömrəli avtomobil artıq sistemdə mövcuddur.");
        }
        
        var vehicle = new Vehicle
        {
            UserId = userId,
            LicensePlate = request.LicensePlate.ToUpper(),
            Make = request.Make,
            Model = request.Model,
            Color = request.Color,
            ContactNumber = request.ContactNumber
        };
        
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync(cancellationToken);
        
        return vehicle.Id;
    }
}