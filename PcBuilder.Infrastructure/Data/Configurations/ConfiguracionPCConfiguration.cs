using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilder.Domain.Entities;

public class ConfiguracionPCConfiguration : IEntityTypeConfiguration<ConfiguracionPC>
{
    public void Configure(EntityTypeBuilder<ConfiguracionPC> builder)
    {
        builder.ToTable("configuraciones_pc");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nombre)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.EstaFinalizada)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(c => c.CreadoEn)
            .IsRequired();

        builder.HasOne<Usuario>()
            .WithMany()
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Items)
            .WithOne()
            .HasForeignKey(i => i.ConfiguracionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(c => c.PrecioTotal);
        builder.Ignore(c => c.ConsumoTotalWatts);

        builder.Navigation(c => c.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_items");
    }
}