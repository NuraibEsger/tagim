using MediatR;
using Tagim.Application.DTOs.Vehicle;

namespace Tagim.Application.Features.Vehicles.Queries.GetMyVehicles;

public record GetMyVehiclesQuery : IRequest<List<VehicleDto>>;