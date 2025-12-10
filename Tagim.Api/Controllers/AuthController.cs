using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tagim.Application.Features.Auth.Commands.Login;
using Tagim.Application.Features.Auth.Commands.Register;

namespace Tagim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            try
            {
                var userId = await mediator.Send(command);
                return Ok(new { UserId = userId, Message = "User created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            try
            {
                var token = await mediator.Send(command);
                return Ok(new { Token = token });
            }
            catch (Exception e)
            {
                return Unauthorized(new { Error = e.Message });
            }
        }
    }
}
