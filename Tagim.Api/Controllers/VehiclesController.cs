using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tagim.Application.Features.Vehicles.Commands.CreateVehicle;
using Tagim.Application.Features.Vehicles.Commands.DeleteVehicle;
using Tagim.Application.Features.Vehicles.Commands.UpdateVehicle;
using Tagim.Application.Features.Vehicles.Commands.UploadVehicleImage;
using Tagim.Application.Features.Vehicles.Queries.GetMyVehicleById;
using Tagim.Application.Features.Vehicles.Queries.GetMyVehicles;
using Tagim.Application.Features.Vehicles.Queries.GetPublicVehicle;

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
            var vehicleId = await _mediator.Send(command);
            return Ok(new { VehicleId = vehicleId, Message = "Vehicle created successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetMyVehicles()
        {
            var vehicles = await _mediator.Send(new GetMyVehiclesQuery());
            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            var vehicle = await _mediator.Send(new GetMyVehicleByIdQuery(id));
            return Ok(vehicle);
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadVehicleImage([FromForm] UploadVehicleImageCommand command)
        {
            var path = await _mediator.Send(command);
            return Ok(new { ImageUrl = path });
        }
        
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateVehicleCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Avtomobil məlumatları yeniləndi!" });
        }
        
        [HttpGet("vehicle/{publicId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVehicleByPublicId(Guid publicId)
        {
            var vehicle = await _mediator.Send(new GetPublicVehicleQuery(publicId));
            return Ok(vehicle);
        }

        [HttpDelete("{publicId}")]
        public async Task<IActionResult> DeleteVehicleById(DeleteVehicleCommand command)
        {
            var vehicle = await _mediator.Send(command);
            return Ok(new { Message = "Avtomobil məlumatları uğurla silindi."});
        }
    }
}