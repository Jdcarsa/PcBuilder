using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilder.Domain.Entities;

namespace PcBuilder.Infrastructure.Data.Configurations;

public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("pedidos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.NumeroPedido)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(p => p.NumeroPedido)
            .IsUnique();

        builder.Property(p => p.Estado)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(p => p.DireccionEnvio)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(p => p.NotasCliente)
            .HasMaxLength(1000);

        builder.Property(p => p.NumeroGuia)
            .HasMaxLength(100);

        builder.Property(p => p.CreadoEn)
            .IsRequired();

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Items)
            .WithOne()
            .HasForeignKey(i => i.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(p => p.Total);

        builder.Navigation(p => p.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_items");
    }
}


