using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilder.Domain.Entities;

namespace PcBuilder.Infrastructure.Data.Configurations;

public class ItemConfiguracionConfiguration : IEntityTypeConfiguration<ItemConfiguracion>
{
    public void Configure(EntityTypeBuilder<ItemConfiguracion> builder)
    {
        builder.ToTable("items_configuracion");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .ValueGeneratedOnAdd();

        builder.Property(i => i.ConfiguracionId)
            .IsRequired();

        builder.Property(i => i.Cantidad)
            .IsRequired();

        builder.HasOne(i => i.Componente)
            .WithMany()
            .HasForeignKey(i => i.ComponenteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}