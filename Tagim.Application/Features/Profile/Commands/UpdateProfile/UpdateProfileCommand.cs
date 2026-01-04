using MediatR;
using Tagim.Application.DTOs;

namespace Tagim.Application.Features.Profile.Commands.UpdateProfile;

public class UpdateProfileCommand : IRequest<bool>
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public List<SocialMediaDto> SocialMedia { get; set; } = [];
}