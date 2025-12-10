using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tagim.Application.Features.Tags.Commands.ActivateTag;
using Tagim.Application.Features.Tags.Commands.CreateBulkTags;
using Tagim.Application.Features.Tags.Queries.GetTagByCode;

namespace Tagim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("activate")]
        public async Task<IActionResult> Handle(ActivateTagCommand request)
        {
            try
            {
                await _mediator.Send(request);
                return Ok(new { Message = "Stiker uğurla aktivləşdirildi! Artıq maşınınız qorunur." });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpPost("bulk-create")]
        public async Task<IActionResult> BulkCreate([FromBody] CreateBulkTagsCommand command)
        {
            try
            {
                var count = await _mediator.Send(command);
                return Ok(new { Message = $"{count} ədəd yeni stiker uğurla yaradıldı!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpGet("code")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTagInfo(string code)
        {
            try
            {
                var result = await _mediator.Send(new GetTagByCodeQuery(code));
                return Ok(result);
            }
            catch (Exception e)
            {
                return NotFound(new { Error = e.Message });
            }
        }
    }
}
