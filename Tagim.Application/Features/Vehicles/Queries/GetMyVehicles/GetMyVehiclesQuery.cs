using MediatR;
using Tagim.Application.DTOs;

namespace Tagim.Application.Features.Vehicles.Queries.GetMyVehicles;

public record GetMyVehiclesQuery : IRequest<List<VehicleDto>>;