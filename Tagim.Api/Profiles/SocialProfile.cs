using AutoMapper;
using Tagim.Application.DTOs.SocialMedias;
using Tagim.Domain.Common;

namespace Tagim.Api.Profiles;

public class SocialProfile : Profile
{
    public SocialProfile()
    {
        CreateMap<SocialMediaLink, SocialMediaDto>();
    }
}