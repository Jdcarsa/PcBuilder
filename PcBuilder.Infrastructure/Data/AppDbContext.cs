using Microsoft.EntityFrameworkCore;
using PcBuilder.Domain.Entities;

namespace PcBuilder.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Componente> Componentes => Set<Componente>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<ConfiguracionPC> ConfiguracionesPC => Set<ConfiguracionPC>();
    public DbSet<ItemConfiguracion> ItemsConfiguracion => Set<ItemConfiguracion>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItemPedido> ItemsPedido => Set<ItemPedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
