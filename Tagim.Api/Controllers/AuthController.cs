using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tagim.Application.Features.Auth.Commands.ForgotPassword;
using Tagim.Application.Features.Auth.Commands.Login;
using Tagim.Application.Features.Auth.Commands.Register;
using Tagim.Application.Features.Auth.Commands.ResetPassword;

namespace Tagim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var userId = await mediator.Send(command);
            return Ok(new { UserId = userId, Message = "User created successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var token = await mediator.Send(command);
            return Ok(new { Token = token });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            var token = await mediator.Send(command);
            return Ok(new { Token = token });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var token = await mediator.Send(command);
            return Ok(new { Token = token });
        }
    }
}