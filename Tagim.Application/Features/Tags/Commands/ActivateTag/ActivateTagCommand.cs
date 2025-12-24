using MediatR;

namespace Tagim.Application.Features.Tags.Commands.ActivateTag;

public class ActivateTagCommand : IRequest<bool>
{
    public string UniqueCode { get; set; } = string.Empty;
    public Guid VehiclePublicId { get; set; }
}