using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using PcBuilder.Application;
using PcBuilder.Infrastructure;
using PcBuilder.Web.Endpoints;
using PcBuilder.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<PcBuilder.Web.Services.CarritoService>();
builder.Services.AddScoped<PcBuilder.Web.Services.SesionService>();

builder.Services.AddHttpClient<PcBuilder.Web.Services.ApiClient>(client =>
{
    var url = builder.Configuration["ApiBaseUrl"];
    client.BaseAddress = new Uri(string.IsNullOrEmpty(url) ? "http://localhost:8080" : url);
});
var jwtConfig = builder.Configuration.GetSection("Jwt");
var secretKey = Encoding.UTF8.GetBytes(jwtConfig["Key"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateIssuer = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtConfig["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SoloAdmin", policy =>
        policy.RequireRole("Administrador"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PcBuilder API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT. Ejemplo: Bearer {token}"
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PcBuilder API v1"));
}

if (app.Environment.IsDevelopment())
    app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/api/auth")
    .WithTags("Auth")
    .MapAuthEndpoints();

app.MapGroup("/api/componentes")
    .WithTags("Componentes")
    .MapComponenteEndpoints();

app.MapGroup("/api/configuraciones")
    .WithTags("Configuraciones")
    .MapConfiguracionEndpoints();

app.MapGroup("/api/pedidos")
    .WithTags("Pedidos")
    .MapPedidoEndpoints();

app.MapGroup("/api/usuarios")
    .WithTags("Usuarios")
    .MapUsuarioEndpoints();

app.MapRazorComponents<PcBuilder.Web.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();