using MediatR;
using Tagim.Application.DTOs;

namespace Tagim.Application.Features.Vehicles.Queries.GetMyVehicles;

public class GetMyVehiclesQuery : IRequest<List<VehicleDto>>
{
    
}