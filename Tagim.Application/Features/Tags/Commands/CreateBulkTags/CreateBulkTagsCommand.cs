using MediatR;

namespace Tagim.Application.Features.Tags.Commands.CreateBulkTags;

public class CreateBulkTagsCommand : IRequest<int>
{
    public int Count { get; set; }
}