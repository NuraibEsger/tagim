using AutoMapper;
using Tagim.Application.DTOs.Vehicle;
using Tagim.Application.Features.Vehicles.Commands.CreateVehicle;
using Tagim.Application.Features.Vehicles.Commands.UpdateVehicle;
using Tagim.Domain.Common;

namespace Tagim.Api.Profiles;

public class VehicleProfile : Profile
{
    public VehicleProfile()
    {
        CreateMap<Vehicle, VehicleDto>()
            .ForMember(dest => dest.SocialMedia, 
                opt => opt.MapFrom(src => src.User.SocialMediaLinks!.Where(x => x.IsVisible)));
        CreateMap<Vehicle, PublicVehicleDto>()
            .ForMember(dest => dest.SocialMediaLinks, 
                opt => opt.MapFrom(src => src.User.SocialMediaLinks!.Where(x => x.IsVisible)));
        CreateMap<CreateVehicleCommand, Vehicle>()
            .ForMember(v => v.LicensePlate,
                opt => opt.MapFrom(v => v.LicensePlate.ToUpper()));
        CreateMap<UpdateVehicleCommand, Vehicle>()
            .ForMember(v => v.LicensePlate, 
                opt => opt.MapFrom(v => v.LicensePlate.ToUpper()));
    }
}