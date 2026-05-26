using System.Net.Http.Json;
using System.Text.Json;

namespace PcBuilder.Web.Services;

public class ApiClient(HttpClient http, SesionService sesion)
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private void AgregarToken()
    {
        http.DefaultRequestHeaders.Authorization = sesion.Token is not null
            ? new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", sesion.Token)
            : null;
    }

    public async Task<T?> GetAsync<T>(string url, CancellationToken ct = default)
    {
        AgregarToken();
        var response = await http.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(JsonOpts, ct);
    }

    public async Task<T?> PostAsync<T>(string url, object body, CancellationToken ct = default)
    {
        AgregarToken();
        var response = await http.PostAsJsonAsync(url, body, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(JsonOpts, ct);
    }

    public async Task<T?> PutAsync<T>(string url, object body, CancellationToken ct = default)
    {
        AgregarToken();
        var response = await http.PutAsJsonAsync(url, body, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(JsonOpts, ct);
    }

    public async Task PostAsync(string url, object body, CancellationToken ct = default)
    {
        AgregarToken();
        var response = await http.PostAsJsonAsync(url, body, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string url, CancellationToken ct = default)
    {
        AgregarToken();
        var response = await http.DeleteAsync(url, ct);
        response.EnsureSuccessStatusCode();
    }
}
