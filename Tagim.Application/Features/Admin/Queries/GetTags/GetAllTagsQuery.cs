using MediatR;
using Tagim.Application.DTOs.Tags;

namespace Tagim.Application.Features.Admin.Queries.GetTags;

public class GetAllTagsQuery : IRequest<IEnumerable<TagsDto>>
{
    
}