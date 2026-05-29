using Microsoft.EntityFrameworkCore;
using PcBuilder.Domain.Entities;
using PcBuilder.Domain.Enums;

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

        // Admin123!
        const string hashAdmin   = "$2a$12$jxUf29zG5KbsXiIZD8g/Vu1ea2ojG5mepPmCYZOdZVDSd26aZy6Da";
        // Cliente123!
        const string hashCliente = "$2a$12$baQPuYecEt6DaYRnCK8dUefh/5THB.LFsFOcUaYwtWKRN3flG37.O";

        modelBuilder.Entity<Usuario>().HasData(
            new
            {
                Id = 1L,
                Nombre = "Admin",
                Apellido = "PcBuilder",
                Email = "admin@pcbuilder.com",
                PasswordHash = hashAdmin,
                Rol = RolUsuario.Administrador,
                EstaActivo = true,
                CreadoEn = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = 2L,
                Nombre = "Juan",
                Apellido = "Pérez",
                Email = "juan@correo.com",
                PasswordHash = hashCliente,
                Rol = RolUsuario.Cliente,
                EstaActivo = true,
                CreadoEn = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        base.OnModelCreating(modelBuilder);
    }
}