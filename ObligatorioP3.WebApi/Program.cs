using Microsoft.OpenApi;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ObligatorioP3.Infraestructura.AccesoDatos.EF;
using ObligatorioP3.Infraestructura.EF.AccesoDatos;
using ObligatorioP3.LogicaAplicacion.CasosDeUso.Categorias;
using ObligatorioP3.LogicaAplicacion.CasosDeUso.Historias;
using ObligatorioP3.LogicaAplicacion.CasosDeUso.Lecturas;
using ObligatorioP3.LogicaAplicacion.CasosDeUso.Rankings;
using ObligatorioP3.LogicaAplicacion.CasosDeUso.Usuarios;
using ObligatorioP3.LogicaAplicacion.DTOs.Categorias;
using ObligatorioP3.LogicaAplicacion.DTOs.Historias;
using ObligatorioP3.LogicaAplicacion.DTOs.Lecturas;
using ObligatorioP3.LogicaAplicacion.DTOs.Usuarios;
using ObligatorioP3.LogicaAplicacion.DTOs.Rankings;
using ObligatorioP3.LogicaNegocio.InterfacesLogicaAplicacion;
using ObligatorioP3.LogicaNegocio.InterfacesRepositorio;
using ObligatorioP3.WebApi.Services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Escoge tu propia aventura - WebApi",
        Version = "v1",
        Description = "API de lectura interactiva y estadísticas de historias."
    });

    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT obtenido mediante POST /api/Login, sin escribir Bearer."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});

string connectionString = builder.Configuration
    .GetConnectionString("ObligatorioP3DB")
    ?? throw new InvalidOperationException(
        "No se encontró la cadena de conexión ObligatorioP3DB.");

builder.Services.AddDbContext<ObligatorioP3Context>(options =>
    options.UseSqlServer(connectionString));
//Repos
builder.Services.AddScoped<IRepositorioHistoriasPublicadas, RepositorioHistoriasPublicadas>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
builder.Services.AddScoped<IRepositorioHistoria, RepositorioHistoria>();
builder.Services.AddScoped<IRepositorioLectura, RepositorioLectura>();
builder.Services.AddScoped<IRepositorioRankingHistorias,RepositorioRankingHistorias>();
builder.Services.AddScoped<IRepositorioEstadisticasFinales,RepositorioEstadisticasFinales>();
builder.Services.AddScoped<IRepositorioRankingLectores,RepositorioRankingLectores>();

//Casos de Uso
builder.Services.AddScoped<IListarHistoriasPublicadas<DtoHistoriaPublicada>,ListarHistoriasPublicadas>();
builder.Services.AddScoped<IAutenticarUsuarioApi<DtoUsuarioAutenticado>, AutenticarUsuarioApi>();
builder.Services.AddScoped<IListarTodos<DtoCategoria>, ListarTodasLasCategorias>();
builder.Services.AddScoped<IIniciarLectura<DtoLecturaActual>, IniciarLectura>();
builder.Services.AddScoped<IConsultarLectura<DtoLecturaActual>, ConsultarLectura>();
builder.Services.AddScoped<IElegirOpcionLectura<DtoLecturaActual>, ElegirOpcionLectura>();
builder.Services.AddScoped<IConsultarHistorialLecturas<DtoLecturaHistorial>,ConsultarHistorialLecturas>();
builder.Services.AddScoped<IObtenerRankingHistorias<DtoRankingHistoria>,ObtenerRankingHistorias>();
builder.Services.AddScoped<IConsultarEstadisticasFinales<DtoEstadisticasFinales>,ConsultarEstadisticasFinales>();
builder.Services.AddScoped<IListarHistoriasParaEstadisticas<DtoHistoriaParaEstadisticas>,ListarHistoriasParaEstadisticas>();
builder.Services.AddScoped<IObtenerRankingLectores<DtoRankingLector>,ObtenerRankingLectores>();

JwtSettings jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
    ?? throw new InvalidOperationException("Falta la configuración JwtSettings.");

if (string.IsNullOrWhiteSpace(jwtSettings.Issuer) ||
    string.IsNullOrWhiteSpace(jwtSettings.Audience) ||
    Encoding.UTF8.GetByteCount(jwtSettings.Key) < 32 ||
    jwtSettings.ExpirationMinutes <= 0)
    throw new InvalidOperationException("La configuración JWT es inválida.");

builder.Services.AddSingleton(jwtSettings);
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Escoge tu propia aventura - v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
