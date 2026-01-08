using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.DTOs;
using Tagim.Application.Exceptions;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Tags.Queries.GetTagByCode;

public class GetTagByCodeQueryHandler(IApplicationDbContext context) : IRequestHandler<GetTagByCodeQuery, ScanResultDto>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<ScanResultDto> Handle(GetTagByCodeQuery request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags
            .AsNoTracking()
            .Include(t => t.Vehicle)
            .ThenInclude(v => v!.User)
            .ThenInclude(u => u.SocialMediaLinks)
            .FirstOrDefaultAsync(t => t.UniqueCode == request.Code, cancellationToken);
        
        if (tag == null)
        {
            throw new NotFoundException("QR Kod tapılmadı.");
        }

        if (!tag.IsActive || tag.Vehicle == null)
        {
            throw new NotFoundException("Bu QR kod hələ aktivləşdirilməyib.");
        }
        
        var socialLinks = tag.Vehicle.User.SocialMediaLinks?
            .Where(s => s.IsVisible) 
            .Select(s => new SocialMediaDto(s.PlatformName, s.Url, true))
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