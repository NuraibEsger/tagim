using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.DTOs.User;
using Tagim.Application.Exceptions;
using Tagim.Application.Extensions;
using Tagim.Application.Interfaces;

namespace Tagim.Application.Features.Profile.Queries.GetProfile;

public class GetProfileQueryHandler(ICurrentUserService currentUserService, IMapper mapper, IApplicationDbContext context) 
    : IRequestHandler<GetProfileQuery, UserProfileDto>
{
    public async Task<UserProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserIdOrThrow();

        var user = await context.Users
            .AsNoTracking()
            .Include(u => u.SocialMediaLinks)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null) throw new NotFoundException("İstifadəçi tapılmadı");
        
        return mapper.Map<UserProfileDto>(user);
    }
}