using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using PcBuilder.Application.DTOs;

namespace PcBuilder.Web.Services;

public class SesionService(ProtectedSessionStorage storage)
{
    private string? _token;
    private UsuarioResponse? _usuario;
    private bool _inicializado;

    public bool EstaAutenticado => _token is not null;
    public UsuarioResponse? Usuario => _usuario;
    public string? Token => _token;
    public bool EsAdministrador => _usuario?.Rol == "Administrador";
    public string NombreCompleto => _usuario?.NombreCompleto ?? string.Empty;

    public event Action? OnChange;

    public async Task InicializarAsync()
    {
        if (_inicializado) return;
        _inicializado = true;

        try
        {
            var tokenResult = await storage.GetAsync<string>("token");
            var usuarioResult = await storage.GetAsync<UsuarioResponse>("usuario");

            _token = tokenResult.Success && !string.IsNullOrWhiteSpace(tokenResult.Value)
                ? tokenResult.Value
                : null;

            _usuario = usuarioResult.Success ? usuarioResult.Value : null;
            OnChange?.Invoke();
        }
        catch { }
    }

    public async Task IniciarSesionAsync(LoginResponse response)
    {
        _token = response.Token;
        _usuario = response.Usuario;

        try
        {
            await storage.SetAsync("token", _token);
            await storage.SetAsync("usuario", _usuario!);
        }
        catch { }

        OnChange?.Invoke();
    }

    public async Task CerrarSesionAsync()
    {
        _token = null;
        _usuario = null;

        try
        {
            await storage.DeleteAsync("token");
            await storage.DeleteAsync("usuario");
        }
        catch { }

        OnChange?.Invoke();
    }

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
