using MediatR;
using Tagim.Application.DTOs;

namespace Tagim.Application.Features.Vehicles.Queries.GetPublicVehicle;

public record GetPublicVehicleQuery(Guid PublicId) : IRequest<PublicVehicleDto>;