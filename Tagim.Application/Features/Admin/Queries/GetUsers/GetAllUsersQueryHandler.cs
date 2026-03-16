using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.DTOs.User;
using Tagim.Application.Interfaces;
using Tagim.Domain.Common;

namespace Tagim.Application.Features.Admin.Queries.GetUsers;

public class GetAllUsersQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : IRequestHandler<GetAllUsersQuery, ICollection<UserDto>>
{
    public async Task<ICollection<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        ICollection<UserDto> users = await dbContext.Users
            .AsNoTracking()
            .IgnoreQueryFilters()
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return users;
    }
}