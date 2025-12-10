using MediatR;

namespace Tagim.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommand : IRequest<string>
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}