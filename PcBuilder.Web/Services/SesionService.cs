using PcBuilder.Application.DTOs;

namespace PcBuilder.Web.Services;

public class SesionService
{
    private string? _token;
    private UsuarioResponse? _usuario;

    public bool EstaAutenticado => _token is not null;
    public UsuarioResponse? Usuario => _usuario;
    public string? Token => _token;

    public event Action? OnChange;

    public void IniciarSesion(LoginResponse response)
    {
        _token = response.Token;
        _usuario = response.Usuario;
        OnChange?.Invoke();
    }

    public void CerrarSesion()
    {
        _token = null;
        _usuario = null;
        OnChange?.Invoke();
    }
}
