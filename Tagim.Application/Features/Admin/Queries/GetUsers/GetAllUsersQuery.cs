using MediatR;
using Tagim.Application.DTOs.User;

namespace Tagim.Application.Features.Admin.Queries.GetUsers;

public class GetAllUsersQuery : IRequest<ICollection<UserDto>>;