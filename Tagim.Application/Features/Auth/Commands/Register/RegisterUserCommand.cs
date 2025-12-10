using MediatR;

namespace Tagim.Application.Features.Auth.Commands.Register;

public class RegisterUserCommand : IRequest<int>
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}