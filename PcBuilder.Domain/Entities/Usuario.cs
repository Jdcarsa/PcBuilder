using PcBuilder.Domain.Enums;

namespace PcBuilder.Domain.Entities;

public class Usuario
{
    public long Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Apellido { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public RolUsuario Rol { get; private set; }
    public bool EstaActivo { get; private set; }
    public DateTime CreadoEn { get; private set; }

    public string NombreCompleto => $"{Nombre} {Apellido}";

    private Usuario() { }

    public static Usuario Crear(string nombre, string apellido, string email, string passwordHash,
        RolUsuario rol = RolUsuario.Cliente)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("El nombre es requerido.");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("El email es requerido.");
        if (!email.Contains('@')) throw new ArgumentException("El email no tiene un formato válido.");

        return new Usuario
        {
            Id = 0,
            Nombre = nombre.Trim(),
            Apellido = apellido.Trim(),
            Email = email.ToLowerInvariant().Trim(),
            PasswordHash = passwordHash,
            Rol = rol,
            EstaActivo = true,
            CreadoEn = DateTime.UtcNow
        };
    }

    public void ActualizarPerfil(string nombre, string apellido)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("El nombre es requerido.");
        Nombre = nombre.Trim();
        Apellido = apellido.Trim();
    }

    public void CambiarPassword(string nuevoPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(nuevoPasswordHash)) throw new ArgumentException("El hash es requerido.");
        PasswordHash = nuevoPasswordHash;
    }

    public bool EsAdministrador() => Rol == RolUsuario.Administrador;

    public void Desactivar() => EstaActivo = false;
}
