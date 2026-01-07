
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonConnect.Domain.Entities;

namespace MonConnect.Infrastructure.Persistence.Configurations;

public class VentaConfiguration : IEntityTypeConfiguration<Venta>
{
    public void Configure(EntityTypeBuilder<Venta> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Detalles)
            .WithOne(x => x.Venta)
            .HasForeignKey(x => x.VentaId);
    }
}