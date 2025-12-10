using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tagim.Domain.Common;

namespace Tagim.Infrastructure.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasIndex(x => x.UniqueCode).IsUnique();

        builder.Property(x => x.UniqueCode)
            .IsRequired()
            .HasMaxLength(20);
    }
}