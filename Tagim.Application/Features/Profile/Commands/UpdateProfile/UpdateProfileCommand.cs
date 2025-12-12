using MediatR;

namespace Tagim.Application.Features.Profile.Commands.UpdateProfile;

public class UpdateProfileCommand : IRequest<bool>
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}