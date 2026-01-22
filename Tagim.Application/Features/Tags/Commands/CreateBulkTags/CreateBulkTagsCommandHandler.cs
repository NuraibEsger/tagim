using MediatR;
using Tagim.Application.Interfaces;
using Tagim.Domain.Common;

namespace Tagim.Application.Features.Tags.Commands.CreateBulkTags;

public class CreateBulkTagsCommandHandler(
    IApplicationDbContext context, 
    ITagGeneratorService tagGenerator) : IRequestHandler<CreateBulkTagsCommand, int>
{
    public async Task<int> Handle(CreateBulkTagsCommand request, CancellationToken cancellationToken)
    {
        var newTags = new List<Tag>();

        for (int i = 0; i < request.Count; i++)
        {
            newTags.Add(new Tag
            {
                UniqueCode = tagGenerator.GenerateTags(), 
                IsActive = false,                  
                VehicleId = null,                
                CreatedAt = DateTime.UtcNow
            });
        }
        
        await context.Tags.AddRangeAsync(newTags, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return newTags.Count;
    }
}