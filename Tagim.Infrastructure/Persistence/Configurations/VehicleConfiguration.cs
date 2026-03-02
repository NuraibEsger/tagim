using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tagim.Domain.Common;

namespace Tagim.Infrastructure.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasIndex(x => x.LicensePlate).IsUnique().HasFilter("\"IsDeleted\" IS FALSE");
        builder.Property(x => x.LicensePlate).HasMaxLength(20).IsRequired();
    }
}