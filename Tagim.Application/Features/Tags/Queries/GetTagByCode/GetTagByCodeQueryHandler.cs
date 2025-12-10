using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.DTOs;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Tags.Queries.GetTagByCode;

public class GetTagByCodeQueryHandler(IApplicationDbContext context) : IRequestHandler<GetTagByCodeQuery, ScanResultDto>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<ScanResultDto> Handle(GetTagByCodeQuery request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags
            .Include(t => t.Vehicle)
            .ThenInclude(v => v!.User)
            .ThenInclude(u => u.SocialMediaLinks)
            .FirstOrDefaultAsync(t => t.UniqueCode == request.Code, cancellationToken);
        
        if (tag == null)
        {
            throw new Exception("QR Kod tapılmadı.");
        }

        if (!tag.IsActive || tag.Vehicle == null)
        {
            throw new Exception("Bu QR kod hələ aktivləşdirilməyib.");
        }
        
        var socialLinks = tag.Vehicle.User.SocialMediaLinks?
            .Where(s => s.IsVisible && !s.IsDeleted) 
            .Select(s => new SocialMediaDto(s.PlatformName, s.Url))
            .ToList();

        return new ScanResultDto(
            tag.Vehicle.LicensePlate,
            $"{tag.Vehicle.Make} {tag.Vehicle.Model} ({tag.Vehicle.Color})",
            tag.Vehicle.User.FullName,
            tag.Vehicle.ContactNumber,
            socialLinks
        );
    }
}