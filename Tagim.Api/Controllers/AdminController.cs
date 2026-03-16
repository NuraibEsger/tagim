using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tagim.Application.Features.Admin.Queries.GetTags;
using Tagim.Application.Features.Admin.Queries.GetUsers;

namespace Tagim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController(IMediator mediator) : ControllerBase
    {
        [HttpGet("/get-users")]
        public async Task<IActionResult> Admin()
        {
            var users = await mediator.Send(new GetAllUsersQuery());
            return Ok(users);
        }

        [HttpGet("/get-users/{id}")]
        public async Task<IActionResult> GetUsers(Guid id)
        {
            return Ok();
        }

        [HttpGet("/get-tags")]
        public async Task<IActionResult> GetTags()
        {
            var tags = await mediator.Send(new GetAllTagsQuery());
            return Ok(tags);
        }
    }
}
