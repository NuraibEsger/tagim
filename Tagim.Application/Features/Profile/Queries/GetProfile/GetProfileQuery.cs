using MediatR;
using Tagim.Application.DTOs.User;

namespace Tagim.Application.Features.Profile.Queries.GetProfile;

public record GetProfileQuery : IRequest<UserProfileDto>;