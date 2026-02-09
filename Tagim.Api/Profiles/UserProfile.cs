using AutoMapper;
using Tagim.Application.DTOs.User;
using Tagim.Domain.Common;

namespace Tagim.Api.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserProfileDto>().ForMember(dest => dest.SocialLinks,
            opt => opt.MapFrom(x => x.SocialMediaLinks!.Where(y => y.IsVisible)));
    }
}