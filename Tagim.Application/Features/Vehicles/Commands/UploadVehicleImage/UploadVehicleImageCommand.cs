using MediatR;
using Microsoft.AspNetCore.Http;

namespace Tagim.Application.Features.Vehicles.Commands.UploadVehicleImage;

public class UploadVehicleImageCommand : IRequest<string>
{
    public int VehicleId { get; set; }
    public IFormFile File { get; set; }
}