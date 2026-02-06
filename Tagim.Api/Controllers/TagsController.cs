using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tagim.Application.Features.Tags.Commands.ActivateTag;
using Tagim.Application.Features.Tags.Commands.CreateBulkTags;
using Tagim.Application.Features.Tags.Queries.GetTagByCode;
using Tagim.Application.Features.Tags.Queries.ScanTag;

namespace Tagim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController(IMediator mediator) : ControllerBase
    {
        [HttpPost("activate")]
        public async Task<IActionResult> Handle(ActivateTagCommand request)
        {
            await mediator.Send(request);
            return Ok(new { Message = "Stiker uğurla aktivləşdirildi! Artıq maşınınız qorunur." });
        }

        [HttpPost("bulk-create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BulkCreate([FromBody] CreateBulkTagsCommand command)
        {
            var count = await mediator.Send(command);
            return Ok(new { Message = $"{count} ədəd yeni stiker uğurla yaradıldı!" });
        }

        [HttpGet("code")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTagInfo(string code)
        {
            var result = await mediator.Send(new GetTagByCodeQuery(code));
            return Ok(result);
        }
        
        [HttpGet("scan/{code}")]
        public async Task<IActionResult> Scan(string code)
        {
            var result = await mediator.Send(new ScanTagQuery(code));
            return Ok(result);
        }
    }
}