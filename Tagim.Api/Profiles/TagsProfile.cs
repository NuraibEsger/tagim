using AutoMapper;
using Tagim.Application.DTOs.Tags;
using Tagim.Domain.Common;

namespace Tagim.Api.Profiles;

public class TagsProfile : Profile
{
    public TagsProfile()
    {
        CreateMap<Tag, TagsDto>().ForMember(dest => dest.IsActive,
            opt => opt.MapFrom(x => x.IsActive));
    }
}