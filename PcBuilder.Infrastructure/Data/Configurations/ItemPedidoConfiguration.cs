using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilder.Domain.Entities;

namespace PcBuilder.Infrastructure.Data.Configurations;

public class ItemPedidoConfiguration : IEntityTypeConfiguration<ItemPedido>
{
    public void Configure(EntityTypeBuilder<ItemPedido> builder)
    {
        builder.ToTable("items_pedido");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .ValueGeneratedOnAdd();

        builder.Property(i => i.PedidoId)
            .IsRequired();

        builder.Property(i => i.NombreComponente)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(i => i.PrecioUnitario)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(i => i.Cantidad)
            .IsRequired();

        builder.Ignore(i => i.Subtotal);

        builder.HasOne(i => i.Componente)
            .WithMany()
            .HasForeignKey(i => i.ComponenteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}