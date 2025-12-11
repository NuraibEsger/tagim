using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tagim.Application.Features.Vehicles.Commands.CreateVehicle;
using Tagim.Application.Features.Vehicles.Queries.GetMyVehicles;

namespace Tagim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleCommand command)
        {
            try
            {
                var vehicleId = await _mediator.Send(command);
                return Ok(new { VehicleId = vehicleId, Message = "Vehicle created successfully" });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetMyVehicles()
        {
            var vehicles = await _mediator.Send(new GetMyVehiclesQuery());
            return Ok(vehicles);
        }
    }
}
