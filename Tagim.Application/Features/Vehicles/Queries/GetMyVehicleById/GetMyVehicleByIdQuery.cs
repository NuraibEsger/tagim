using MediatR;
using Tagim.Application.DTOs;

namespace Tagim.Application.Features.Vehicles.Queries.GetMyVehicleById;

public record GetMyVehicleByIdQuery(int Id) : IRequest<VehicleDto>;
