using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.DTOs;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Tags.Queries.ScanTag;

public class ScanTagQueryHandler(IApplicationDbContext context) : IRequestHandler<ScanTagQuery, ScanTagResponseDto>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<ScanTagResponseDto> Handle(ScanTagQuery request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags
            .Include(t => t.Vehicle)
            .ThenInclude(v => v!.User)
            .ThenInclude(u => u.SocialMediaLinks)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.UniqueCode == request.UniqueCode, cancellationToken);

        if (tag == null)
        {
            return new ScanTagResponseDto()
            {
                Status = TagScanStatus.NotFound,
                UniqueCode = request.UniqueCode
            };
        }
        
        if (!tag.IsActive || tag.Vehicle == null)
        {
            return new ScanTagResponseDto 
            { 
                Status = TagScanStatus.ReadyToActivate, 
                UniqueCode = tag.UniqueCode 
            };
        }
        
        var socialLinks = tag.Vehicle.User.SocialMediaLinks?
            .Where(s => s.IsVisible)
            .Select(s => new SocialMediaDto(s.PlatformName, s.Url, true))
            .ToList();

        var vehicleData = new ScanResultDto(
            tag.Vehicle.LicensePlate,
            $"{tag.Vehicle.Make} {tag.Vehicle.Model} ({tag.Vehicle.Color})",
            tag.Vehicle.User.FullName,
            tag.Vehicle.ContactNumber,
            socialLinks
        );

        return new ScanTagResponseDto 
        { 
            Status = TagScanStatus.Active, 
            UniqueCode = tag.UniqueCode,
            VehiclePublicId = tag.Vehicle.PublicId,
            VehicleData = vehicleData
        };
    }
}