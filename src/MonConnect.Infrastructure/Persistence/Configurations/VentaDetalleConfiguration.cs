
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonConnect.Domain.Entities;

namespace MonConnect.Infrastructure.Persistence.Configurations;

public class VentaDetalleConfiguration : IEntityTypeConfiguration<VentaDetalle>
{
    public void Configure(EntityTypeBuilder<VentaDetalle> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne( x => x.Producto)
            .WithMany()
            .HasForeignKey(x=> x.ProductoId);
    }
}