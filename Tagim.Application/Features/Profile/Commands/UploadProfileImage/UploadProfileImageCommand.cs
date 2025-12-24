using MediatR;
using Microsoft.AspNetCore.Http;

namespace Tagim.Application.Features.Profile.Commands.UploadProfileImage;

public class UploadProfileImageCommand : IRequest<string>
{
    public IFormFile File { get; set; }
}