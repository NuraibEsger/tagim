using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tagim.Application.Features.Profile.Commands.AddSocialLink;
using Tagim.Application.Features.Profile.Commands.UpdateProfile;

namespace Tagim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost("social-links")]
        public async Task<IActionResult> AddSocialLink([FromBody] AddSocialLinkCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                return Ok(new { Id = id, Message = "Sosial media linki əlavə edildi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Profil məlumatları uğurla yeniləndi!" });
        }
    }
}
