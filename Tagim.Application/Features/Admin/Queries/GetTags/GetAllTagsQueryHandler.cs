using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.DTOs.Tags;
using Tagim.Application.Interfaces;
using Tagim.Domain.Common;

namespace Tagim.Application.Features.Admin.Queries.GetTags;

public class GetAllTagsQueryHandler(IApplicationDbContext dbContext, IMapper mapper) 
    : IRequestHandler<GetAllTagsQuery, IEnumerable<TagsDto>>
{
    public async Task<IEnumerable<TagsDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TagsDto> tags = await dbContext.Tags
            .AsNoTracking()
            .IgnoreQueryFilters()
            .ProjectTo<TagsDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        return tags;
    }
}