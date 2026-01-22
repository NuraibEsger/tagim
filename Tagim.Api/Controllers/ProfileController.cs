using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tagim.Application.Features.Profile.Commands.AddSocialLink;
using Tagim.Application.Features.Profile.Commands.UpdateProfile;
using Tagim.Application.Features.Profile.Commands.UploadProfileImage;

namespace Tagim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController(IMediator mediator) : ControllerBase
    {
        [HttpPost("social-links")]
        public async Task<IActionResult> AddSocialLink([FromBody] AddSocialLinkCommand command)
        {
            var id = await mediator.Send(command);
            return Ok(new { Id = id, Message = "Sosial media linki əlavə edildi." });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
        {
            await mediator.Send(command);
            return Ok(new { Message = "Profil məlumatları uğurla yeniləndi!" });
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] UploadProfileImageCommand command)
        {
            var path = await mediator.Send(command);
            return Ok(new { ImageUrl = path });
        }
    }
}